using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class RopMachineUpdateInfo
    {
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string MachineId { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string JPushRegId { get; set; }
    }
}
