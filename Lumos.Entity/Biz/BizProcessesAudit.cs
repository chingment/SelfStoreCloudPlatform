using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("BizProcessesAudit")]
    public class BizProcessesAudit
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int MerchantId { get; set; }

        [MaxLength(128)]
        public string AduitTypeEnumName { get; set; }

        public Enumeration.BizProcessesAuditType AduitType { get; set; }

        public int AduitReferenceId { get; set; }

        public int? Auditor { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int Status { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public int? Mender { get; set; }

        public DateTime? LastUpdateTime { get; set; }

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
