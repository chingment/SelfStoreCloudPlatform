using Lumos.Common;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace WebUploadImageServer.Controllers
{
    public class UploadFileController : BaseApiController
    {
        public HttpResponseMessage Post(UploadFileEntity entity)
        {
            SetTrackID();
            Log.Info("调用UploadFile");
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象 
            CustomJsonResult r = new CustomJsonResult();
            try
            {

                if (entity.FileData == null)
                {
                    r = new CustomJsonResult(ResultType.Failure, "文件对象为空");
                }
                else if (entity.FileData.Length == 0)
                {
                    r = new CustomJsonResult(ResultType.Failure, "文件内容为空");
                }
                else if (entity.FileData.Length > (50 * 1024 * 1024))
                {
                    r = new CustomJsonResult(ResultType.Failure, "文件大小不能超过50M");
                }
                else 
                {

                    string imageSign = "";

                    string savefolder = CommonSetting.GetUploadPath(entity.UploadFolder);
                    string extension = Path.GetExtension(entity.FileName).ToLower();
                    string yyyyMMddhhmmssfff = Guid.NewGuid().ToString();
                    string originalNewfilename = imageSign + yyyyMMddhhmmssfff + extension;//原图片文件名称

                    string originalSavePath = string.Format("{0}/{1}", savefolder, originalNewfilename);

                    string path = System.Configuration.ConfigurationManager.AppSettings["custom:ImagesServerUploadPath"];

                    string serverOriginalSavePath = path + originalSavePath;


                    entity.FileName = entity.FileName.ToLower().Replace("\\", "/");

                    ImageUpload s = new ImageUpload();
                    string domain = System.Configuration.ConfigurationManager.AppSettings["custom:ImagesServerUrl"];




                    DirectoryInfo Drr = new DirectoryInfo(path + savefolder);
                    if (!Drr.Exists)
                    {
                        Drr.Create();
                    }

                    FileStream fs = new FileStream(serverOriginalSavePath, FileMode.Create, FileAccess.Write);
                    fs.Write(entity.FileData, 0, entity.FileData.Length);
                    fs.Flush();
                    fs.Close();

                    string url = domain + originalSavePath;
                    string name = entity.FileName == null ? "" : entity.FileName;
                    r.Data = "{\"url\":\"" + url + "\",\"name\":\"" + name + "\",\"referenceId\":\"" + entity.ReferenceId + "\"}";
                    r.Message = "上传成功";
                    r.Result = ResultType.Success;
                }

            }
            catch (Exception ex)
            {
                r.Result = ResultType.Exception;
                r.Message = "上传失败";
                Log.Error("WebApi上传图片异常", ex);

            }

            string json = r.ToString();
            HttpResponseMessage result = new HttpResponseMessage { Content = new StringContent(json, Encoding.GetEncoding("UTF-8"), "application/json") };
            return result;
        }
    }
}