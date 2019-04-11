using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    public class AdRelease
    {
        [Key]
        public string Id { get; set; }
        public Enumeration.AdSpaceId AdSpaceId { get; set; }
        public string MerchantId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int Priority { get; set; }
        public Enumeration.AdReleaseStatus Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
