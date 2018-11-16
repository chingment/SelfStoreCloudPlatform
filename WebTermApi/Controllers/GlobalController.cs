using Lumos;
using Lumos.BLL.Service.AppTerm;
using System.Web;
using System.Web.Http;
namespace WebApiTerm.Controllers
{
    [OwnApiAuthorize]
    public class GlobalController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse DataSet([FromUri]RupGlobalDataSet rup)
        {
            IResult result = TermServiceFactory.Global.DataSet(rup.MerchantId, rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public OwnApiHttpResponse PostFile()
        {
            var s = this;
            HttpContextBase context = (HttpContextBase)Request.Properties["MS_HttpContext"];//获取传统context
            HttpRequestBase request = context.Request;//定义传统request对象 

            if (request.Files != null)
            {
                for (int i = 0; i < request.Files.Count; i++)
                {
                    string c = request.Files[i].FileName;

                    LogUtil.Info("file name:" + c);
                }
            }

            //string result = "";
            //if (FileContent != null)
            //{
            //    //String Tpath = "/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";
            //    //string FilePath = 保存路径 + "\\" + Tpath + "\\";
            //    //String Err = "";
            //    //string FileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //    //bool upres = FileUtil.WriteFile(FilePath, FileContent, FileName, out Err);
            //    //if (upres)
            //    //{
            //    //    result = (Tpath + FileName).Replace("\\", "/");
            //    //}
            //    //else
            //    //{
            //    //    result = "上传文件写入失败：" + Err;
            //    //}
            //}
            //else
            //{
            //    result = "上传的文件信息不存在！";
            //}
            return null;
        }
    }
}