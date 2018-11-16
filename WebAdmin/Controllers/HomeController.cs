using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using log4net;
using System.Web.Mvc;
using Lumos.Web.Mvc;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Lumos.BLL;
using System.Security.Cryptography;
using System.Text;
using MySDK;
using System.Linq;
using System.Data.Entity.Core.Objects;
using Lumos;
using Lumos.BLL.Service.Admin;

namespace WebAdmin.Controllers
{

    public class HomeController : OwnBaseController
    {
        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Main()
        {
            return View();
        }


        public ViewResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public CustomJsonResult LogOff()
        {
            OwnRequest.Quit();
            var ret = new { url = OwnWebSettingUtils.GetLoginPage("") };
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "退出成功", ret);
        }

        [HttpPost]
        public CustomJsonResult ChangePassword(RopChangePassword rop)
        {
            var result = AdminServiceFactory.AuthorizeRelay.ChangePassword(this.CurrentUserId, this.CurrentUserId, rop.OldPassword, rop.NewPassword);

            if (result.Result != ResultType.Success)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, result.Message);
            }

            OwnRequest.Quit();

            return Json(ResultType.Success, "点击<a href=\"" + OwnWebSettingUtils.GetLoginPage("") + "\">登录</a>");

        }


        public CustomJsonResult GetIndexData()
        {
            var ret = new IndexModel();

            ret.Title = OwnWebSettingUtils.GetWebName();
            ret.IsLogin = OwnRequest.IsLogin();

            if (ret.IsLogin)
            {
                ret.UserName = OwnRequest.GetUserNameWithSymbol();

                string pId = "00000000000000000000000000000001";
                var menus = OwnRequest.GetMenus();
                var menuLevel1 = from c in menus where c.PId == pId select c;
                foreach (var menuLevel1Child in menuLevel1)
                {
                    var menuModel1 = new IndexModel.MenuModel();

                    menuModel1.Name = menuLevel1Child.Name;


                    var menuLevel2 = from c in menus where c.PId == menuLevel1Child.Id select c;

                    foreach (var menuLevel2Child in menuLevel2)
                    {

                        menuModel1.SubMenus.Add(new IndexModel.SubMenuModel { Name = menuLevel2Child.Name, Url = menuLevel2Child.Url });
                    }

                    ret.MenuNavByLeft.Add(menuModel1);
                }

            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }


        public class IndexModel
        {
            public IndexModel()
            {
                this.MenuNavByLeft = new List<MenuModel>();
            }

            public string Title { get; set; }

            public bool IsLogin { get; set; }

            public string UserName { get; set; }

            public List<MenuModel> MenuNavByLeft { get; set; }

            public class MenuModel
            {
                public MenuModel()
                {
                    this.SubMenus = new List<SubMenuModel>();
                }

                public string Name { get; set; }

                public List<SubMenuModel> SubMenus { get; set; }
            }

            public class SubMenuModel
            {
                public string Url { get; set; }

                public string Name { get; set; }
            }
        }
    }
}