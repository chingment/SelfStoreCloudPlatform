using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Lumos.Entity;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Common;
using Lumos.Web.Mvc;
using Lumos.DAL;
using Lumos.BLL;
using Lumos;
using Lumos.BLL.Service.Admin;

namespace WebAdmin.Controllers.Sys
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

        public CustomJsonResult GetList(RupSysStaffUserGetList rup)
        {
            var list = (from u in CurrentDb.SysStaffUser
                        where (rup.UserName == null || u.UserName.Contains(rup.UserName)) &&
                        (rup.FullName == null || u.FullName.Contains(rup.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        public CustomJsonResult GetSelectList(RupSysStaffUserGetList rup)
        {
            var list = (from u in CurrentDb.SysStaffUser
                        where (rup.UserName == null || u.UserName.Contains(rup.UserName)) &&
                        (rup.FullName == null || u.FullName.Contains(rup.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }



        public CustomJsonResult GetDetails(string userId)
        {
            return AdminServiceFactory.SysStaffUser.GetDetails(this.CurrentUserId, userId);
        }

        [HttpPost]
        public CustomJsonResult Add(RopSysStaffUserAdd rop)
        {
            return AdminServiceFactory.SysStaffUser.Add(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopSysStaffUserEdit rop)
        {
            return AdminServiceFactory.SysStaffUser.Edit(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string[] userIds)
        {
            return AdminServiceFactory.SysStaffUser.Delete(this.CurrentUserId, userIds);
        }

    }
}