using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class RopOrderReserve
    {
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public int PayTimeout { get; set; }
        public List<Detail> Details { get; set; }
        public class Detail
        {
            public string SkuId { get; set; }
            public int Quantity { get; set; }
            public Enumeration.ReceptionMode ReceptionMode { get; set; }
        }
    }
}
