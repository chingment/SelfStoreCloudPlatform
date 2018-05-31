using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("BizProcessesAuditDetails")]
    public class BizProcessesAuditDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AuditStep { get; set; }

        [MaxLength(128)]
        public string AuditStepEnumName { get; set; }

        public int AuditStatus{ get; set; }

        [MaxLength(128)]
        public string AuditStatusEnumName { get; set; }

        public int BizProcessesAuditId { get; set; }

        public int? Auditor { get; set; }

        [MaxLength(1024)]
        public string AuditComments { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public DateTime? AuditTime { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public Enumeration.BizProcessesAuditType AduitType { get; set; }

        public int AduitReferenceId { get; set; }

    }


}
