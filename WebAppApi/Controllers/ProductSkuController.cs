using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class ProductSkuController : OwnBaseApiController
    {

        [HttpGet]
        public APIResponse List(RupProductSkuList rup)
        {
            var model = AppServiceFactory.ProductSku.List(this.CurrentUserId, this.CurrentUserId, rup);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);

        }


        [HttpGet]
        public APIResponse Details(RupProductSkuDetails rup)
        {
            var model = AppServiceFactory.ProductSku.Details(rup.SkuId);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }

    }
}