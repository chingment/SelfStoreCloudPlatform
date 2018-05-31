using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Lumos.Entity
{
   [Table("SysOperateHistory")]
    public class SysOperateHistory
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }


        /// <summary>
        /// IP地址
        /// </summary>
        [MaxLength(128)]
        [Column(TypeName = "varchar")]
        public string Ip { get; set; }

        /// <summary>
        /// 关联ID
        /// </summary>
        [Description("关联ID")]
        public int? ReferenceId { get; set; }

        [Description("操作类型")]
        public Enumeration.OperateType Type { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [MaxLength(512)]
        [Description("内容")]
        public string Content { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
