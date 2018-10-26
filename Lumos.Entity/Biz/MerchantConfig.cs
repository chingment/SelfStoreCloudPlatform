﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("MerchantConfig")]
    public class MerchantConfig
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
        public string ApiHost { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public int PayTimeout { get; set; }
    }
}
