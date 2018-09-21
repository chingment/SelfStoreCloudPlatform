using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("StoreBanner")]
    public class StoreBanner
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string StoreId { get; set; }
        public string Title { get; set; }
        [MaxLength(1024)]
        public string ImgUrl { get; set; }
        public int Priority { get; set; }
        public Enumeration.StoreBannerStatus Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
