using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RopOrder2StockOutAdd
    {
        public RopOrder2StockOutAdd()
        {
            this.Skus = new List<Sku>();
        }

        public DateTime StockOutTime { get; set; }

        public string WarehouseId { get; set; }

        public string TargetId { get; set; }

        public Entity.Enumeration.Order2StockOutTargetType TargetType { get; set; }

        public string TargetName { get; set; }

        public string Description { get; set; }

        public List<Sku> Skus { get; set; }

        public class Sku
        {
            public string SkuId { get; set; }

            public int Quantity { get; set; }

        }


    }
}
