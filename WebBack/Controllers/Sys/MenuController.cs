using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service.WebBack;
using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using WebBack.Models.Sys.Menu;

namespace WebBack.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.菜单管理)]
    public class MenuController : OwnBaseController
    {

        public ActionResult List()
        {
            ListViewModel mode = new ListViewModel();
            return View(mode);
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Sort()
        {
            return View();
        }

        public CustomJsonResult GetPermissions()
        {
            return WebBackServiceFactory.SysMenu.GetPermissions(this.CurrentUserId);
        }


        public CustomJsonResult GetDetails(string menuId)
        {
            return WebBackServiceFactory.SysMenu.GetDetails(this.CurrentUserId, menuId);
        }

        public CustomJsonResult GetAll(string pMenuId)
        {
            SysMenu[] arr;
            if (pMenuId == "0")
            {
                arr = CurrentDb.SysMenu.OrderByDescending(m => m.Priority).ToArray();
            }
            else
            {
                arr = CurrentDb.SysMenu.Where(m => m.PId == pMenuId).OrderByDescending(m => m.Priority).ToArray();
            }

            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);
        }


        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(RopSysMenuAdd rop)
        {
            return WebBackServiceFactory.SysMenu.Add(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopSysMenuEdit rop)
        {
            return WebBackServiceFactory.SysMenu.Edit(this.CurrentUserId, rop);

        }

        [HttpPost]
        public CustomJsonResult Delete(string[] menuIds)
        {
            return WebBackServiceFactory.SysMenu.Delete(this.CurrentUserId, menuIds);
        }

        [HttpPost]
        public CustomJsonResult EditSort(RopSysMenuEditSort rop)
        {
            return WebBackServiceFactory.SysMenu.EditSort(this.CurrentUserId, rop);
        }

    }
}