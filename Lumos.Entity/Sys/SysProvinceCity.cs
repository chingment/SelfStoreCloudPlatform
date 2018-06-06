using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("SysProvinceCity")]
    public class SysProvinceCity
    {
        [Key]
        [MaxLength(128)]
        public string Id { get; set; }
        [MaxLength(128)]
        public string PId { get; set; }
        [MaxLength(128)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string FullName { get; set; }
        [MaxLength(128)]
        public string PhoneAreaNo { get; set; }
        [MaxLength(128)]
        public string Zip { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public int Priority { get; set; }
        public bool IsDelete { get; set; }
    }
}
