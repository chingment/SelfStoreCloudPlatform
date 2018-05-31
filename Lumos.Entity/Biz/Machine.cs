using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Machine")]
    public class Machine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(128)]
        public string Sn { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string DeviceId { get; set; }
        [MaxLength(128)]
        public string MacAddress { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
