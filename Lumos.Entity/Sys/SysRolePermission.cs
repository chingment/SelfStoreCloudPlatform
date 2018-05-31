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
    [Table("SysRolePermission")]
    public class SysRolePermission
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }

        /// <summary>
        /// 权限ID
        /// </summary>
        [Key]
        [Column(Order = 2)]
        [MaxLength(128)]
        public string PermissionId { get; set; }
    }
}
