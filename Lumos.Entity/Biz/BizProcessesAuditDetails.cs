using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lumos.Entity
{
    [Table("BizProcessesAuditDetails")]
    public class BizProcessesAuditDetails
    {
        [Key]
        public string Id { get; set; }

        public int AuditStep { get; set; }

        [MaxLength(128)]
        public string AuditStepEnumName { get; set; }

        public int AuditStatus{ get; set; }

        [MaxLength(128)]
        public string AuditStatusEnumName { get; set; }

        public string BizProcessesAuditId { get; set; }

        public string Auditor { get; set; }

        [MaxLength(1024)]
        public string AuditComments { get; set; }

        [MaxLength(1024)]
        public string Description { get; set; }

        public DateTime? AuditTime { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public Enumeration.BizProcessesAuditType AduitType { get; set; }

        public string AduitReferenceId { get; set; }

    }


}
