using Lumos;
using Lumos.BLL.Service.Term;
using System.Web.Http;


namespace WebTermApi.Controllers
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


        [HttpGet]
        public OwnApiHttpResponse UpdateInfo([FromBody]RopMachineUpdateInfo rop)
        {
            IResult result = TermServiceFactory.Machine.UpdateInfo(GuidUtil.Empty(), rop);
            return new OwnApiHttpResponse(result);
        }

    }
}