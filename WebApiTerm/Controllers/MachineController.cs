using Lumos;
using Lumos.BLL.Service.AppTerm;
using System.Web.Http;


namespace WebApiTerm.Controllers
{
    [OwnApiAuthorize]
    public class MachineController : OwnApiBaseController
    {
        [HttpGet]
        public OwnApiHttpResponse ApiConfig([FromUri]RupMachineApiConfig rup)
        {
            IResult result = TermServiceFactory.Machine.ApiConfig(rup);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse GetSlotSkuStock(RupMachineGetSlotSkuStock rup)
        {
            IResult result = TermServiceFactory.Machine.GetSlotSkusStock(rup.MerchantId, rup.MachineId);
            return new OwnApiHttpResponse(result);
        }


        [HttpPost]
        public OwnApiHttpResponse UpdateInfo(RopMachineUpdateInfo rop)
        {
            IResult result = TermServiceFactory.Machine.UpdateInfo(rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse LoginResultQuery(RupMachineLoginResultQuery rup)
        {
            IResult result = TermServiceFactory.Machine.LoginResultQuery(rup);
            return new OwnApiHttpResponse(result);
        }

    }
}