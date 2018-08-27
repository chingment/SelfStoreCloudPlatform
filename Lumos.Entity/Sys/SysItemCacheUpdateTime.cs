using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("SysItemCacheUpdateTime")]
    public class SysItemCacheUpdateTime
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public Enumeration.SysItemCacheType Type { get; set; }

        public string ReferenceId { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }
    }
}
