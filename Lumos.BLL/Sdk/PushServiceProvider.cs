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
            PushService.Send<List<BannerModel>>(machine.JPushRegId, "update_banner", banners);
        }
    }
}
