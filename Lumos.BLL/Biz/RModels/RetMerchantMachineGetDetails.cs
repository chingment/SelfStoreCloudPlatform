using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class RetMerchantMachineGetDetails
    {
        public RetMerchantMachineGetDetails()
        {
            this.Skus = new List<SkuModel>();
        }

        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public string DeviceId { get; set; }
        public List<SkuModel> Skus { get; set; }
        public class SkuModel
        {
            public string SlotId { get; set; }
            public string SkuId { get; set; }

            public string SkuName { get; set; }

            public string SkuImgUrl { get; set; }

            public int Quantity { get; set; }

            public int LockQuantity { get; set; }

            public int SellQuantity { get; set; }

            public decimal SalePrice { get; set; }

            public decimal SalePriceByVip { get; set; }
        }
    }
}
