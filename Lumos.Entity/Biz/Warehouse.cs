using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("Warehouse")]
    public class Warehouse
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        public string Address { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
