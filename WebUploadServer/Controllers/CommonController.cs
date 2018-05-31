using log4net;
using Lumos.Common;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUploadImageServer.Controllers
{
    public class CommonController : BaseController
    {
        //


        public CustomJsonResult UploadImage(string fileInputName, string path, string oldFileName)
        {
            ILog log = LogManager.GetLogger(this.GetType());
            log.Info("测试上传服务");
            CustomJsonResult rm = new CustomJsonResult();
           // rm.ContentType = "text/html";
            try
            {
                HttpPostedFileBase file_upload = Request.Files[fileInputName];

                if (file_upload == null)
                    return Json("text/html", ResultType.Failure, "上传失败");

                System.IO.FileInfo file = new System.IO.FileInfo(file_upload.FileName);
                if (file.Extension != ".jpg" && file.Extension != ".png" && file.Extension != ".gif" && file.Extension != ".bmp")
                {
                    return Json("text/html", ResultType.Failure, "上传的文件不是图片格式(jpg,png,gif,bmp)");
                }


                string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:ImagesServerUploadUrl"] + "?date=" + DateTime.Now.ToString("yyyyMMddhhmmssfff");


                byte[] bytes = null;
                using (var binaryReader = new BinaryReader(file_upload.InputStream))
                {
                    bytes = binaryReader.ReadBytes(file_upload.ContentLength);
                }
                string fileExt = Path.GetExtension(file_upload.FileName).ToLower();
                UploadFileEntity entity = new UploadFileEntity();
                entity.FileName = DateTime.Now.ToString("yyyyMMddhhmmssfff") + fileExt;//自定义文件名称，这里以当前时间为例
                entity.FileData = bytes;
                entity.UploadFolder = path;
                rm = HttpClientOperate.Post<CustomJsonResult>(path, strUrl, entity);//封装的POST提交方
                //rm.ContentType = "text/html";
                if (rm.Result == ResultType.Exception || rm.Result == ResultType.Unknown)
                {
                    rm.Message = "上传图片发生异常";
                }
            }
            catch (Exception ex)
            {
                log.Error("上传图片发生异常", ex);
                rm.Result = ResultType.Exception;
                rm.Message = "上传图片发生异常";
                
                throw (ex);
            }
            return rm;

        }
	}
}