﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("ImsOrder2StockInDetails")]
    public class ImsOrder2StockInDetails
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string Order2StockInId { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ProductSkuId { get; set; }
        public string ProductSkuBarCode { get; set; }
        public string ProductSkuName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public DateTime StockInTime { get; set; }
        public int Quantity { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? MendTime { get; set; }
        public string Mender { get; set; }
    }
}
