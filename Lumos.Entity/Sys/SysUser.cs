using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{

    [Table("SysUser")]
    public class SysUser
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(128)]
        public string UserName { get; set; }
        [MaxLength(128)]
        public string FullName { get; set; }
        [MaxLength(68)]
        [Required]
        public string PasswordHash { get; set; }
        [MaxLength(36)]
        [Required]
        public string SecurityStamp { get; set; }
        [MaxLength(20)]
        public string PhoneNumber { get; set; }
        [MaxLength(128)]
        public string Email { get; set; }
        [MaxLength(256)]
        public string HeadImgUrl { get; set; }
        public string Nickname { get; set; }
        [MaxLength(128)]
        public string Sex { get; set; }
        [MaxLength(128)]
        public string Province { get; set; }
        [MaxLength(128)]
        public string City { get; set; }
        [MaxLength(128)]
        public string Country { get; set; }
        public DateTime RegisterTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        [MaxLength(50)]
        public string LastLoginIp { get; set; }
        public bool IsDelete { get; set; }
        public Enumeration.UserStatus Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
        public Enumeration.UserType Type { get; set; }

        public bool IsCanDelete { get; set; }

    }
}
