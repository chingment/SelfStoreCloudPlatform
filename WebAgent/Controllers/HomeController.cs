using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using log4net;
using System.Web.Mvc;
using WebAgent.Models.Home;
using Lumos.Mvc;
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

namespace WebAgent.Controllers
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

        /// <summary>
        /// 退出方法
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            OwnRequest.Quit();

            return Redirect(OwnWebSettingUtils.GetLoginPage(""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult ChangePassword(ChangePasswordModel model)
        {
            SysFactory.AuthorizeRelay.ChangePassword(this.CurrentUserId, this.CurrentUserId, model.OldPassword, model.NewPassword);

            return Json(ResultType.Success, "点击<a href=\"" + OwnWebSettingUtils.GetLoginPage("") + "\">登录</a>");

        }

    }
}