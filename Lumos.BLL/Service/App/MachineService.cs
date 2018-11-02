using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class MachineService : BaseProvider
    {
        public CustomJsonResult LoginByQrCode(string pOperater, string pClientId, RopMachineLoginByQrCode rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            Biz.RModels.RopMachineLoginByQrCode bizRop = new Biz.RModels.RopMachineLoginByQrCode();

            bizRop.LoginUserId = pClientId;
            bizRop.Token = rop.Token;
            bizRop.MerchantId = rop.MerchantId;
            bizRop.StoreId = rop.StoreId;
            bizRop.MachineId = rop.MachineId;


            result = BizFactory.Machine.LoginByQrCode(pOperater, bizRop);

            return result;
        }


        public CustomJsonResult GetInfoByLoginBefore(string pOperater, string pClientId, RopMachineLoginByQrCode rop)
        {


        }
    }
}
