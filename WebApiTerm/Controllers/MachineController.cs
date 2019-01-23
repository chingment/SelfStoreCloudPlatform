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
            IResult result = TermServiceFactory.Machine.ApiConfig(GuidUtil.Empty(), rup.DeviceId);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse GetSlotSkuStock([FromUri]RupMachineGetSlotSkuStock rup)
        {
            IResult result = TermServiceFactory.Machine.GetSlotSkusStock(rup.MerchantId, rup.MerchantId, rup.MachineId);
            return new OwnApiHttpResponse(result);
        }


        [HttpPost]
        public OwnApiHttpResponse UpdateInfo([FromBody]RopMachineUpdateInfo rop)
        {
            IResult result = TermServiceFactory.Machine.UpdateInfo(GuidUtil.Empty(), rop);
            return new OwnApiHttpResponse(result);
        }

        [HttpGet]
        public OwnApiHttpResponse LoginResultQuery([FromUri]RupMachineLoginResultQuery rup)
        {
            IResult result = TermServiceFactory.Machine.LoginResultQuery(GuidUtil.Empty(), rup);
            return new OwnApiHttpResponse(result);
        }

    }
}