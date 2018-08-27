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
        [Key]
        public string Id { get; set; }

        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        public string PId { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        [MaxLength(512)]
        public string Description { get; set; }

        public int Priority { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }

        [NotMapped]
        public string[] Permission { get; set; }

    }
}
