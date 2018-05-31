using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("SysPageAccessRecord")]
    public class SysPageAccessRecord
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
        /// 访问时间
        /// </summary>
        [MaxLength(256)]
        [Column(TypeName = "varchar")]
        public string PageUrl { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        [MaxLength(128)]
        public string Ip { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime AccessTime { get; set; }
    }
}
