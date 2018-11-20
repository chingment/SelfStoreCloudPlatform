using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class BackgroundJobProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, RopBackgroundJobAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();


            var backgroundJob = new BackgroundJob();
            backgroundJob.Id = GuidUtil.New();
            backgroundJob.Name = rop.Name;
            backgroundJob.JobType = rop.JobType;
            backgroundJob.AssemblyName = rop.AssemblyName;
            backgroundJob.ClassName = rop.ClassName;
            backgroundJob.CronExpression = rop.CronExpression;
            backgroundJob.CronExpressionDescription = rop.CronExpressionDescription;
            backgroundJob.Description = rop.Description;
            backgroundJob.DisplayOrder = 0;
            backgroundJob.RunCount = 0;
            backgroundJob.Status = Enumeration.BackgroundJobStatus.Stoped;
            backgroundJob.IsDelete = false;
            backgroundJob.Creator = pOperater;
            backgroundJob.CreateTime = DateTime.Now;


            CurrentDb.BackgroundJob.Add(backgroundJob);
            CurrentDb.SaveChanges();


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功");

            return result;
        }

        public void WriteLog(string pOperater, string pBackgroundJobId, string pJobName, DateTime pExecutionTime, double pExecutionDuration, string pRunLog)
        {
            var backgroundJobLog = new BackgroundJobLog();
            backgroundJobLog.Id = GuidUtil.New();
            backgroundJobLog.BackgroundJobId = pBackgroundJobId;
            backgroundJobLog.JobName = pJobName;
            backgroundJobLog.ExecutionTime = pExecutionTime;
            backgroundJobLog.ExecutionDuration = pExecutionDuration;
            backgroundJobLog.CreateTime = DateTime.Now;
            backgroundJobLog.RunLog = pRunLog;
            CurrentDb.BackgroundJobLog.Add(backgroundJobLog);
            CurrentDb.SaveChanges();


        }


        public CustomJsonResult SetStartOrStop(string pOperater, string pBackgroundJobId)
        {
            CustomJsonResult result = new CustomJsonResult();
            var backgroundJob = CurrentDb.BackgroundJob.Where(m => m.Id == pBackgroundJobId).FirstOrDefault();

            if (backgroundJob == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到该任务");
            }

            if (backgroundJob.Status == Enumeration.BackgroundJobStatus.Runing)
            {
                SetStatus(pOperater, pBackgroundJobId, Enumeration.BackgroundJobStatus.Stoping);
            }
            else if (backgroundJob.Status == Enumeration.BackgroundJobStatus.Stoped)
            {
                SetStatus(pOperater, pBackgroundJobId, Enumeration.BackgroundJobStatus.Starting);
            }

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "设置成功");

            return result;
        }

        public CustomJsonResult SetStatus(string pOperater, string pBackgroundJobId, Enumeration.BackgroundJobStatus pStatus)
        {
            CustomJsonResult result = new CustomJsonResult();
            var backgroundJob = CurrentDb.BackgroundJob.Where(m => m.Id == pBackgroundJobId).FirstOrDefault();
            if (backgroundJob != null)
            {
                backgroundJob.Status = pStatus;
                backgroundJob.MendTime = this.DateTime;
                CurrentDb.SaveChanges();
            }

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "设置成功");

            return result;
        }

        public bool UpdateInfo(string pOperater, string pBackgroundJobId, string pJobName, DateTime pLastRunTime, DateTime pNextRunTime, double pExecutionDuration, string pRunLog)
        {
            var backgroundJob = CurrentDb.BackgroundJob.Where(m => m.Id == pBackgroundJobId).FirstOrDefault();
            if (backgroundJob != null)
            {
                backgroundJob.RunCount += 1;
                backgroundJob.LastRunTime = pLastRunTime;
                backgroundJob.NextRunTime = pNextRunTime;
                backgroundJob.MendTime = this.DateTime;
                CurrentDb.SaveChanges();
            }

            WriteLog(pOperater, pBackgroundJobId, pJobName, this.DateTime, pExecutionDuration, pRunLog);

            return true;
        }

        public List<BackgroundJob> GeAllowScheduleJobInfoList()
        {

            List<BackgroundJob> list = null;
            list = CurrentDb.BackgroundJob.Where(it => it.IsDelete == false && (it.Status == Enumeration.BackgroundJobStatus.Runing || it.Status == Enumeration.BackgroundJobStatus.Starting || it.Status == Enumeration.BackgroundJobStatus.Stoping)).OrderBy(it => it.CreateTime).ToList();
            return list;
        }
    }
}
