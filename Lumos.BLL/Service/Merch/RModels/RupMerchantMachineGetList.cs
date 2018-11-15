using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RupMerchantMachineGetList : RupBaseGetList
    {
        public string DeviceId { get; set; }

        public string MerchantId { get; set; }
    }
}
