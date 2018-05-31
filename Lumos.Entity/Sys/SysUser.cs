using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{

    [Table("SysUser")]
    public class SysUser
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SysUser() { }
 
        public SysUser(string name) : this() { UserName = name; }

        /// <summary>
        /// 用户帐号
        /// </summary>
        [MaxLength(128)]
        public string UserName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [MaxLength(128)]
        public string FullName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [MaxLength(68)]
        [Required]
        public  string PasswordHash { get; set; }

        /// <summary>
        ///  安全钥匙
        /// </summary>
        [MaxLength(36)]
        [Required]
        public  string SecurityStamp { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [MaxLength(128)]

        public string Email { get; set; }

        /// <summary>
        /// 用户头像图片
        /// </summary>
        [MaxLength(256)]
        public string HeadImg { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }

        /// <summary>
        /// 最后登录IP
        /// </summary>
        [MaxLength(50)]
        public string LastLoginIp { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        
        public Enumeration.UserStatus Status { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int Creator { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public int? Mender { get; set; }

        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastUpdateTime { get; set; }

        public Enumeration.UserType Type { get; set; }

        [NotMapped]
        public string Password { get; set; }

        [MaxLength(128)]
        public string WechatNumber { get; set; }

}
}
