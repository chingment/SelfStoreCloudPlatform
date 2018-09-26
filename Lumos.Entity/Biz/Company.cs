using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Company")]
    public class Company
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public Enumeration.CompanyClass Class { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(256)]
        public string Address { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
