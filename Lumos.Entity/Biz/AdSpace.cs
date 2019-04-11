using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("AdSpace")]
    public class AdSpace
    {
        [Key]
        public Enumeration.AdSpaceId Id { get; set; }
        public string Name { get; set; }
        public Enumeration.AdSpaceBelong Belong { get; set; }
        public string Description { get; set; }
        public Enumeration.AdSpaceType Type { get; set; }
        public Enumeration.AdSpaceStatus Status { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
