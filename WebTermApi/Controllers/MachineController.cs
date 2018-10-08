using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service.Term;
using Lumos.BLL.Service.Term.Models;
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
    [OwnAuthorize]
    public class MachineController : OwnBaseApiController
    {
        [HttpGet]
        public APIResponse ApiConfig([FromUri]RupMachineApiConfig rup)
        {
            IResult result = TermServiceFactory.Machine.ApiConfig(GuidUtil.Empty(), rup.DeviceId);
            return new APIResponse(result);

        }

        [HttpGet]
        public APIResponse GetSlotSkuStock([FromUri]RupMachineGetSlotSkuStock rup)
        {
            IResult result = TermServiceFactory.Machine.GetSlotSkusStock(rup.UserId, rup.UserId, rup.MerchantId, rup.MachineId);
            return new APIResponse(result);

        }
    }
}