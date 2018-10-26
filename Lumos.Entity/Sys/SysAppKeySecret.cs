﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysAppKeySecret")]
    public class SysAppKeySecret
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(128)]
        public string Key { get; set; }

        [MaxLength(128)]
        public string Secret { get; set; }

        public Enumeration.AppKeySecretStatus Status { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }
    }
}
