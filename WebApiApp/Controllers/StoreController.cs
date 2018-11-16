using Lumos;
using Lumos.BLL.Service.AppMobile;
using System.Web.Http;

namespace WebAppApi.Controllers
{
    public class StoreController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse List([FromUri]RupStoreList rup)
        {
            var model = AppServiceFactory.Store.List(this.CurrentUserId, this.CurrentUserId, rup);

            OwnApiHttpResult result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new OwnApiHttpResponse(result);

        }
    }
}
