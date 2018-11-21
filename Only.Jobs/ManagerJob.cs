using log4net;
using Lumos.BLL.Task;
using Quartz;
using System;

namespace Only.Jobs
{
    [DisallowConcurrentExecution]
    public class ManagerJob : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(ManagerJob));

        public void Execute(IJobExecutionContext context)
        {
            string curRunId = Guid.NewGuid().ToString();
            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            _logger.InfoFormat("ManagerJob[{0}] Execute begin Ver.", Ver.ToString(), curRunId);
            try
            {
                QuartzManager.JobScheduler(context.Scheduler);
                _logger.InfoFormat("ManagerJob[{0}] Executing ...", curRunId);
            }
            catch (Exception ex)
            {
                JobExecutionException e2 = new JobExecutionException(ex);
                e2.RefireImmediately = true;
            }
            finally
            {
                _logger.InfoFormat("ManagerJob[{0}] Execute end ", curRunId);
            }
        }
    }
}
