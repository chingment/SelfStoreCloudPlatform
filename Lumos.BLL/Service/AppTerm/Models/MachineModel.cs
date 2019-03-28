using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.Entity;

namespace Lumos.BLL.Service.AppTerm
{
    public class MachineModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MerchantName { get; set; }
        public string StoreName { get; set; }
        public string LogoImgUrl { get; set; }
        public int PayTimeout { get; set; }
        public string Currency { get; set; }
        public string CurrencySymbol { get; set; }
    }
}
