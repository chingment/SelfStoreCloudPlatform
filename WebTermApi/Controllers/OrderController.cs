using Lumos;
using Lumos.BLL.Service.Term;
using Lumos.BLL.Service.Term.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebTermApi.Controllers
{
    public class OrderController : OwnBaseApiController
    {

        [HttpPost]
        public APIResponse Reserve(RopOrderReserve rop)
        {
            IResult result = TermServiceFactory.Order.Reserve(rop.MerchantId, rop);
            return new APIResponse(result);

        }

        [HttpPost]
        public APIResponse<RetOrderPayResultQuery> PayResulyQuery(RupOrderPayResultQuery rup)
        {
            IResult<RetOrderPayResultQuery> result = TermServiceFactory.Order.PayResultQuery(rup.MerchantId, rup);
            return new APIResponse<RetOrderPayResultQuery>(result);
        }

        [HttpPost]
        public APIResponse Cancle(RopOrderCancle rop)
        {
            IResult result = TermServiceFactory.Order.Cancle(rop.MerchantId, rop);
            return new APIResponse(result);
        }


    }
}