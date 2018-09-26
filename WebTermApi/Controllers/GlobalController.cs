using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service.Term;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace WebTermApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class GlobalController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse DataSet([FromUri]RupGlobalDataSet rup)
        {
            IResult result = TermServiceFactory.Global.DataSet(rup.MerchantId, rup.MerchantId, rup.MachineId, rup.Datetime);
            return new APIResponse(result);
        }
    }
}