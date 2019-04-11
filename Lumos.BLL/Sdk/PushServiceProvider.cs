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

        public void UpdateMachineBanner(string merchantId)
        {
            var machines = CurrentDb.Machine.Where(m => m.MerchantId == merchantId).ToList();
            foreach (var machine in machines)
            {
                var banners = TermServiceFactory.Machine.GetBanners(merchantId, machine.StoreId, machine.Id);
                PushService.Send(machine.JPushRegId, "update_banner", banners);
            }
        }

        public void UpdateMachineProductKind(string merchantId)
        {
            var machines = CurrentDb.Machine.Where(m => m.MerchantId == merchantId).ToList();
            foreach (var machine in machines)
            {
                var productKinds = TermServiceFactory.Machine.GetProductKinds(machine.MerchantId, machine.StoreId, machine.Id);
                PushService.Send(machine.JPushRegId, "update_product_kinds", productKinds);
            }
        }

        public void UpdateMachineProductSkus(string machineId)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();
            var banners = TermServiceFactory.Machine.GetProductSkus(machine.MerchantId, machine.StoreId, machine.Id);
            PushService.Send(machine.JPushRegId, "update_product_skus", banners);
        }

        public void UpdateMachineLogo(string machineId)
        {

        }

        public void OpenPickupView(string machineId)
        {

        }

    }
}
