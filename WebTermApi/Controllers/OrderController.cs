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
        public APIResponse PayResulyQuery(RupPayResultQuery rup)
        {
            IResult result = TermServiceFactory.Order.PayResultQuery(rup.UserId, rup);
            return new APIResponse(result);

        }
    }
}