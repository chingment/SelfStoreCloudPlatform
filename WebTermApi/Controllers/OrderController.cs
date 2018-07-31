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
        public APIResponse Reserve(OrderReservePms pms)
        {
            IResult result = TermServiceFactory.Order.Reserve(pms.UserId, pms);
            return new APIResponse(result);

        }

        public APIResponse PayQuery(OrderReservePms pms)
        {
            IResult result = TermServiceFactory.Order.Reserve(GuidUtil.Empty(), pms);
            return new APIResponse(result);

        }
    }
}