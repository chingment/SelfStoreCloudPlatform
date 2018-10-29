
using System.Web.Http;
using Lumos;
using Lumos.BLL.Service.App;
using Lumos.WeiXinSdk;

namespace WebAppApi.Controllers
{
    public class OrderController : OwnApiBaseController
    {
        [HttpPost]
        public OwnApiHttpResponse Confirm([FromBody]RopOrderConfirm rop)
        {
            IResult result = AppServiceFactory.Order.Confrim(this.CurrentUserId, this.CurrentUserId, rop);

            return new OwnApiHttpResponse(result);
        }


        [HttpPost]
        public OwnApiHttpResponse Reserve([FromBody]RopOrderReserve rop)
        {
            IResult result = AppServiceFactory.Order.Reserve(this.CurrentUserId, this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }


        [HttpGet]
        public OwnApiHttpResponse GetJsApiPaymentPms([FromUri]RupOrderGetJsApiPaymentPms rop)
        {
            IResult result = AppServiceFactory.Order.GetJsApiPaymentPms(this.CurrentUserId, this.CurrentUserId, this.CurrentAppInfo, rop);
            return new OwnApiHttpResponse(result);
        }
    }
}