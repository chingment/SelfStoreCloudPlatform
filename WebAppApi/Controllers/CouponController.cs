﻿using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace WebAppApi.Controllers
{

    [BaseAuthorizeAttribute]
    public class CouponController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse My(RupCouponMy rup)
        {
            var model = AppServiceFactory.Coupon.List(this.CurrentUserId,this.CurrentUserId, rup);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }
    }
}