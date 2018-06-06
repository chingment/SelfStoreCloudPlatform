using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("SysPageAccessRecord")]
    public class SysPageAccessRecord
    {

        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        [MaxLength(256)]
        public string PageUrl { get; set; }
        [MaxLength(128)]
        public string Ip { get; set; }
        public DateTime AccessTime { get; set; }
    }
}
