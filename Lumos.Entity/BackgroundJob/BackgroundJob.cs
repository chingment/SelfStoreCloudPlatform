using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Lumos.Entity
{
    [Table("BackgroundJob")]
    public class BackgroundJob
    {
        /// <summary>
        /// JobID
        /// </summary>				
        public string Id { get; set; }

        /// <summary>
        /// Job名称
        /// </summary>				
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>				
        public string Description { get; set; }

        /// <summary>
        /// 程序集名称(所属程序集)
        /// </summary>				
        public string AssemblyName { get; set; }

        /// <summary>
        /// 类名(完整命名空间的类名)
        /// </summary>				
        public string ClassName { get; set; }

        /// <summary>
        /// 参数
        /// </summary>				
        public string JobArgs { get; set; }

        /// <summary>
        /// Cron表达式
        /// </summary>				
        public string CronExpression { get; set; }

        /// <summary>
        /// Cron表达式描述
        /// </summary>				
        public string CronExpressionDescription { get; set; }

        /// <summary>
        /// 最后运行时间
        /// </summary>				
        public System.Nullable<System.DateTime> LastRunTime { get; set; }

        /// <summary>
        /// 下次运行时间
        /// </summary>				
        public System.Nullable<System.DateTime> NextRunTime { get; set; }

        /// <summary>
        /// 运行次数
        /// </summary>
        public int RunCount { get; set; }

        /// <summary>
        /// 状态  0-停止  1-运行   3-正在启动中...   5-停止中...
        /// </summary>
        public Enumeration.BackgroundJobStatus Status { get; set; }

        /// <summary>
        /// 排序
        /// </summary>				
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 是否删除 0-未删除   1-已删除
        /// </summary>				
        public bool IsDelete { get; set; }

        public string Creator { get; set; }
        public DateTime CreateTime { get; set; }
        public string Mender { get; set; }
        public DateTime? MendTime { get; set; }
    }
}
