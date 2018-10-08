using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class RetMachineApiConfig
    {
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string MachineId { get; set; }
        public string ApiHost { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public int PayTimeout { get; set; }
    }
}
