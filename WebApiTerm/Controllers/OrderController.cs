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

        [HttpPost]
        public OwnApiHttpResponse PayUrlBuild([FromBody]RopOrderPayUrlBuild rop)
        {
            IResult result = TermServiceFactory.Order.PayUrlBuild(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse<RetOrderPayStatusQuery> PayStatusQuery([FromUri]RupOrderPayStatusQuery rup)
        {
            IResult<RetOrderPayStatusQuery> result = TermServiceFactory.Order.PayStatusQuery(rup);
            return new OwnApiHttpResponse<RetOrderPayStatusQuery>(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Cancle([FromBody]RopOrderCancle rop)
        {
            IResult result = TermServiceFactory.Order.Cancle(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse Details([FromUri]RupOrderDetails rup)
        {
            IResult result = TermServiceFactory.Order.Details(rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse SkuPickupStatusQuery([FromUri]RupOrderSkuPickupStatusQuery rup)
        {
            IResult result = TermServiceFactory.Order.SkuPickupStatusQuery(rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse SkuPickupEventNotify([FromBody]RopOrderSkuPickupEventNotify rop)
        {
            IResult result = TermServiceFactory.Order.SkuPickupEventNotify(rop);
            return new OwnApiHttpResponse(result);
        }
    }
}