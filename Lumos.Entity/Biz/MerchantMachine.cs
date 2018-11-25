using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("MerchantMachine")]
    public class MerchantMachine
    {
        [Key]
        public string Id { get; set; }
        public string MerchantId { get; set; }
        public string MachineId { get; set; }
        public bool isBind { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
