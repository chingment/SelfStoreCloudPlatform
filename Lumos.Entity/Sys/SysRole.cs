using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysRole")]
    public class SysRole
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public string PId { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }

        public int Dept { get; set; }

        public Enumeration.BelongSite BelongSite { get; set; }

        public int Priority { get; set; }
    }
}
