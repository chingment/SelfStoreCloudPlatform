using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string MerchantId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
