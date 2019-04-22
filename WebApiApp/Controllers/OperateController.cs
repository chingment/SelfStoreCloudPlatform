using System.Web.Http;
using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service.ApiApp;

namespace WebAppApi.Controllers
{
    public class OperateController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetResult([FromUri]RupOperateGetResult rup)
        {
            IResult result = AppServiceFactory.Operate.GetResult(this.CurrentUserId, this.CurrentUserId, rup);

            return new OwnApiHttpResponse(result);
        }
    }
}