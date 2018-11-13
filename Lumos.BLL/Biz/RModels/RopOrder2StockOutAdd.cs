using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RopOrder2StockOutAdd
    {
        public RopOrder2StockOutAdd()
        {
            this.Skus = new List<Sku>();
        }

        public DateTime StockOutTime { get; set; }

        public string WarehouseId { get; set; }

        public string StoreId { get; set; }

        public string Description { get; set; }

        public List<Sku> Skus { get; set; }

        public class Sku
        {
            public string SkuId { get; set; }

            public int Quantity { get; set; }

        }
    }
}
