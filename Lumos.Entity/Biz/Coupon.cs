using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Coupon")]
    public class Coupon
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(128)]
        public string Sn { get; set; }
        public string UserId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public Enumeration.CouponStatus Status { get; set; }
        public Enumeration.CouponSourceType SourceType { get; set; }
        public string SourceUserId { get; set; }
        public DateTime? SourceTime { get; set; }
        [MaxLength(512)]
        public string SourceDescription { get; set; }
        public Enumeration.CouponType Type { get; set; }
        public decimal LimitAmount { get; set; }
        public decimal Discount { get; set; }
        [MaxLength(1024)]
        public string LimitTarget { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
