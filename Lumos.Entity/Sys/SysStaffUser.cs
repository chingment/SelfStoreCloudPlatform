
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("SysStaffUser")]
    public class SysStaffUser : SysUser
    {

        [MaxLength(50)]
        public string CtiNo { get; set; }


    }
}
