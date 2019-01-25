using Lumos.Common;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using Lumos;
using Lumos.BLL;
using System.Collections.Generic;
using Lumos.Entity;

namespace WebMerch.Controllers
{

    public class CommonController : OwnBaseController
    {
        [HttpPost]
        public ActionResult CkEditorUpLoadFile()
        {
            LogUtil.Info("调用上传图片接口(CkEditorUpLoadFile)");
            string CKEditorFuncNum = Request.QueryString["CKEditorFuncNum"];
            try
            {
                HttpPostedFileBase file_upload = Request.Files["upload"];
                string type = Request.QueryString["type"].ToString();
                string fileName = file_upload.FileName;
                string fileExtension = System.IO.Path.GetExtension(fileName).ToLower();
                StringBuilder sb = new StringBuilder();
                if (fileExtension != ".jpg" && fileExtension != ".gif" && fileExtension != ".png" && fileExtension != ".bmp")
                {
                    return Content(CkEditorUpLoadCallFunction(CKEditorFuncNum, "上传的文件格式不正确,请重新选择！"));
                }

                string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:UploadServerUrl"];

                LogUtil.Info("调用上传图片接口" + strUrl);

                byte[] bytes = null;
                using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                {
                    bytes = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                }
                string fileExt = Path.GetExtension(Request.Files[0].FileName).ToLower();
                UploadFileEntity entity = new UploadFileEntity();
                entity.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;//自定义文件名称，这里以当前时间为例
                entity.FileData = bytes;
                entity.UploadFolder = "CkEditorFile";
                entity.GenerateSize = false;

                CustomJsonResult rm = HttpClientOperate.Post<CustomJsonResult>("", strUrl, entity);//封装的POST提交方
                if (rm.Result == ResultType.Exception || rm.Result == ResultType.Unknown)
                {
                    rm.Message = "上传图片发生异常";
                    LogUtil.Error("调用api上传图片失败");

                    return Content(CkEditorUpLoadCallFunction(CKEditorFuncNum, "远程上传图片发生异常"));
                }

                ImageUpload imageUpload = rm.Data.ToJsonObject<ImageUpload>();

                return Content(CkEditorUpLoadCallFunction(CKEditorFuncNum, "", imageUpload.OriginalPath));
            }
            catch (Exception ex)
            {
                LogUtil.Error("调用上传图片接口(CkEditorUpLoadFile)", ex);
                return Content(CkEditorUpLoadCallFunction(CKEditorFuncNum, ex.Message));
            }
        }

        private string CkEditorUpLoadCallFunction(string CKEditorFuncNum, string message, string imageurl = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type=\"text/javascript\">");
            sb.Append("window.parent.CKEDITOR.tools.callFunction(" + CKEditorFuncNum + ",'" + imageurl + "','" + message + "')");
            sb.Append("</script>");
            return sb.ToString();
        }

        [HttpPost]
        public CustomJsonResult UploadImage(string fileinputname, string path, string oldfilename, bool generateSize)
        {

            CustomJsonResult rm = new CustomJsonResult();
            //rm.ContentType = "text/html";
            try
            {
                LogUtil.Info("调用上传图片接口");

                HttpPostedFileBase file_upload = Request.Files[fileinputname];

                if (file_upload == null)
                    return Json("text/html", ResultType.Failure, "找不到上传的对象");

                if (file_upload.ContentLength == 0)
                    return Json("text/html", ResultType.Failure, "文件内容为空,请重新选择");

                if (file_upload.ContentLength > (10 * 1024 * 1024))
                {
                    return Json("text/html", ResultType.Failure, "图片大小不能超过10M,请重新选择");
                }


                System.IO.FileInfo file = new System.IO.FileInfo(file_upload.FileName);
                string ext = file.Extension.ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png" && ext != ".gif" && ext != ".bmp")
                {
                    return Json("text/html", ResultType.Failure, "上传的文件不是图片格式(jpg,png,gif,bmp)");
                }


                string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:UploadServerUrl"];
                LogUtil.Info("调用上传图片接口" + strUrl);

                byte[] bytes = null;
                using (var binaryReader = new BinaryReader(Request.Files[0].InputStream))
                {
                    bytes = binaryReader.ReadBytes(Request.Files[0].ContentLength);
                }
                string fileExt = Path.GetExtension(Request.Files[0].FileName).ToLower();
                UploadFileEntity entity = new UploadFileEntity();
                entity.FileName = DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;//自定义文件名称，这里以当前时间为例
                entity.FileData = bytes;
                entity.UploadFolder = path;
                entity.GenerateSize = generateSize;
                rm = HttpClientOperate.Post<CustomJsonResult>(path, strUrl, entity);//封装的POST提交方
                //rm.ContentType = "text/html";
                if (rm.Result == ResultType.Exception || rm.Result == ResultType.Unknown)
                {
                    rm.Message = "上传图片发生异常.";
                    LogUtil.Error("调用api上传图片失败");
                }
            }
            catch (Exception ex)
            {
                rm.Result = ResultType.Exception;
                rm.Message = "上传图片发生异常..";
                LogUtil.Error("", ex);

            }
            return rm;
        }

