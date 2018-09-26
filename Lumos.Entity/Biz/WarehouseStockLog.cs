using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("WarehouseStockLog")]
    public class WarehouseStockLog
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string ProductSkuId { get; set; }
        public string ProductSkuName { get; set; }
        public Enumeration.WarehouseStockLogChangeTpye ChangeType { get; set; }
        public int ChangeQuantity { get; set; }
        public int Quantity { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
