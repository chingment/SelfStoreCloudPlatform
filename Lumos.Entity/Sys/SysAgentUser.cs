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
    [Table("SysAgentUser")]
    public class SysAgentUser : SysUser
    {
        [MaxLength(50)]
        public string AgentCode { get; set; }

    }
}
