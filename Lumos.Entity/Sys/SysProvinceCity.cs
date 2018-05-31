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
    [Table("SysProvinceCity")]
    public class SysProvinceCity
    {
        /// <summary>
        /// 城市ID
        /// </summary>
        [Key]
        [MaxLength(128)]
        [Required]
        public string Id { get; set; }

        /// <summary>
        /// 城市父ID
        /// </summary>
        [MaxLength(128)]
        [Required]
        public string PId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [MaxLength(128)]
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 全称
        /// </summary>
        [MaxLength(128)]
        public string FullName { get; set; }

        /// <summary>
        /// 电话区号
        /// </summary>
        [MaxLength(128)]
        public string PhoneAreaNo { get; set; }

        /// <summary>
        /// 邮政
        /// </summary>
        [MaxLength(128)]
        public string Zip { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
