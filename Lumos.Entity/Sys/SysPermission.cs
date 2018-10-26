using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lumos.Entity
{
    [Table("SysPermission")]
    public class SysPermission
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string PId { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
