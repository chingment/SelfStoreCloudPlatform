using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("SysOpenIdInfo")]
    public class SysOpenIdInfo
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(128)]
        public string OpenId { get; set; }

        public Enumeration.OpenIdType OpenIdType { get; set; }

        [MaxLength(128)]
        public string ClientCode { get; set; }

    }
}
