using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term.Models
{
    public class OrderReserveResult
    {
        public OrderReserveResult()
        {
            this.Details = new List<Detail>();
        }

        public string OrderSn { get; set; }
        public string PayUrl { get; set; }

        public List<Detail> Details { get; set; }


        public class Detail
        {
            public string SlotId { get; set; }
            public string SkuId { get; set; }
            public int Quantity { get; set; }

        }
    }
}
