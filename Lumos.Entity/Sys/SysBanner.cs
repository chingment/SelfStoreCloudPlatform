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
    [Table("SysBanner")]
    public class SysBanner
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Enumeration.BannerType Type { get; set; }

        [MaxLength(256)]
        public string Title { get; set; }

        [MaxLength(256)]
        public string Source { get; set; }

        public string Content { get; set; }

        [MaxLength(1024)]
        public string ImgUrl { get; set; }

        [MaxLength(1024)]
        public string LinkUrl { get; set; }

        public int ReadCount { get; set; }

        public int Priority { get; set; }

        public Enumeration.SysBannerStatus Status { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

    }
}
