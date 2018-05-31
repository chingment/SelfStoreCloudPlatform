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
    [Table("SysPermission")]
    public class SysPermission
    {
        /// <summary>
        /// 权限ID
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        [MaxLength(128)]
        [Required]
        [Column(TypeName = "varchar")]
        public string Id { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 权限父ID
        /// </summary>
        [MaxLength(128)]
        [Required]
        public string PId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
