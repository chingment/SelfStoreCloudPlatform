using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lumos.Entity
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public string Sn { get; set; }
        public string ClientId { get; set; }

        public string ClientName { get; set; }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }

        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public int Quantity { get; set; }
        public string CouponId { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public DateTime? CancledTime { get; set; }

        public Enumeration.OrderSource Source { get; set; }
        public Enumeration.OrderStatus Status { get; set; }
        public string CancelReason { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }

        public Enumeration.OrderPayWay PayWay { get; set; }
        public string PayPrepayId { get; set; }
        public DateTime? PayExpireTime { get; set; }
        public string PayQrCodeUrl { get; set; }
        public string PickCode { get; set; }
        public DateTime? PickCodeExpireTime { get; set; }

    }
}
