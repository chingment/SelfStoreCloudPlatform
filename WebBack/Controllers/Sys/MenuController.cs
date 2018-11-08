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

        public CustomJsonResult GetInitDataByAddView()
        {
            return WebBackServiceFactory.SysMenu.GetInitDataByAddView(this.CurrentUserId);
        }


        public CustomJsonResult GetDetails(string id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model, "");
        }

        public CustomJsonResult GetMenus(string pMenuId)
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
        public CustomJsonResult EditSort(List<SysMenu> sysMenu)
        {

            //for (int i = 0; i < Request.Form.Count; i++)
            //{
            //    string name = Request.Form.AllKeys[i].ToString();
            //    if (name.IndexOf("menuId") > -1)
            //    {
            //        string id = name.Split('_')[1].Trim();
            //        int priority = int.Parse(Request.Form[i].Trim());
            //        SysMenu model = CurrentDb.SysMenu.Where(m => m.Id == id).FirstOrDefault();
            //        if (model != null)
            //        {
            //            model.Priority = priority;
            //            CurrentDb.SaveChanges();
            //        }
            //    }
            //}
            return Json(ResultType.Success, "保存成功");

        }

    }
}