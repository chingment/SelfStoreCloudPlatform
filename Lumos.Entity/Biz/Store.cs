﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Store")]
    public class Store
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        [MaxLength(128)]
        public string AreaCode { get; set; }
        [MaxLength(128)]
        public string AreaName { get; set; }
        [MaxLength(128)]
        public string Address { get; set; }
        public string Description { get; set; }
        public Enumeration.StoreStatus Status { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
