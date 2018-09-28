using Lumos;
using Lumos.BLL.Service.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAppApi.Controllers
{
    public class StoreController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse List([FromUri]RupStoreList rup)
        {
            var model = AppServiceFactory.Store.List(this.CurrentUserId, this.CurrentUserId, rup);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);

        }
    }
}
