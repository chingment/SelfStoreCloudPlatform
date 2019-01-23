using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("Store")]
    public class Store
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string MerchantId { get; set; }
        public Enumeration.StoreStatus Status { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string Address { get; set; }

        public double Lat { get; set; }

        public double Lng { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }

        public string Description { get; set; }
    }
}
