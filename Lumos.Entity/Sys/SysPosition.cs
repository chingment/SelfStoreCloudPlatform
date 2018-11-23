﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysPosition")]
    public class SysPosition
    {
        [Key]
        public string Id { get; set; }

        [MaxLength(128)]
        public string Name { get; set; }

        public string OrganizationId { get; set; }

        public string PId { get; set; }
        [MaxLength(512)]
        public string Description { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }

        public bool IsDelete { get; set; }

        public bool IsCanDelete { get; set; }
    }
}