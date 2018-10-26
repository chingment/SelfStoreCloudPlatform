using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysClientUser")]
    public class SysClientUser : SysUser
    {
        public bool IsVip { get; set; }
    }
}
