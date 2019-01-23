using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("MachineBindLog")]
    public class MachineBindLog
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }
        public string MerchantId { get; set; }
        public string MachineId { get; set; }
        public string StoreId { get; set; }
        public Enumeration.MachineBindType BindType { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Description { get; set; }
    }
}
