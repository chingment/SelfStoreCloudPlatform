using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("OrderDetailsChildSon")]
    public class OrderDetailsChildSon
    {
        [Key]
        public string Id { get; set; }
        public string Sn { get; set; }
        public string ClientId { get; set; }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string ChannelId { get; set; }
        public Entity.Enumeration.ChannelType ChannelType { get; set; }
        public Enumeration.ReceptionMode ReceptionMode { get; set; }
        public string OrderId { get; set; }
        public string OrderSn { get; set; }
        public string OrderDetailsId { get; set; }
        public string OrderDetailsSn { get; set; }
        public string OrderDetailsChildId { get; set; }
        public string OrderDetailsChildSn { get; set; }
        public string SlotId { get; set; }
        public string ProductSkuId { get; set; }
        public string ProductSkuName { get; set; }
        public string ProductSkuImgUrl { get; set; }
        public int Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public decimal SalePriceByVip { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public DateTime? PayTime { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? CancledTime { get; set; }
        public DateTime? CompletedTime { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }

        public Enumeration.OrderDetailsChildSonStatus Status { get; set; }

    }
}
