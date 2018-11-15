using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RopMachineLoginByQrCode
    {
        public string Token { get; set; }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string MachineId { get; set; }

        public string LoginUserId { get; set; }
    }
}
