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
    [Table("SysMenu")]
    public class SysMenu
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        public int PId { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }

        public int Priority { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        [NotMapped]
        public string[] Permission { get; set; }

    }
}
