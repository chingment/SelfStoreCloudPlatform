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

    [Table("ProductKind")]
    public class ProductKind
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(128)]
        public string PId { get; set; }
        public string Name { get; set; }
        [MaxLength(128)]
        public string UserId { get; set; }
        public string IconImg { get; set; }
        public string MainImg { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public bool IsDelete { get; set; }
        public Lumos.Entity.Enumeration.ProductKindStatus Status { get; set; }
        public int Priority { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
        public int Depth { get; set; }

    }
}
