using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("ClientDeliveryAddress")]
    public class ClientDeliveryAddress
    {
        [Key]
        public string Id { get; set; }

        public string ClientId { get; set; }

        [MaxLength(128)]
        public string Consignee { get; set; }
        [MaxLength(128)]
        public string PhoneNumber { get; set; }
        [MaxLength(128)]
        public string AreaName { get; set; }
        [MaxLength(128)]
        public string AreaCode { get; set; }
        [MaxLength(128)]
        public string Address { get; set; }
      
        public bool IsDefault { get; set; }

        public bool IsDelete { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }
        
    }
}
