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

        public CustomJsonResult GetAll(Enumeration.BelongSite belongSite)
        {
            var arr = CurrentDb.SysRole.Where(m => m.BelongSite == belongSite).OrderBy(m => m.Priority).ToArray();
            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "role");
            return Json(ResultType.Success, json);
        }

        public CustomJsonResult GetDetails(string id)
        {
            return AdminServiceFactory.SysRole.GetDetails(this.CurrentUserId, id);
        }

        public CustomJsonResult GetMenus(string id)
        {
            var roleMenus = AdminServiceFactory.SysRole.GetMenus(this.CurrentUserId, id);
            var isCheckedIds = from p in roleMenus select p.Id;
            object obj = ConvertToZTreeJson(CurrentDb.SysMenu.OrderBy(m => m.Priority).ToArray(), "id", "pid", "name", "menu", isCheckedIds.ToArray());
            return Json(ResultType.Success, obj);

        }


        [HttpPost]
        public CustomJsonResult SaveMenu(string id, string[] menuIds)
        {
            return AdminServiceFactory.SysRole.SaveMenus(this.CurrentUserId, id, menuIds);
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
        public CustomJsonResult Delete(string id)
        {
            return AdminServiceFactory.SysRole.Delete(this.CurrentUserId, id);
        }

        [HttpPost]
        public CustomJsonResult EditSort(RopSysRoleEditSort rop)
        {
            return AdminServiceFactory.SysRole.EditSort(this.CurrentUserId, rop);
        }

        #endregion
    }
}