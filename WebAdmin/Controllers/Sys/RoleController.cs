using Lumos;
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
    [OwnAuthorize(AdminPermissionCode.角色管理)]
    public class RoleController : OwnBaseController
    {
        #region 视图
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult AddUserToRole()
        {
            return View();
        }

        #endregion

        #region 方法

        public CustomJsonResult GetAll()
        {
            object obj = Newtonsoft.Json.JsonConvert.DeserializeObject(ConvertToZTreeJson2(CurrentDb.SysRole.ToArray(), "id", "pid", "name", "role"));
            return Json(ResultType.Success, obj);
        }

        public CustomJsonResult GetDetails(string roleId)
        {
            return AdminServiceFactory.SysRole.GetDetails(this.CurrentUserId, roleId);
        }

        public CustomJsonResult GetRoleMenus(string roleId)
        {
            var roleMenus = AdminServiceFactory.SysRole.GetRoleMenus(this.CurrentUserId, roleId);
            var isCheckedIds = from p in roleMenus select p.Id;
            object obj = Newtonsoft.Json.JsonConvert.DeserializeObject(ConvertToZTreeJson(CurrentDb.SysMenu.OrderByDescending(m => m.Priority).ToArray(), "id", "pid", "name", "menu", isCheckedIds.ToArray()));
            return Json(ResultType.Success, obj);

        }

        public CustomJsonResult GetRoleUserList(RupSysRoleUserGetList rup)
        {


            string userName = "";
            if (rup.UserName != null)
            {
                userName = rup.UserName.Trim();
            }

            string fullName = "";
            if (rup.FullName != null)
            {
                fullName = rup.FullName.Trim();
            }


            var list = (from ur in CurrentDb.SysUserRole
                        join r in CurrentDb.SysRole on ur.RoleId equals r.Id
                        join u in CurrentDb.SysAdminUser on ur.UserId equals u.Id
                        where ur.RoleId == rup.RoleId &&
                            (userName.Length == 0 || u.UserName.Contains(userName)) &&
                               (fullName.Length == 0 || u.FullName.Contains(fullName)) &&
                              u.IsDelete == false
                        select new { ur.UserId, ur.RoleId, u.FullName, u.UserName }).Distinct();

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.UserId).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        public CustomJsonResult GetNotBindUsers(RupSysRoleUserGetList rup)
        {

            string userName = "";
            if (rup.UserName != null)
            {
                userName = rup.UserName.Trim();
            }

            string fullName = "";
            if (rup.FullName != null)
            {
                fullName = rup.FullName.Trim();
            }


            var list = (from u in CurrentDb.SysAdminUser
                        where !(from d in CurrentDb.SysUserRole

                                where d.RoleId == rup.RoleId
                                select d.UserId).Contains(u.Id)

                        where
                                              (userName.Length == 0 || u.UserName.Contains(userName)) &&
                               (fullName.Length == 0 || u.FullName.Contains(fullName)) &&
                                                u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName }).Distinct();

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }


        [HttpPost]
        public CustomJsonResult AddUserToRole(string roleId, string[] userIds)
        {
            return AdminServiceFactory.SysRole.AddUserToRole(this.CurrentUserId, roleId, userIds);
        }

        [HttpPost]
        public CustomJsonResult RemoveUserFromRole(string roleId, string[] userIds)
        {
            return AdminServiceFactory.SysRole.RemoveUserFromRole(this.CurrentUserId, roleId, userIds);
        }


        [HttpPost]
        public CustomJsonResult SaveRoleMenu(string roleId, string[] menuIds)
        {
            return AdminServiceFactory.SysRole.SaveRoleMenu(this.CurrentUserId, roleId, menuIds);
        }


        [HttpPost]
        public CustomJsonResult Add(RopSysRoleAdd rop)
        {
            return AdminServiceFactory.SysRole.Add(this.CurrentUserId, rop);
        }


        [HttpPost]
        public CustomJsonResult Edit(RopSysRoleEdit rop)
        {
            return AdminServiceFactory.SysRole.Edit(this.CurrentUserId, rop);
        }


        [HttpPost]
        public CustomJsonResult Delete(string[] roleIds)
        {
            return AdminServiceFactory.SysRole.Delete(this.CurrentUserId, roleIds);
        }

        #endregion
    }
}