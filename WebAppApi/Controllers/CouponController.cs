using Lumos;
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

    public class CouponController : OwnApiBaseController
    {
        [HttpPost]
        public OwnApiHttpResponse My([FromBody]RupCouponMy rup)
        {
            var model = AppServiceFactory.Coupon.My(this.CurrentUserId,this.CurrentUserId, rup);

            OwnApiHttpResult result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new OwnApiHttpResponse(result);
        }
    }
}