using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("SysUserLoginHistory")]
    public class SysUserLoginHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 登录类型
        /// </summary>
        public Enumeration.LoginType LoginType { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        [MaxLength(128)]
        [Column(TypeName = "varchar")]
        public string Ip { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [MaxLength(128)]
        public string Country { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [MaxLength(128)]
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        [MaxLength(128)]
        public string City { get; set; }

        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; }

        /// <summary>
        /// 登录结果
        /// </summary>
        public Enumeration.LoginResult Result { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(512)]
        public string Description { get; set; }
    }
}
