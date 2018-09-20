using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class ShippingAddressModel
    {
        public string Id { get; set; }
        public string Receiver { get; set; }
        public string PhoneNumber { get; set; }
        public string Area { get; set; }
        public string AreaCode { get; set; }
        public string Address { get; set; }
        public bool CanSelectElse { get; set; }
    }
}
