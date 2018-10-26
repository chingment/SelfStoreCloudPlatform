using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Lumos.Entity
{
    [Table("SysMenuPermission")]
    public class SysMenuPermission
    {
        public string Id { get; set; }
        [Key]
        [Column(Order = 1)]
        public string MenuId { get; set; }
        [Key]
        [Column(Order = 2, TypeName = "varchar")]
        [MaxLength(128)]
        public string PermissionId { get; set; }

        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
