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

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未入库登记");
            }

            if (string.IsNullOrEmpty(machine.MerchantId))
            {
                return new CustomJsonResult(ResultType.Failure, "未绑定商户");
            }

            var merchant = CurrentDb.Merchant.Where(m => m.Id == machine.MerchantId).FirstOrDefault();

            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, "商户不存在");
            }

            if (string.IsNullOrEmpty(machine.StoreId))
            {
                return new CustomJsonResult(ResultType.Failure, "未绑定店铺");
            }

            var store = CurrentDb.Store.Where(m => m.Id == machine.StoreId).FirstOrDefault();

            if (store == null)
            {
                return new CustomJsonResult(ResultType.Failure, "店铺不存在");
            }

            ret.Machine.Id = machine.Id;
            ret.Machine.Name = machine.Name;
            ret.Machine.MerchantName = merchant.Name;
            ret.Machine.StoreName = store.Name;
            ret.Machine.PayTimeout = merchant.PayTimeout;
            ret.Machine.LogoImgUrl = machine.LogoImgUrl;
      
            ret.Banners = TermServiceFactory.Machine.GetBanners(machine.MerchantId, machine.StoreId, machine.Id);
            ret.ProductKinds = TermServiceFactory.ProductKind.GetKinds(machine.MerchantId, machine.StoreId, machine.Id);
            ret.ProductSkus = TermServiceFactory.Machine.GetProductSkus(machine.MerchantId, machine.StoreId, machine.Id);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }
    }
}
