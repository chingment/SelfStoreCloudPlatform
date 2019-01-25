using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class GlobalService : BaseService
    {
        public CustomJsonResult DataSet(RupGlobalDataSet rup)
        {
            CustomJsonResult result = new CustomJsonResult();

            var ret = new RetGlobalDataSet();

            var machine = CurrentDb.Machine.Where(m => m.Id == rup.MachineId).FirstOrDefault();

            ret.LogoImgUrl = machine.LogoImgUrl;
            ret.BtnBuyImgUrl = machine.BtnBuyImgUrl;
            ret.BtnPickImgUrl = machine.BtnPickImgUrl;

            ret.Banners = TermServiceFactory.Machine.GetBanners(machine.MerchantId, machine.StoreId, machine.Id);
            ret.ProductKinds = TermServiceFactory.ProductKind.GetKinds(machine.MerchantId, machine.StoreId, machine.Id);
            ret.ProductSkus = TermServiceFactory.Machine.GetProductSkus(machine.MerchantId, machine.StoreId, machine.Id);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }
    }
}
