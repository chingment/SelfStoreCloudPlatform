using Lumos.BLL.Task;
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
        public CustomJsonResult GetDetails(string pOperater, string pBackgroundJobId)
        {
            var ret = new RetBackgroundJobGetDetails();
            var backgroundJob = CurrentDb.BackgroundJob.Where(m => m.Id == pBackgroundJobId).FirstOrDefault();
            if (backgroundJob != null)
            {
                ret.BackgroundJobId = backgroundJob.Id;
                ret.Name = backgroundJob.Name;
                ret.AssemblyName = backgroundJob.AssemblyName;
                ret.ClassName = backgroundJob.ClassName;
                ret.Description = backgroundJob.Description;
                ret.CronExpression = backgroundJob.CronExpression;
                ret.CronExpressionDescription = backgroundJob.CronExpressionDescription;
                ret.JobArgs = backgroundJob.JobArgs;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, RopBackgroundJobAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var isExists = CurrentDb.BackgroundJob.Where(m => m.AssemblyName == rop.AssemblyName && m.ClassName == rop.ClassName).FirstOrDefault();
            if (isExists != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("程序集:{0},类名:{1},已存在"));
            }

            var type = QuartzManager.GetClassInfo(rop.AssemblyName, rop.ClassName);
            if (type == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "程序集或类型无效，无法映射");
            }

            if (!QuartzManager.ValidExpression(rop.CronExpression))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "无效的Cron表达式");
            }

            var backgroundJob = new BackgroundJob();
            backgroundJob.Id = GuidUtil.New();
            backgroundJob.Name = rop.Name;
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

        public CustomJsonResult Edit(string pOperater, RopBackgroundJobEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            if (!QuartzManager.ValidExpression(rop.CronExpression))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "无效的Cron表达式");
            }

            var backgroundJob = CurrentDb.BackgroundJob.Where(m => m.Id == rop.BackgroundJobId).FirstOrDefault();
            //backgroundJob.AssemblyName = rop.AssemblyName;
            //backgroundJob.ClassName = rop.ClassName;
            backgroundJob.CronExpression = rop.CronExpression;
            backgroundJob.CronExpressionDescription = rop.CronExpressionDescription;
            backgroundJob.Description = rop.Description;
            backgroundJob.Mender = pOperater;
            backgroundJob.MendTime = DateTime.Now;

            CurrentDb.SaveChanges();

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

            return result;
        }

        public void WriteLog(string pOperater, string pBackgroundJobId, string pJobName, DateTime pExecutionTime, decimal pExecutionDuration, string pRunLog)
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

        public bool UpdateInfo(string pOperater, string pBackgroundJobId, string pJobName, DateTime pLastRunTime, DateTime pNextRunTime, decimal pExecutionDuration, string pRunLog)
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
            var list = CurrentDb.BackgroundJob.Where(it => it.IsDelete == false).OrderBy(it => it.CreateTime).ToList();
            return list;
        }
    }
}
