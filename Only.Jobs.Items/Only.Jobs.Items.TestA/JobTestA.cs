﻿using log4net;
using Quartz;
using System;

namespace Only.Jobs.Items.TestA
{
    //不允许此 Job 并发执行任务（禁止新开线程执行）
    [DisallowConcurrentExecution]
    public sealed class JobTestA : IJob
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(JobTestA));

        public void Execute(IJobExecutionContext context)
        {
            string curRunId = Guid.NewGuid().ToString();

            Version Ver = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            _logger.InfoFormat("JobTestA[{0}] Execute begin Ver.{1}", curRunId, Ver.ToString());
            try
            {
                context.MergedJobDataMap.Put("extend_logA", "JobTestA Executing" + DateTime.Now);
                context.MergedJobDataMap.Put("extend_run_result", "success");
                _logger.InfoFormat("JobTestA[{0}] Executing ...", curRunId);
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("JobTestA[{0}] 执行过程中发生异常:{1}", curRunId, ex.ToString());
            }
            finally
            {
                _logger.InfoFormat("JobTestA[{0}] Execute end", curRunId);
            }
        }
    }
}
