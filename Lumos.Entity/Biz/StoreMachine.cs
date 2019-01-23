﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lumos.Entity
{
    [Table("StoreMachine")]
    public class StoreMachine
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string MerchantId { get; set; }
        public string MachineId { get; set; }
        public string StoreId { get; set; }
        public bool IsBind { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
