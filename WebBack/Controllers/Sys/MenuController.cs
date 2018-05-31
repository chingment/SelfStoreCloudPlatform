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
            AddViewModel mode = new AddViewModel();
            return View(mode);
        }

        public ActionResult Sort()
        {
            return View();
        }



        public CustomJsonResult GetDetails(int id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model, "");
        }

        public CustomJsonResult GetTree(int pId)
        {
            SysMenu[] arr;
            if (pId == 0)
            {
                arr = CurrentDb.SysMenu.OrderByDescending(m => m.Priority).ToArray();
            }
            else
            {
                arr = CurrentDb.SysMenu.Where(m => m.PId == pId).OrderByDescending(m => m.Priority).ToArray();
            }

            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);
        }


        [HttpPost]
        [OwnNoResubmit]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Add(AddViewModel model)
        {
            return SysFactory.AuthorizeRelay.CreateMenu(this.CurrentUserId, model.SysMenu, model.SysMenu.Permission);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Edit(EditViewModel model)
        {
            var menu = new SysMenu();
            menu.Id = model.SysMenu.Id;
            menu.Name = model.SysMenu.Name;
            menu.Url = model.SysMenu.Url;
            menu.Description = model.SysMenu.Description;

            return SysFactory.AuthorizeRelay.UpdateMenu(this.CurrentUserId, model.SysMenu, model.SysMenu.Permission);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Delete(int[] ids)
        {
            return SysFactory.AuthorizeRelay.DeleteMenu(this.CurrentUserId, ids);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult EditSort()
        {

            for (int i = 0; i < Request.Form.Count; i++)
            {
                string name = Request.Form.AllKeys[i].ToString();
                if (name.IndexOf("menuId") > -1)
                {
                    int id = int.Parse(name.Split('_')[1].Trim());
                    int priority = int.Parse(Request.Form[i].Trim());
                    SysMenu model = CurrentDb.SysMenu.Where(m => m.Id == id).FirstOrDefault();
                    if (model != null)
                    {
                        model.Priority = priority;
                        CurrentDb.SaveChanges();
                    }
                }
            }
            return Json(ResultType.Success, "保存成功");

        }

    }
}