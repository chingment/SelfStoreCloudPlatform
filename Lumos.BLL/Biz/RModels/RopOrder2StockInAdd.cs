using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RopOrder2StockInAdd
    {
        public RopOrder2StockInAdd()
        {
            this.Skus = new List<Sku>();
        }

        public DateTime StockInTime { get; set; }

        public string WarehouseId { get; set; }

        public string SupplierId { get; set; }

        public string Description { get; set; }

        public List<Sku> Skus { get; set; }

        public class Sku
        {
            public string SkuId { get; set; }

            public int Quantity { get; set; }

            public decimal Amount { get; set; }
        }
    }
}
