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
    public class MachineBanner
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Enumeration.MachineBannerType Type { get; set; }
        [MaxLength(256)]
        public string Title { get; set; }
        [MaxLength(1024)]
        public string ImgUrl { get; set; }
        public int Priority { get; set; }
        public Enumeration.MachineBannerStatus Status { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
