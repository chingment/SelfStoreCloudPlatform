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

            var tk = GetTicketInfo(rup.Ticket);

            var ret = new RetGlobalDataSet();

            var machine = CurrentDb.Machine.Where(m => m.MerchantId == tk.MerchantId && m.StoreId == tk.StoreId && m.Id == tk.MachineId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未绑定商户");
            }

            ret.LogoImgUrl = machine.LogoImgUrl;
            ret.BtnBuyImgUrl = machine.BtnBuyImgUrl;
            ret.BtnPickImgUrl = machine.BtnPickImgUrl;


            ret.Banners = TermServiceFactory.Machine.GetBanners(tk.MerchantId, tk.MerchantId, tk.MachineId);
            ret.ProductKinds = TermServiceFactory.ProductKind.GetKinds(tk.MerchantId, tk.MerchantId, tk.MachineId);
            ret.ProductSkus = TermServiceFactory.Machine.GetProductSkus(tk.MerchantId, tk.MerchantId, tk.MachineId);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }
    }
}
