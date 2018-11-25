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

            var machine = CurrentDb.Machine.Where(m => m.MerchantId == rup.MerchantId && m.Id == rup.MachineId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未绑定商户");
            }

            ret.LogoImgUrl = machine.LogoImgUrl;
            ret.BtnBuyImgUrl = machine.BtnBuyImgUrl;
            ret.BtnPickImgUrl = machine.BtnPickImgUrl;


            ret.Banners = TermServiceFactory.Machine.GetBanners(operater, rup.MerchantId, rup.MachineId);
            ret.ProductKinds = TermServiceFactory.ProductKind.GetKinds(operater, rup.MerchantId, rup.MachineId);
            ret.ProductSkus = TermServiceFactory.Machine.GetProductSkus(operater, rup.MerchantId, rup.MachineId);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }
    }
}
