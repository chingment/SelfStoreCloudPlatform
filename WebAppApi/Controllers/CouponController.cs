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
using WebAppApi.Models.Coupon;

namespace WebAppApi.Controllers
{

    [BaseAuthorizeAttribute]
    public class CouponController : OwnBaseApiController
    {
        [HttpPost]
        public APIResponse GetList(GetListParams pms)
        {
            var model = AppServiceFactory.Coupon.List(pms.UserId, pms.UserId, pms.IsGetHis, pms.CouponId, pms.Skus);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }
    }
}