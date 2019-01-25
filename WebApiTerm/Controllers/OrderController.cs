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
        public OwnApiHttpResponse Reserve([FromUri]RupOrderReserve rup, [FromBody]RopOrderReserve rop)
        {
            IResult result = TermServiceFactory.Order.Reserve(rup, rop);
            return new OwnApiHttpResponse(result);

        }

        [HttpGet]
        public OwnApiHttpResponse<RetOrderPayResultQuery> PayResultQuery([FromUri]RupOrderPayResultQuery rup)
        {
            IResult<RetOrderPayResultQuery> result = TermServiceFactory.Order.PayResultQuery(rup);
            return new OwnApiHttpResponse<RetOrderPayResultQuery>(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Cancle([FromUri]RupOrderCancle rup, [FromBody]RopOrderCancle rop)
        {
            IResult result = TermServiceFactory.Order.Cancle(rup, rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse PickupSkusList([FromUri]RupOrderPickupSkusList rup)
        {
            IResult result = TermServiceFactory.Order.PickupSkusList(rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse PickupStatusQuery([FromUri]RupOrderPickupStatusQuery rup)
        {
            IResult result = TermServiceFactory.Order.PickupStatusQuery(rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse PickupEventNotify([FromBody]RopOrderPickupEventNotify rop)
        {
            IResult result = TermServiceFactory.Order.PickupEventNotify(rop);
            return new OwnApiHttpResponse(result);
        }
    }
}