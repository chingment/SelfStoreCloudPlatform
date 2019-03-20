using Lumos.BLL.Service.AppTerm;
using MySDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class PushServiceProvider : BaseProvider
    {
        public void UpdateMachineBanner(string id)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();
            var banners = TermServiceFactory.Machine.GetBanners(machine.MerchantId, machine.StoreId, machine.Id);
            PushService.Send(machine.JPushRegId, "update_banner", banners);
        }

        public void UpdateMachineProductKind(string id)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();
            var banners = TermServiceFactory.Machine.GetProductKinds(machine.MerchantId, machine.StoreId, machine.Id);
            PushService.Send(machine.JPushRegId, "update_product_kinds", banners);
        }

        public void UpdateMachineProductSkus(string id)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();
            var banners = TermServiceFactory.Machine.GetProductSkus(machine.MerchantId, machine.StoreId, machine.Id);
            PushService.Send(machine.JPushRegId, "update_product_skus", banners);
        }
    }
}
