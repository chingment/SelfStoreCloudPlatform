﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("Machine")]
    public class Machine
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string MacAddress { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
        public float Lat { get; set; }
        public float Lng { get; set; }
        public string JPushRegId { get; set; }
        public string MerchantId { get; set; }
        public string StoreId { get; set; }
        public string LogoImgUrl { get; set; }
        public string BtnBuyImgUrl { get; set; }
        public string BtnPickImgUrl { get; set; }
    }
}
