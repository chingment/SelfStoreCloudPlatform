using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class RetOrderReserve
    {
        public RetOrderReserve()
        {
            
        }

        public string OrderSn { get; set; }
        public string PayQrCodeUrl { get; set; }

    }



    public class OrderReserveDetail
    {
        public OrderReserveDetail()
        {
            this.Details = new List<DetailChild>();
        }
        public string MachineId { get; set; }
        public int Quantity { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public List<DetailChild> Details { get; set; }


        public class DetailChild
        {
            public DetailChild()
            {
                this.Details = new List<DetailChildSon>();
                this.SlotStock = new List<SlotStock>();
            }
            public string MachineId { get; set; }
            public string SkuId { get; set; }
            public string SkuName { get; set; }
            public string SkuImgUrl { get; set; }
            public decimal SalesPrice { get; set; }
            public int Quantity { get; set; }
            public decimal OriginalAmount { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal ChargeAmount { get; set; }
            public List<DetailChildSon> Details { get; set; }

            public List<SlotStock> SlotStock { get; set; }
        }

        public class DetailChildSon
        {
            public string Id { get; set; }
            public string MachineId { get; set; }
            public string SlotId { get; set; }
            public string SkuId { get; set; }
            public int Quantity { get; set; }
            public string SkuName { get; set; }
            public string SkuImgUrl { get; set; }
            public decimal SalesPrice { get; set; }
            public decimal OriginalAmount { get; set; }
            public decimal DiscountAmount { get; set; }
            public decimal ChargeAmount { get; set; }
        }

        public class SlotStock
        {
            public string MachineId { get; set; }
            public string SlotId { get; set; }
            public string SkuId { get; set; }
            public int Quantity { get; set; }
        }
    }

}
