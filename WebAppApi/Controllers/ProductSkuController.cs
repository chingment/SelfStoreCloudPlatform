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
    public class ProductSkuController : OwnApiBaseController
    {

        [HttpGet]
        public OwnApiHttpResponse List([FromUri]RupProductSkuList rup)
        {
            var model = AppServiceFactory.ProductSku.List(this.CurrentUserId, this.CurrentUserId, rup);

            OwnApiHttpResult result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new OwnApiHttpResponse(result);

        }


        [HttpGet]
        public OwnApiHttpResponse Details([FromUri]RupProductSkuDetails rup)
        {
            var model = AppServiceFactory.ProductSku.Details(rup.SkuId);

            OwnApiHttpResult result = new OwnApiHttpResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new OwnApiHttpResponse(result);
        }

    }
}