using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RupMachineGetList:RupBaseGetList
    {
        public string MacAddress { get; set; }

        public string DeviceId { get; set; }

        public string StoreId { get; set; }
    }
}
