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
    [Table("Order")]
    public class Order
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Sn { get; set; }
        public DateTime? SubmitTime { get; set; }
        public DateTime? PayTime { get; set; }
        public decimal OriginalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal ChargeAmount { get; set; }
        public Enumeration.OrderStatus Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsPromoteProfit { get; set; }
        public string PromoteId { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
        public string WxPrepayId { get; set; }
        public DateTime? WxPrepayIdExpireTime { get; set; }
        
        public string RefereeId { get; set; }

        public string CancelReason { get; set; }
    }
}
