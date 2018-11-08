﻿using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Lumos.Entity;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Common;
using WebBack.Models.Sys.StaffUser;
using Lumos.Web.Mvc;
using Lumos.DAL;
using Lumos.BLL;
using Lumos;
using Lumos.BLL.Service.WebBack;

namespace WebBack.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.后台用户管理)]
    public class StaffUserController : OwnBaseController
    {

        #region 视图

        public ViewResult List()
        {
            return View();
        }

        public ViewResult SelectList()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit()
        {
            return View();
        }
        #endregion

        public CustomJsonResult GetList(SearchCondition condition)
        {
            var list = (from u in CurrentDb.SysStaffUser
                        where (condition.UserName == null || u.UserName.Contains(condition.UserName)) &&
                        (condition.FullName == null || u.FullName.Contains(condition.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        public CustomJsonResult GetSelectList(SearchCondition condition)
        {
            var list = (from u in CurrentDb.SysStaffUser
                        where (condition.UserName == null || u.UserName.Contains(condition.UserName)) &&
                        (condition.FullName == null || u.FullName.Contains(condition.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        public CustomJsonResult GetInitDataByAddView()
        {
            return WebBackServiceFactory.SysStaffUser.GetInitDataByAddView(this.CurrentUserId);
        }

        public CustomJsonResult GetInitDataByEditView(string userId)
        {
            return WebBackServiceFactory.SysStaffUser.GetInitDataByEditView(this.CurrentUserId, userId);
        }

        [HttpPost]
        public CustomJsonResult Add(RopSysStaffUserAdd rop)
        {
            return WebBackServiceFactory.SysStaffUser.Add(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopSysStaffUserEdit rop)
        {
            return WebBackServiceFactory.SysStaffUser.Edit(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string[] userIds)
        {
            return WebBackServiceFactory.SysStaffUser.Edit(this.CurrentUserId, userIds);
        }

    }
}