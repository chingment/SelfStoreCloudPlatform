using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysPositionRole")]
    public class SysPositionRole
    {
        public string Id { get; set; }
        [Key, Column(Order = 1)]
        public Enumeration.SysPositionId PositionId { get; set; }
        [Key, Column(Order = 2)]
        public string RoleId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsCanDelete { get; set; }
    }
}
