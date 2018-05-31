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
    [Table("SysVerifyEmail")]
    public class SysVerifyEmail
    {
        public SysVerifyEmail()
        {
            this.Id = Guid.NewGuid();
            this.IsVerify = false;
            this.CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [MaxLength(128)]
        [Required]
        [Column(TypeName = "varchar")]
        public string Email { get; set; }

        /// <summary>
        /// 是否验证
        /// </summary>
        public bool IsVerify { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
