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
using WebTermApi.Models;
namespace WebTermApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class GlobalController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse DataSet(string merchantId, string machineId, DateTime? datetime)
        {
            IResult result = TermServiceFactory.Global.DataSet("0", merchantId, machineId, datetime);
            return new APIResponse(result);
        }
    }
}