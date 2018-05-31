using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("SysSalesmanUser")]
    public class SysSalesmanUser : SysUser
    {
        [MaxLength(50)]
        public string CtiNo { get; set; }

        public int AgentId { get; set; }
    }
}
