using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lumos.Entity
{
    [Table("BizProcessesAudit")]
    public class BizProcessesAudit
    {
        [Key]
        public string Id { get; set; }

        public string UserId { get; set; }

        public string MerchantId { get; set; }

        [MaxLength(128)]
        public string AduitTypeEnumName { get; set; }

        public Enumeration.BizProcessesAuditType AduitType { get; set; }

        public string AduitReferenceId { get; set; }

        public string Auditor { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Status { get; set; }

        public string Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public string Mender { get; set; }

        public DateTime? MendTime { get; set; }

        [MaxLength(2058)]
        public string TempAuditComments { get; set; }

        [NotMapped]
        public List<BizProcessesAuditDetails> HistoricalDetails { get; set; }

        public BizProcessesAudit()
        {
            this.HistoricalDetails = new List<BizProcessesAuditDetails>();

        }
    }
}
