using Lumos.BLL;
using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebBack.Models.Sys.Role;

namespace WebBack.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.角色管理)]
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
        public CustomJsonResult GetRoleTree()
        {
            object json = ConvertToZTreeJson2(CurrentDb.SysRole.ToArray(), "id", "pid", "name", "role");
            return Json(ResultType.Success, json);
        }


        public CustomJsonResult GetDetails(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model);
        }

        public CustomJsonResult GetRoleMenuTreeList(int roleId)
        {
            var roleMenus = SysFactory.AuthorizeRelay.GetRoleMenus(roleId);
            var isCheckedIds = from p in roleMenus select p.Id;
            object json = ConvertToZTreeJson(CurrentDb.SysMenu.OrderByDescending(m => m.Priority).ToArray(), "id", "pid", "name", "menu", isCheckedIds.ToArray());

            return Json(ResultType.Success, json);

        }

        public CustomJsonResult GetRoleUserList(RoleUserSearchCondition condition)
        {


            string userName = "";
            if (condition.UserName != null)
            {
                userName = condition.UserName.Trim();
            }

            string fullName = "";
            if (condition.FullName != null)
            {
                fullName = condition.FullName.Trim();
            }


            var list = (from ur in CurrentDb.SysUserRole
                        join r in CurrentDb.SysRole on ur.RoleId equals r.Id
                        join u in CurrentDb.SysStaffUser on ur.UserId equals u.Id
                        where ur.RoleId == condition.RoleId &&
                            (userName.Length == 0 || u.UserName.Contains(userName)) &&
                               (fullName.Length == 0 || u.FullName.Contains(fullName)) &&
                              u.IsDelete == false
                        select new { ur.UserId, ur.RoleId, u.FullName, u.UserName }).Distinct();

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.UserId).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        public CustomJsonResult GetNotBindUsers(RoleUserSearchCondition condition)
        {

            string userName = "";
            if (condition.UserName != null)
            {
                userName = condition.UserName.Trim();
            }

            string fullName = "";
            if (condition.FullName != null)
            {
                fullName = condition.FullName.Trim();
            }


            var list = (from u in CurrentDb.SysStaffUser
                        where !(from d in CurrentDb.SysUserRole

                                where d.RoleId == condition.RoleId
                                select d.UserId).Contains(u.Id)

                        where
                                              (userName.Length == 0 || u.UserName.Contains(userName)) &&
                               (fullName.Length == 0 || u.FullName.Contains(fullName)) &&
                                                u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName }).Distinct();

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult AddUserToRole(int roleId, int[] userIds)
        {
            return SysFactory.AuthorizeRelay.AddUserToRole(this.CurrentUserId, roleId, userIds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult RemoveUserFromRole(int roleId, int[] userIds)
        {
            return SysFactory.AuthorizeRelay.RemoveUserFromRole(this.CurrentUserId, roleId, userIds);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult SaveRoleMenu(int roleId, int[] menuIds)
        {
            return SysFactory.AuthorizeRelay.SaveRoleMenu(this.CurrentUserId, roleId, menuIds);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Add(AddViewModel model)
        {
            return SysFactory.AuthorizeRelay.CreateRole(this.CurrentUserId, model.SysRole);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return SysFactory.AuthorizeRelay.UpdateRole(this.CurrentUserId, model.SysRole);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Delete(int[] ids)
        {
            return SysFactory.AuthorizeRelay.DeleteRole(this.CurrentUserId, ids);
        }

        #endregion
    }
}