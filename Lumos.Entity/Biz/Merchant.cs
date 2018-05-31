using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("Merchant")]
    public class Merchant
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        [MaxLength(128)]
        public string Sn { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string ContactName { get; set; }
        [MaxLength(128)]
        public string ContactPhone { get; set; }
        [MaxLength(128)]
        public string ContactAddress { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
