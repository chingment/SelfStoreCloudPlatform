using Lumos;
using Lumos.BLL.Service.Admin;
using Quartz;
using System;

namespace Lumos.BLL.Task
{
    public class SchedulerJobListener : IJobListener
    {
        public void JobExecutionVetoed(IJobExecutionContext context)
        {

        }

        public void JobToBeExecuted(IJobExecutionContext context)
        {

        }

        public void JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException)
        {
            string BackgroundJobId = GuidUtil.New();


            BackgroundJobId = context.JobDetail.Key.Name;

            //string.TryParse(context.JobDetail.Key.Name, out BackgroundJobId);
            DateTime NextFireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.NextFireTimeUtc.Value.DateTime, TimeZoneInfo.Local);
            DateTime FireTimeUtc = TimeZoneInfo.ConvertTimeFromUtc(context.FireTimeUtc.Value.DateTime, TimeZoneInfo.Local);

            decimal TotalSeconds = decimal.Parse(context.JobRunTime.TotalSeconds.ToString());
            string JobName = string.Empty;
            string LogContent = string.Empty;
            if (context.MergedJobDataMap != null)
            {
                JobName = context.MergedJobDataMap.GetString("JobName");
                System.Text.StringBuilder log = new System.Text.StringBuilder();
                int i = 0;
                foreach (var item in context.MergedJobDataMap)
                {
                    string key = item.Key;
                    if (key.StartsWith("extend_"))
                    {
                        if (i > 0)
                        {
                            log.Append(",");
                        }
                        log.AppendFormat("{0}:{1}", item.Key, item.Value);
                        i++;
                    }
                }
                if (i > 0)
                {
                    LogContent = string.Concat("[", log.ToString(), "]");
                }
            }
            if (jobException != null)
            {
                LogContent = LogContent + " EX:" + jobException.ToString();
            }

            AdminServiceFactory.BackgroundJob.UpdateInfo(GuidUtil.New(), BackgroundJobId, JobName, FireTimeUtc, NextFireTimeUtc, TotalSeconds, LogContent);
        }

        public string Name
        {
            get { return "SchedulerJobListener"; }
        }
    }
}
