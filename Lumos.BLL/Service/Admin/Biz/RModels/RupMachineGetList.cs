using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin.Biz
{
    public class RupMachineGetList:RupBaseGetList
    {
        public string MacAddress { get; set; }

        public string DeviceId { get; set; }

        public string StoreId { get; set; }

        public string MerchantId { get; set; }
    }
}
