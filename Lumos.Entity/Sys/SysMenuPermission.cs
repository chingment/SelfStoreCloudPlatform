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
    [Table("SysMenuPermission")]
    public class SysMenuPermission
    {
        /// <summary>
        ///菜单ID
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int MenuId { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        [Key]
        [Column(Order = 2, TypeName = "varchar")]
        [MaxLength(128)]
        public string PermissionId { get; set; }
    }
}
