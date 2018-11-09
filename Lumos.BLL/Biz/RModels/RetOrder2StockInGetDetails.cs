using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RetOrder2StockInGetDetails
    {
        public RetOrder2StockInGetDetails()
        {
            this.Skus = new List<Sku>();
        }

        public string Order2StockInId { get; set; }
        public string Sn { get; set; }
        public string StockInTime { get; set; }
        public string WarehouseName { get; set; }
        public string SupplierName { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public List<Sku> Skus { get; set; }
        public class Sku
        {
            public string SkuId { get; set; }

            public string BarCode { get; set; }
            public string Name { get; set; }

            public int Quantity { get; set; }

            public decimal Amount { get; set; }
        }

    }
}
