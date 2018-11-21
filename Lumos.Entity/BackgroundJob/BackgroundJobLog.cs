using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lumos.Entity
{
    [Table("BackgroundJobLog")]
    public class BackgroundJobLog
    {
        /// <summary>
        /// JobID
        /// </summary>				
        public string Id { get; set; }

        /// <summary>
        /// JobID
        /// </summary>
        public string BackgroundJobId { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>
        public string JobName { get; set; }

        /// <summary>
        /// 执行时间
        /// </summary>				
        public Nullable<DateTime> ExecutionTime { get; set; }

        /// <summary>
        /// 执行持续时长
        /// </summary>				
        public decimal ExecutionDuration { get; set; }

        /// <summary>
        /// 创建日期时间
        /// </summary>				
        public System.DateTime CreateTime { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>				
        public string RunLog { get; set; }

    }
}
