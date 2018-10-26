using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("OrderDetails")]
    public class OrderDetails
    {
        [Key]
        public string Id { get; set; }
        public string Sn { get; set; }
        public string ClientId { get; set; }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string ChannelId { get; set; }
        public Entity.Enumeration.ChannelType ChannelType { get; set; }
        public string OrderId { get; set; }
        public string OrderSn { get; set; }
        public Enumeration.ReceptionMode ReceptionMode { get; set; }
        public string Receiver { get; set; }
        public string ReceiverPhone { get; set; }
        public string ReceptionAddress { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public DateTime? CancledTime { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public int Quantity { get; set; }
        [MaxLength(1024)]
        public string Remark { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }

        public Enumeration.OrderStatus Status { get; set; }
    }
}
