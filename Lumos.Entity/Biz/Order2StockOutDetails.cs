using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lumos.Entity
{
    [Table("Order2StockOutDetails")]
    public class Order2StockOutDetails
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string Order2StockOutId { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string ProductSkuId { get; set; }
        public string ProductSkuBarCode { get; set; }
        public string ProductSkuName { get; set; }
        public DateTime StockOutTime { get; set; }
        public int Quantity { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? MendTime { get; set; }
        public string Mender { get; set; }
    }
}
