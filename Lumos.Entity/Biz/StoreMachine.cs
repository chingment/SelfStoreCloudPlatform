using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("StoreMachine")]
    public class StoreMachine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Userid { get; set; }
        public int MerchantId { get; set; }
        public int MachineId { get; set; }
        public int StoreId { get; set; }
        public Enumeration.StoreMachineStatus Status { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
