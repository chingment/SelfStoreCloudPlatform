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
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public Enumeration.SysItemCacheType Type { get; set; }

        public int? ReferenceId { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
