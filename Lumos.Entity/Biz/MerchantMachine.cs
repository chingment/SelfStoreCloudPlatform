using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    [Table("MerchantMachine")]
    public class MerchantMachine
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MerchantId { get; set; }
        public int MachineId { get; set; }
        public string Name { get; set; }
        public Enumeration.MerchantMachineStatus Status { get; set; }
        public int Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int? Mender { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        //[MaxLength(256)]
        //public string LogoImgUrl { get; set; }
        //[MaxLength(256)]
        //public string BtnBuyImgUrl { get; set; }
        //[MaxLength(256)]
        //public string BtnPickImgUrl { get; set; }
    }
}
