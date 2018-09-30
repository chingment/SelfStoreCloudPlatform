using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("OrderDetailsChild")]
    public class OrderDetailsChild
    {
        [Key]
        public string Id { get; set; }
        public string Sn { get; set; }
        public string ClientId { get; set; }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string MachineId { get; set; }
        public string OrderId { get; set; }
        public string OrderSn { get; set; }
        public string OrderDetailsId { get; set; }
        public string OrderDetailsSn { get; set; }
        public string ProductSkuId { get; set; }
        public string ProductSkuName { get; set; }
        public string ProductSkuImgUrl { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? CancledTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
