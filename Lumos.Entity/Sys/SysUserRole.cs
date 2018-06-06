using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{

    [Table("SysUserRole")]
    public class SysUserRole
    {
        public string Id { get; set; }
        [Key,Column(Order = 1)]
        public string RoleId { get; set; }
        [Key, Column(Order = 2)]
        public string UserId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
