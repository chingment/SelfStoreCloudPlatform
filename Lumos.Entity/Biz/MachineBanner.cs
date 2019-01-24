using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("MachineBanner")]
    public class MachineBanner
    {
        [Key]
        public string Id { get; set; }

        public string MerchantId { get; set; }

        public string StoreId { get; set; }
        public string MachineId { get; set; }
        [MaxLength(1024)]
        public string ImgUrl { get; set; }
        public int Priority { get; set; }
        public Enumeration.MachineBannerStatus Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
