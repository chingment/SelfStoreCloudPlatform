using Lumos;
using Lumos.BLL.Service.AppTerm;
using System.Web;
using System.Web.Http;

namespace WebApiTerm.Controllers
{
    [OwnApiAuthorize]
    public class OrderController : OwnApiBaseController
    {

        [HttpPost]
        public OwnApiHttpResponse Reserve([FromBody]RopOrderReserve rop)
        {
            IResult result = TermServiceFactory.Order.Reserve(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse<RetOrderPayResultQuery> PayResultQuery([FromUri]RupOrderPayResultQuery rup)
        {
            IResult<RetOrderPayResultQuery> result = TermServiceFactory.Order.PayResultQuery(rup);
            return new OwnApiHttpResponse<RetOrderPayResultQuery>(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Cancle([FromBody]RopOrderCancle rop)
        {
            IResult result = TermServiceFactory.Order.Cancle(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse SkusPickupBill(RupOrderSkusPickupBill rup)
        {
            IResult result = TermServiceFactory.Order.SkusPickupBill(rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse SkuPickupStatusQuery(RupOrderSkuPickupStatusQuery rup)
        {
            IResult result = TermServiceFactory.Order.SkuPickupStatusQuery(rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse SkuPickupEventNotify(RopOrderSkuPickupEventNotify rop)
        {
            IResult result = TermServiceFactory.Order.SkuPickupEventNotify(rop);
            return new OwnApiHttpResponse(result);
        }
    }
}