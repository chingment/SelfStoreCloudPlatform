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

    [Table("SysClientUser")]
    public class SysClientUser : SysUser
    {
        public string ClientCode { get; set; }

        public int MerchantId { get; set; }

        public Enumeration.ClientAccountType ClientAccountType { get; set; }

        public bool IsTestAccount { get; set; }


    }
}
