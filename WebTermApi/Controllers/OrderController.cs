using Lumos;
using Lumos.BLL.Service.Term;
using System.Web.Mvc;


namespace WebTermApi.Controllers
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
        public OwnApiHttpResponse<RetOrderPayResultQuery> PayResulyQuery(RupOrderPayResultQuery rup)
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
    }
}