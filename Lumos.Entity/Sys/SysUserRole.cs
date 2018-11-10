using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace Lumos.Entity
{

    [Table("SysUserRole")]
    public class SysUserRole
    {
        public string Id { get; set; }
        [Key, Column(Order = 1)]
        public string RoleId { get; set; }
        [Key, Column(Order = 2)]
        public string UserId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsCanDelete { get; set; }
    }
}
