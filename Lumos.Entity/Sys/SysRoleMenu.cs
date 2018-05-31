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
    [Table("SysRoleMenu")]
    public class SysRoleMenu
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Key]
        [Column(Order = 2)]
        public int MenuId { get; set; }
    }
}
