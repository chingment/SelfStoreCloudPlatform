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
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string ContactName { get; set; }
        [MaxLength(128)]
        public string ContactPhone { get; set; }
        [MaxLength(128)]
        public string ContactAddress { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string ApiHost { get; set; }
        public string ApiKey { get; set; }
        public string ApiSecret { get; set; }
        public int PayTimeout { get; set; }
    }
}
