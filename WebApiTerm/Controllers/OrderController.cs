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
        public OwnApiHttpResponse Reserve([FromUri]RupOrderReserve rup , [FromBody]RopOrderReserve rop)
        {
            IResult result = TermServiceFactory.Order.Reserve(rup, rop);
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
        public OwnApiHttpResponse GetPickupList([FromUri]RupOrderGetPickupList rup)
        {
            IResult result = TermServiceFactory.Order.GetPickupList(rup.MerchantId, rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse PickupStatusQuery([FromUri]RupOrderPickupStatusQuery rup)
        {
            IResult result = TermServiceFactory.Order.PickupStatusQuery(rup.MerchantId, rup);
            return new OwnApiHttpResponse(result);
        }


        [HttpPost]
        public OwnApiHttpResponse PickupStatusNotify([FromBody]RopOrderPickupStatusNotify rop)
        {
            IResult result = TermServiceFactory.Order.PickupStatusNotify(rop.MerchantId, rop);
            return new OwnApiHttpResponse(result);
        }
    }
}