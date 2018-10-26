using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysMerchantUser")]
    public class SysMerchantUser : SysUser
    {
        [MaxLength(128)]
        public string MerchantName { get; set; }
        [MaxLength(128)]
        public string ContactName { get; set; }
        [MaxLength(128)]
        public string ContactPhone { get; set; }
        [MaxLength(128)]
        public string ContactAddress { get; set; }
    }
}
