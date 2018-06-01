using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term.Models.Machine
{
    public class ApiConfigModel
    {
        public int MerchantId { get; set; }
        public int MachineId { get; set; }
        public string ApiHost { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public int PayTimeout { get; set; }
    }
}
