using log4net;
using Lumos;
using Lumos.BLL.Service.Admin;
using Lumos.Entity;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;

namespace Lumos.BLL.Task
{
    public class QuartzManager
    {


        /// <summary>
        /// 从程序集中加载指定类
        /// </summary>
        /// <param name="assemblyName">含后缀的程序集名</param>
        /// <param name="className">含命名空间完整类名</param>
        /// <returns></returns>
        public static Type GetClassInfo(string assemblyName, string className)
        {
            Type type = null;
            try
            {
                assemblyName = GetAbsolutePath(assemblyName);
                Assembly assembly = null;
                assembly = Assembly.LoadFrom(assemblyName);
                type = assembly.GetType(className, true, true);
            }
            catch (Exception ex)
            {
            }
            return type;
        }

        /// <summary>
        /// 校验字符串是否为正确的Cron表达式
        /// </summary>
        /// <param name="cronExpression">带校验表达式</param>
        /// <returns></returns>
        public static bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        ///  获取文件的绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        public static string GetAbsolutePath(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                throw new ArgumentNullException("参数relativePath空异常！");
            }
            relativePath = relativePath.Replace("/", "\\");
            if (relativePath[0] == '\\')
            {
                relativePath = relativePath.Remove(0, 1);
            }
            if (HttpContext.Current != null)
            {
                return Path.Combine(HttpRuntime.AppDomainAppPath + "\\bin\\", relativePath);
            }
            else
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }
        }


        /// <summary>
        /// Job调度
        /// </summary>
        /// <param name="scheduler"></param>
        /// <param name="jobInfo"></param>
        public static void ScheduleJob(IScheduler scheduler, BackgroundJob jobInfo)
        {
            if (ValidExpression(jobInfo.CronExpression))
            {
                Type type = GetClassInfo(jobInfo.AssemblyName, jobInfo.ClassName);
                if (type != null)
                {
                    IJobDetail job = new JobDetailImpl(jobInfo.Id, jobInfo.Id + "Group", type);
                    job.JobDataMap.Add("Parameters", jobInfo.JobArgs);
                    job.JobDataMap.Add("JobName", jobInfo.Name);

                    CronTriggerImpl trigger = new CronTriggerImpl();
                    trigger.CronExpressionString = jobInfo.CronExpression;
                    trigger.Name = jobInfo.Id;
                    trigger.Description = jobInfo.Description;
                    trigger.StartTimeUtc = DateTime.UtcNow;
                    trigger.Group = jobInfo.Id + "TriggerGroup";
                    scheduler.ScheduleJob(job, trigger);
                }
                else
                {
                    AdminServiceFactory.BackgroundJob.WriteLog(GuidUtil.New(), jobInfo.Id, jobInfo.Name, DateTime.Now, 0, jobInfo.AssemblyName + jobInfo.ClassName + "无效，无法启动该任务");
                }
            }
            else
            {
                AdminServiceFactory.BackgroundJob.WriteLog(GuidUtil.New(), jobInfo.Id, jobInfo.Name, DateTime.Now, 0, jobInfo.CronExpression + "不是正确的Cron表达式,无法启动该任务");
            }
        }


        public static void DeleteJob(IScheduler scheduler, JobKey jobKey)
        {
            if (scheduler == null)
                return;

            if (jobKey == null)
                return;

            if (scheduler.CheckExists(jobKey))
            {
                scheduler.DeleteJob(jobKey);
            }
        }

        /// <summary>
        /// Job状态管控
        /// </summary>
        /// <param name="Scheduler"></param>
        public static void JobScheduler(IScheduler scheduler)
        {
            ILog _logger = LogManager.GetLogger(typeof(QuartzManager));
            _logger.InfoFormat("Job状态管控");

            var jobs = AdminServiceFactory.BackgroundJob.GeAllowScheduleJobInfoList();

            _logger.InfoFormat("Job状态管控,有效控数为:" + jobs.Count);

            if (jobs.Count > 0)
            {
                foreach (BackgroundJob job in jobs)
                {
                    string jobId = job.Id;
                    string jobGroup = string.Format("{0}Group", jobId);

                    _logger.InfoFormat("Job状态管控,任务({0})的状态:{1}", jobId, job.Status.GetCnName());

                    JobKey jobKey = new JobKey(jobId, jobGroup);

                    switch (job.Status)
                    {
                        case Enumeration.BackgroundJobStatus.Stoped:
                            DeleteJob(scheduler, jobKey);
                            break;
                        case Enumeration.BackgroundJobStatus.Starting:
                            ScheduleJob(scheduler, job);
                            AdminServiceFactory.BackgroundJob.SetStatus(GuidUtil.New(), job.Id, Enumeration.BackgroundJobStatus.Runing);
                            break;
                        case Enumeration.BackgroundJobStatus.Runing:
                            ScheduleJob(scheduler, job);
                            break;
                        case Enumeration.BackgroundJobStatus.Stoping:
                            DeleteJob(scheduler, jobKey);
                            AdminServiceFactory.BackgroundJob.SetStatus(GuidUtil.New(), job.Id, Enumeration.BackgroundJobStatus.Stoped);
                            break;
                    }
                }
            }
        }
    }
}
