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
        public void UpdateMachineBanner(string machineId)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();
            var banners = TermServiceFactory.Machine.GetBanners(machine.MerchantId, machine.StoreId, machine.Id);
            PushService.Send(machine.JPushRegId, "update_banner", banners);
        }

        public void UpdateMachineProductKind()
        {
            
            var machines = CurrentDb.Machine.ToList();
            foreach (var machine in machines)
            {
                var banners = TermServiceFactory.Machine.GetProductKinds(machine.MerchantId, machine.StoreId, machine.Id);
                PushService.Send(machine.JPushRegId, "update_product_kinds", banners);
            }
        }

        public void UpdateMachineProductSkus(string machineId)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();
            var banners = TermServiceFactory.Machine.GetProductSkus(machine.MerchantId, machine.StoreId, machine.Id);
            PushService.Send(machine.JPushRegId, "update_product_skus", banners);
        }
    }
}
