using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term.Models
{
    public class OrderReservePms
    {
        public OrderReservePms()
        {
            this.Details = new List<Detail>();
        }

        public string UserId { get; set; }
        public string MerchantId { get; set; }
        public string MachineId { get; set; }
        public string PayWay { get; set; }
        public List<Detail> Details { get; set; }
        public class Detail
        {
            public string SkuId { get; set; }
            public int Quantity { get; set; }
        }
    }
}
