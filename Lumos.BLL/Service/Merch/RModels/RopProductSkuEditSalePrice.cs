using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class RopProductSkuEditSalePrice
    {
        public string StoreId { get; set; }
        public string ProductSkuId { get; set; }
        public decimal SalePrice { get; set; }
    }
}
