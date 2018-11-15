using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class RopMerchantAdd
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string MerchantName { get; set; }
        public string ContactName { get; set; }
        public string ContactPhone { get; set; }

        public string ContactAddress { get; set; }
    }
}
