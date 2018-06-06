using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    public class MachineProductStock
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string MerchantId { get; set; }
        public string MachineId { get; set; }
        public string StoreId { get; set; }
        public string ProductId { get; set; }
        public string ProductSkuId { get; set; }
        public int Quantity { get; set; }
        public int LockQuantity { get; set; }
        public int SellQuantity { get; set; }
        public bool IsOffSell { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
