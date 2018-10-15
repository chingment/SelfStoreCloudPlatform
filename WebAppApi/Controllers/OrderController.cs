using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Lumos.BLL.Service;
using System.Web;
using Lumos;
using Lumos.BLL.Service.App;

namespace WebAppApi.Controllers
{
    public class OrderController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse Confirm([FromBody]RopOrderConfirm rop)
        {
            IResult result = AppServiceFactory.Order.Confrim(this.CurrentUserId,this.CurrentUserId, rop);

            return new APIResponse(result);
        }


        [HttpPost]
        public APIResponse Reserve([FromBody]RopOrderReserve rop)
        {
            IResult result = AppServiceFactory.Order.Reserve(this.CurrentUserId, this.CurrentUserId, rop);
            return new APIResponse(result);

        }


    }
}