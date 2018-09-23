﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("UserDeliveryAddress")]
    public class UserDeliveryAddress
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

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

        public DateTime? LastUpdateTime { get; set; }
        
    }
}