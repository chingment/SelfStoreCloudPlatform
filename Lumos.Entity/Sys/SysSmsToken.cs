using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lumos.Entity
{
    [Table("SysSmsSendHistory")]
    public class SysSmsSendHistory
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [MaxLength(128)]
        public string ApiName { get; set; }

        [MaxLength(512)]
        public string TemplateParams { get; set; }

        [MaxLength(128)]
        public string TemplateCode { get; set; }

        [MaxLength(2048)]
        public string FailureReason { get; set; }

        [MaxLength(128)]
        public string Phone { get; set; }

        [MaxLength(128)]
        public string Token { get; set; }

        [MaxLength(128)]
        public string ValidCode { get; set; }

        public bool IsUse { get; set; }

        public DateTime? ExpireTime { get; set; }

        public int Creator { get; set; }

        public DateTime CreateTime { get; set; }

        public Enumeration.SysSmsSendResult Result { get; set; }
    }
}
