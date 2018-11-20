﻿using Common.Logging;
using Lumos;
using Only.Jobs.Core;
using Only.Jobs.JobItems;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System;
using Topshelf;

namespace Only.Jobs
{
    public sealed class ServiceRunner : ServiceControl, ServiceSuspend
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ServiceRunner));
        private readonly IScheduler scheduler;

        private string ServiceName
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings.Get("ServiceName");
            }
        }

        public ServiceRunner()
        {
            scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public bool Start(HostControl hostControl)
        {

            Type mainJobType = typeof(ManagerJob);
            string mainJobId = "main";
            JobKey jobKey = new JobKey(mainJobId, mainJobId + "Group");
            IJobDetail job = new JobDetailImpl(mainJobId, mainJobId + "Group", mainJobType);
            job.JobDataMap.Add("Parameters", "");
            job.JobDataMap.Add("JobName", "监控子任务状态变化任务");

            CronTriggerImpl trigger = new CronTriggerImpl();
            trigger.CronExpressionString = "0/3 * * * * ?";
            trigger.Name = mainJobId;
            trigger.Description = "监控子任务状态变化任务";
            trigger.StartTimeUtc = DateTime.UtcNow;
            trigger.Group = mainJobId + "TriggerGroup";

            if (!scheduler.CheckExists(jobKey))
            {
                scheduler.ScheduleJob(job, trigger);
            }


            scheduler.ListenerManager.AddJobListener(new SchedulerJobListener(), GroupMatcher<JobKey>.AnyGroup());
            scheduler.Start();
            new QuartzManager().JobScheduler(scheduler);
            _logger.Info(string.Format("{0} Start", ServiceName));
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            scheduler.Shutdown(false);
            _logger.Info(string.Format("{0} Stop", ServiceName));
            return true;
        }

        public bool Continue(HostControl hostControl)
        {
            scheduler.ResumeAll();
            _logger.Info(string.Format("{0} Continue", ServiceName));
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            scheduler.PauseAll();
            _logger.Info(string.Format("{0} Pause", ServiceName));
            return true;
        }

    }
}
