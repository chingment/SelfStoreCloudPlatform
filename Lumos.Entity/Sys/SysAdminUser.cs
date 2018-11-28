using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysAdminUser")]
    public class SysAdminUser : SysUser
    {
        public string OrganizationId { get; set; }

        public Enumeration.SysPositionId PositionId { get; set; }
    }
}
