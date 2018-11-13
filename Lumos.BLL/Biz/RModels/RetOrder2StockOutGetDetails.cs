using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{

    public class RetOrder2StockOutGetDetails
    {
        public RetOrder2StockOutGetDetails()
        {
            this.Skus = new List<Sku>();
        }

        public string Order2StockOutId { get; set; }
        public string Sn { get; set; }
        public string StockOutTime { get; set; }
        public string WarehouseName { get; set; }
        public string StoreName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public List<Sku> Skus { get; set; }
        public class Sku
        {
            public string SkuId { get; set; }
            public string BarCode { get; set; }
            public string Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}
