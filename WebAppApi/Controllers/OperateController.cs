using Lumos.BLL;
using Lumos.Entity;
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
    public class OperateController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse GetResult([FromUri]RupOperateGetResult rup)
        {
            IResult result = AppServiceFactory.Operate.GetResult(this.CurrentUserId, this.CurrentUserId, rup);

            return new OwnApiHttpResponse(result);
        }
    }
}