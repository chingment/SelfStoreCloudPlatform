using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Lumos.Entity
{
   [Table("SysOperateHistory")]
    public class SysOperateHistory
    {

        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        [MaxLength(128)]
        public string Ip { get; set; }
        public string ReferenceId { get; set; }
        public Enumeration.OperateType Type { get; set; }
        [MaxLength(512)]
        public string Content { get; set; }
        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
