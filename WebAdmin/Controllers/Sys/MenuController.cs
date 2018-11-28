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
    [OwnAuthorize(AdminPermissionCode.菜单管理)]
    public class MenuController : OwnBaseController
    {

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Sort()
        {
            return View();
        }

        public CustomJsonResult GetPermissions(Enumeration.BelongSite belongSite)
        {
            return AdminServiceFactory.SysMenu.GetPermissions(this.CurrentUserId, belongSite);
        }


        public CustomJsonResult GetDetails(string id)
        {
            return AdminServiceFactory.SysMenu.GetDetails(this.CurrentUserId, id);
        }

        public CustomJsonResult GetAll(Enumeration.BelongSite belongSite)
        {
            var arr = CurrentDb.SysMenu.Where(m => m.BelongSite == belongSite).OrderBy(m => m.Priority).ToArray();
            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);
        }

        public CustomJsonResult GetByPId(string pId)
        {
            var arr = CurrentDb.SysMenu.Where(m => m.PId == pId).OrderBy(m => m.Priority).ToArray();
            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);
        }

        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(RopSysMenuAdd rop)
        {
            return AdminServiceFactory.SysMenu.Add(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopSysMenuEdit rop)
        {
            return AdminServiceFactory.SysMenu.Edit(this.CurrentUserId, rop);

        }

        [HttpPost]
        public CustomJsonResult Delete(string id)
        {
            return AdminServiceFactory.SysMenu.Delete(this.CurrentUserId, id);
        }

        [HttpPost]
        public CustomJsonResult EditSort(RopSysMenuEditSort rop)
        {
            return AdminServiceFactory.SysMenu.EditSort(this.CurrentUserId, rop);
        }

    }
}