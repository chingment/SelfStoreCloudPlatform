using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("ShippingAddress")]
    public class ShippingAddress
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        [MaxLength(128)]
        public string Receiver { get; set; }
        [MaxLength(128)]
        public string PhoneNumber { get; set; }
        [MaxLength(128)]
        public string Area { get; set; }
        [MaxLength(128)]
        public string AreaCode { get; set; }
        [MaxLength(128)]
        public string Address { get; set; }
      
        public bool IsDefault { get; set; }

        public bool IsDelete { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }
        
    }
}
