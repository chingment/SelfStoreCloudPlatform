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
    public class MachineController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse ApiConfig(string deviceId)
        {
            IResult result = TermServiceFactory.Machine.ApiConfig("0", deviceId);
            return new APIResponse(result);

        }
    }
}