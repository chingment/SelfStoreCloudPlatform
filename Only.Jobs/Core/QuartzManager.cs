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

namespace Only.Jobs.Core
{
    public class QuartzManager
    {


        /// <summary>
        /// 从程序集中加载指定类
        /// </summary>
        /// <param name="assemblyName">含后缀的程序集名</param>
        /// <param name="className">含命名空间完整类名</param>
        /// <returns></returns>
        public Type GetClassInfo(string assemblyName, string className)
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
        public bool ValidExpression(string cronExpression)
        {
            return CronExpression.IsValidExpression(cronExpression);
        }

        /// <summary>
        ///  获取文件的绝对路径
        /// </summary>
        /// <param name="relativePath">相对路径</param>
        /// <returns></returns>
        public string GetAbsolutePath(string relativePath)
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
                return Path.Combine(HttpRuntime.AppDomainAppPath, relativePath);
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
        public void ScheduleJob(IScheduler scheduler, BackgroundJob jobInfo)
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


        /// <summary>
        /// Job状态管控
        /// </summary>
        /// <param name="Scheduler"></param>
        public void JobScheduler(IScheduler Scheduler)
        {
            ILog _logger = LogManager.GetLogger(typeof(QuartzManager));
            _logger.InfoFormat("进入Job状态管控");

            List<BackgroundJob> list = AdminServiceFactory.BackgroundJob.GeAllowScheduleJobInfoList();
            if (list != null && list.Count > 0)
            {
                _logger.InfoFormat("进入Job状态管控,有效监控数为:" + list.Count);

                foreach (BackgroundJob jobInfo in list)
                {
                    _logger.InfoFormat("进入Job[{0}]的状态为:{1}", jobInfo.Id, jobInfo.Status.GetCnName());

                    JobKey jobKey = new JobKey(jobInfo.Id, jobInfo.Id + "Group");
                    if (Scheduler.CheckExists(jobKey) == false)
                    {
                        if (jobInfo.Status == Enumeration.BackgroundJobStatus.Runing || jobInfo.Status == Enumeration.BackgroundJobStatus.Starting)
                        {
                            ScheduleJob(Scheduler, jobInfo);
                            if (Scheduler.CheckExists(jobKey) == false)
                            {
                                AdminServiceFactory.BackgroundJob.SetStatus(GuidUtil.New(), jobInfo.Id, Enumeration.BackgroundJobStatus.Stoped);
                            }
                            else
                            {
                                AdminServiceFactory.BackgroundJob.SetStatus(GuidUtil.New(), jobInfo.Id, Enumeration.BackgroundJobStatus.Runing);
                            }
                        }
                        else if (jobInfo.Status == Enumeration.BackgroundJobStatus.Stoping)
                        {
                            AdminServiceFactory.BackgroundJob.SetStatus(GuidUtil.New(), jobInfo.Id, Enumeration.BackgroundJobStatus.Stoped);
                        }
                    }
                    else
                    {
                        if (jobInfo.Status == Enumeration.BackgroundJobStatus.Stoping)
                        {
                            Scheduler.DeleteJob(jobKey);
                            AdminServiceFactory.BackgroundJob.SetStatus(GuidUtil.New(), jobInfo.Id, Enumeration.BackgroundJobStatus.Stoped);
                        }
                        else if (jobInfo.Status == Enumeration.BackgroundJobStatus.Starting)
                        {
                            AdminServiceFactory.BackgroundJob.SetStatus(GuidUtil.New(), jobInfo.Id, Enumeration.BackgroundJobStatus.Runing);
                        }
                    }
                }
            }
        }

    }
}
