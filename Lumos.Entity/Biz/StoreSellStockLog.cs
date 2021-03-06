﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("StoreSellStockLog")]
    public class StoreSellStockLog
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string SlotId { get; set; }
        public Enumeration.ChannelType ChannelType { get; set; }
        public string ChannelId { get; set; }
        public string ProductSkuId { get; set; }
        public Enumeration.MachineStockLogChangeTpye ChangeType { get; set; }
        public int ChangeQuantity { get; set; }
        public int Quantity { get; set; }
        public int LockQuantity { get; set; }
        public int SellQuantity { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }

        public string RemarkByDev { get; set; }
    }
}
