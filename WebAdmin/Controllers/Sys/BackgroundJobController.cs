﻿using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service.Admin;
using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace WebAdmin.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.后台任务管理)]
    public class BackgroundJobController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public CustomJsonResult GetList(RupBaseGetList rup)
        {
            var query = (from u in CurrentDb.BackgroundJob
                         where (rup.Name == null || u.Name.Contains(rup.Name)) &&
                         u.IsDelete == false
                         select new { u.Id, u.Name, u.JobType, u.AssemblyName, u.ClassName, u.Description, u.CronExpression, u.CronExpressionDescription, u.NextRunTime, u.LastRunTime, u.RunCount, u.Status, u.IsDelete, u.CreateTime });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {
                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    JobType = item.JobType,
                    AssemblyName = item.AssemblyName,
                    ClassName = item.ClassName,
                    Description = item.Description,
                    CronExpression = item.CronExpression,
                    CronExpressionDescription = item.CronExpressionDescription,
                    NextRunTime = item.NextRunTime.ToUnifiedFormatDate(),
                    LastRunTime = item.LastRunTime.ToUnifiedFormatDate(),
                    RunCount = item.RunCount,
                    Status = item.Status,
                    StatusName = item.Status.GetCnName(),
                    IsDelete = item.IsDelete,
                    CreateTime = item.CreateTime.ToUnifiedFormatDate()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Add(RopBackgroundJobAdd rop)
        {
            return AdminServiceFactory.BackgroundJob.Add(this.CurrentUserId, rop);
        }
    }
}