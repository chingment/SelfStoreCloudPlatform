using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class GlobalService : BaseProvider
    {
        public CustomJsonResult DataSet(string operater, RupGlobalDataSet rup)
        {
            CustomJsonResult result = new CustomJsonResult();
            var ret = new RetGlobalDataSet();

            var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == rup.MerchantId && m.MachineId == rup.MachineId && m.IsBind == true).FirstOrDefault();

            if (merchantMachine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未绑定商户");
            }

            ret.LogoImgUrl = merchantMachine.LogoImgUrl;
            ret.BtnBuyImgUrl = merchantMachine.BtnBuyImgUrl;
            ret.BtnPickImgUrl = merchantMachine.BtnPickImgUrl;


            ret.Banners = TermServiceFactory.Machine.GetBanners(operater, rup.MerchantId, rup.MachineId);
            ret.ProductKinds = TermServiceFactory.ProductKind.GetKinds(operater, rup.MerchantId, rup.MachineId);
            ret.ProductSkus = TermServiceFactory.Machine.GetProductSkus(operater, rup.MerchantId, rup.MachineId);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }
    }
}