        /// <summary>
        /// 获取验证码的图片 使用方式 请求url:/Common/GetImgVerifyCode?name=sessionname
        /// </summary>
        /// <param name="name">代表后台session的名称</param>
        /// <returns>返回一张带数字的图片</returns>
        [AllowAnonymous]
        public ActionResult GetImgVerifyCode(string name)
        {
            VerifyCodeHelper v = new VerifyCodeHelper();
            v.CodeSerial = "0,1,2,3,4,5,6,7,8,9";
            string code = v.CreateVerifyCode(); //取随机码 
            v.CreateImageOnPage(code, ControllerContext.HttpContext);   //输出图片
            Session[name] = code;   //Session 取出验证码
            Response.End();
            return null;
        }

        public CustomJsonResult GetSelectFields(string type)
        {
            if (type == null)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "类型为空");

            var result = new CustomJsonResult();

            var fields = new List<FieldModel>();

            type = type.ToLower();

            switch (type)
            {
                case "warehouse":
                    #region warehouse
                    var warehouses = CurrentDb.Warehouse.Where(m => m.MerchantId == this.CurrentMerchantId).ToList();

                    foreach (var item in warehouses)
                    {
                        fields.Add(new FieldModel(item.Name, item.Id));
                    }

                    #endregion
                    break;
                case "stockouttarget":
                    #region sysorganization
                    var stores = CurrentDb.Store.Where(m => m.MerchantId == this.CurrentMerchantId).ToList();
                    foreach (var item in stores)
                    {
                        fields.Add(new FieldModel(string.Format("[店铺]{0}", item.Name), item.Id, 2));
                    }
                    #endregion 
                    break;
                case "supplier":
                    #region sysorganization
                    var suppliers = CurrentDb.Company.Where(m => m.Class == Lumos.Entity.Enumeration.CompanyClass.Supplier && m.MerchantId == this.CurrentMerchantId).ToList();

                    foreach (var item in suppliers)
                    {
                        fields.Add(new FieldModel(item.Name, item.Id));
                    }
                    #endregion 
                    break;
                case "recipientmode":
                    #region recipientmode
                    Enumeration.ReceptionMode[] models = new Enumeration.ReceptionMode[2] { Enumeration.ReceptionMode.Machine, Enumeration.ReceptionMode.Express };

                    foreach (Enumeration.ReceptionMode t in models)
                    {
                        string strKey = Convert.ToInt32(t).ToString();
                        Enum en = (Enum)Enum.Parse(t.GetType(), strKey);
                        string strValue = en.GetCnName();
                        fields.Add(new FieldModel(strValue, strKey));

                    }
                    #endregion
                    break;
                case "productkind":
                    #region 
                    var productKinds = CurrentDb.ProductKind.Where(m => m.MerchantId == this.CurrentMerchantId).Where(m => m.IsDelete == false).OrderBy(m => m.Priority).ToList();

                    foreach (var item in productKinds)
                    {
                        var field = new FieldModel();
                        field.Name = item.Name;
                        field.Value = item.Id;
                        field.PValue = item.PId;
                        field.Dept = item.Dept;
                        if (item.Dept <= 1)
                        {
                            field.Disabled = true;
                        }
                        else
                        {
                            field.Disabled = false;
                        }
                        fields.Add(field);
                    }
                    #endregion
                    break;
                case "productkindstatus":
                    #region productkindstatus
                    foreach (Enumeration.ProductKindStatus t in Enum.GetValues(typeof(Enumeration.ProductKindStatus)))
                    {
                        string strKey = Convert.ToInt32(t).ToString();
                        Enum en = (Enum)Enum.Parse(t.GetType(), strKey);
                        string strValue = en.GetCnName();
                        if (strKey != "0")
                        {
                            fields.Add(new FieldModel(strValue, strKey));
                        }
                    }
                    #endregion
                    break;
                case "productsubject":
                    #region productsubject
                    var productSubjects = CurrentDb.ProductSubject.Where(m => m.MerchantId == this.CurrentMerchantId).Where(m => m.IsDelete == false).OrderBy(m => m.Priority).ToList();

                    foreach (var item in productSubjects)
                    {
                        var field = new FieldModel();
                        field.Name = item.Name;
                        field.Value = item.Id;
                        field.PValue = item.PId;
                        field.Dept = item.Dept;
                        if (item.Dept <= 0)
                        {
                            field.Disabled = true;
                        }
                        else
                        {
                            field.Disabled = false;
                        }
                        fields.Add(field);
                    }
                    #endregion
                    break;
                case "productsubjectstatus":
                    #region
                    foreach (Enumeration.ProductSubjectStatus t in Enum.GetValues(typeof(Enumeration.ProductSubjectStatus)))
                    {
                        string strKey = Convert.ToInt32(t).ToString();
                        Enum en = (Enum)Enum.Parse(t.GetType(), strKey);
                        string strValue = en.GetCnName();
                        if (strKey != "0")
                        {
                            fields.Add(new FieldModel(strValue, strKey));
                        }
                    }
                    #endregion
                    break;
                case "sysposition":
                    #region sysposition
                    var sysPositions = CurrentDb.SysPosition.Where(m => m.BelongSite == Enumeration.BelongSite.Merchant).ToList();
                    foreach (var item in sysPositions)
                    {
                        fields.Add(new FieldModel(item.Name, ((int)item.Id).ToString()));
                    }
                    #endregion
                    break;

            }



            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", fields);

        }
    }
}