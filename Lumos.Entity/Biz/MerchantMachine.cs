﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("MerchantMachine")]
    public class MerchantMachine
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string MachineId { get; set; }
        public string MachineName { get; set; }
        public bool IsBind { get; set; }
        [MaxLength(1024)]
        public string LogoImgUrl { get; set; }
        [MaxLength(1024)]
        public string BtnBuyImgUrl { get; set; }
        [MaxLength(1024)]
        public string BtnPickImgUrl { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
