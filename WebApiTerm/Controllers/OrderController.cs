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
        public OwnApiHttpResponse Reserve(RopOrderReserve rop)
        {
            IResult result = TermServiceFactory.Order.Reserve(rop.MerchantId, rop);
            return new OwnApiHttpResponse(result);

        }

        [HttpPost]
        public OwnApiHttpResponse<RetOrderPayResultQuery> PayResultQuery(RupOrderPayResultQuery rup)
        {
            IResult<RetOrderPayResultQuery> result = TermServiceFactory.Order.PayResultQuery(rup.MerchantId, rup);
            return new OwnApiHttpResponse<RetOrderPayResultQuery>(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Cancle(RopOrderCancle rop)
        {
            IResult result = TermServiceFactory.Order.Cancle(rop.MerchantId, rop);
            return new OwnApiHttpResponse(result);
        }


        [HttpGet]
        public OwnApiHttpResponse PickupList([FromUri]RupOrderSkuStatusQuery rup)
        {
            IResult result = TermServiceFactory.Order.SkuStatusQuery(rup.MerchantId, rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse PickupStatusQuery([FromUri]RupOrderSkuStatusQuery rup)
        {
            IResult result = TermServiceFactory.Order.SkuStatusQuery(rup.MerchantId, rup);
            return new OwnApiHttpResponse(result);
        }


        [HttpGet]
        public OwnApiHttpResponse PickupStatusNotify([FromUri]RupOrderSkuStatusQuery rup)
        {
            IResult result = TermServiceFactory.Order.SkuStatusQuery(rup.MerchantId, rup);
            return new OwnApiHttpResponse(result);
        }
    }
}