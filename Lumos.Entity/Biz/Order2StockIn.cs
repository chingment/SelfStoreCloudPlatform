using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Order2StockIn")]
    public class Order2StockIn
    {
        [Key]
        public string Id { get; set; }
        public string Sn { get; set; }
        public string MerchantId { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }
        public DateTime StockInTime { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? MendTime { get; set; }
        public string Mender { get; set; }
    }
}
