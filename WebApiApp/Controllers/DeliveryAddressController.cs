﻿using Lumos;
using Lumos.BLL.Service.ApiApp;
using System.Web.Http;

namespace WebAppApi.Controllers
{
    public class DeliveryAddressController : OwnApiBaseController
    {

        [HttpGet]
        public OwnApiHttpResponse My()
        {
            var list = AppServiceFactory.UserDeliveryAddress.My(this.CurrentUserId, this.CurrentUserId);

            OwnApiHttpResult result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = list };

            return new OwnApiHttpResponse(result);
        }

        [HttpPost]
        public OwnApiHttpResponse Edit([FromBody]RopUserDeliveryAddressEdit rop)
        {
            IResult result = AppServiceFactory.UserDeliveryAddress.Edit(this.CurrentUserId, this.CurrentUserId, rop);
            return new OwnApiHttpResponse(result);
        }
    }
}