using Lumos;
using Lumos.BLL.Service.AppTerm;
using System.IO;
using System.Web;
using System.Web.Http;


namespace WebApiTerm.Controllers
{
    [OwnApiAuthorize]
    public class MachineController : OwnApiBaseController
    {

        [HttpGet]
        public OwnApiHttpResponse GetSlotSkuStock(RupMachineGetSlotSkuStock rup)
        {
            IResult result = TermServiceFactory.Machine.GetSlotSkusStock(rup.MerchantId, rup.MachineId);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse UpdateInfo(RopMachineUpdateInfo rop)
        {
            IResult result = TermServiceFactory.Machine.UpdateInfo(rop);
            return new OwnApiHttpResponse(result);
        }


        [HttpPost]
        public OwnApiHttpResponse Login(RopMachineLogin rop)
        {
            IResult result = TermServiceFactory.Machine.Login(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public OwnApiHttpResponse UpLoadLog()
        {
            LogUtil.Info("进入UpLoadLog");

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


            if (request.Form.AllKeys != null)
            {
                for (int i = 0; i < request.Form.AllKeys.Length; i++)
                {
                    string key = request.Form.GetKey(i);
                    string value = request.Form[i];
                    LogUtil.Info("file name:" + key + ":" + value);
                }
            }

            var file = request.Files[0];
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string fileName = GuidUtil.New();
            string filePath = HttpContext.Current.Server.MapPath("/") + ("/log-data-app/");
            string path = filePath + fileName + fileExtension;//获取存储的目标地址
            file.SaveAs(path);

            IResult result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            return new OwnApiHttpResponse(result);
        }
    }
}