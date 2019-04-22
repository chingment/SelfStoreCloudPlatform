using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class RopOrderReserve
    {
        public string StoreId { get; set; }
        public List<Sku> Skus { get; set; }
        public class Sku
        {
            public string Id { get; set; }
            public string CartId { get; set; }
            public int Quantity { get; set; }
            public Enumeration.ReceptionMode ReceptionMode { get; set; }
        }
    }
}
