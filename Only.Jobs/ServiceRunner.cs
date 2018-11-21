using Common.Logging;
using Lumos;
using Lumos.BLL.Task;
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
            string mainJobId = GuidUtil.Empty();
            string mainJobGroup = string.Format("{0}Group", mainJobId);

            JobKey jobKey = new JobKey(mainJobId, mainJobGroup);
            IJobDetail job = new JobDetailImpl(mainJobId, mainJobGroup, mainJobType);
            job.JobDataMap.Add("Parameters", "");
            job.JobDataMap.Add("JobName", "监控子任务状态变化任务");

            CronTriggerImpl trigger = new CronTriggerImpl();
            trigger.CronExpressionString = "0/3 * * * * ?";//每3秒执行一次
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
            //QuartzManager.JobScheduler(scheduler);
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
