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
    }
}
