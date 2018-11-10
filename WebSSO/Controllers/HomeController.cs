using Lumos.Common;
using Lumos.Entity;
using Lumos.Web.Mvc;
using Lumos.BLL;
using System.Web.Mvc;
using Lumos.Session;
using System;
using Lumos;
using log4net;
using System.Reflection;
using Lumos.Redis;
using Lumos.Web;
using Lumos.BLL.Sys;

namespace WebSSO.Controllers
{

    public class HomeController : OwnBaseController
    {

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Session["WebSSOLoginVerifyCode"] = null;
            return View();
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [CheckVerifyCode("WebSSOLoginVerifyCode")]
        public CustomJsonResult Login(RopLogin rop)
        {
            GoToViewModel gotoViewModel = new GoToViewModel();

            var result = SysFactory.AuthorizeRelay.SignIn(rop.UserName, rop.Password, CommonUtils.GetIP(), Enumeration.LoginType.Website);

            if (result.ResultType == Enumeration.LoginResult.Failure)
            {

                if (result.ResultTip == Enumeration.LoginResultTip.UserNotExist || result.ResultTip == Enumeration.LoginResultTip.UserPasswordIncorrect)
                {
                    return Json(ResultType.Failure, gotoViewModel, "用户名或密码不正确");
                }

                if (result.ResultTip == Enumeration.LoginResultTip.UserDisabled)
                {
                    return Json(ResultType.Failure, gotoViewModel, "账户被禁用");
                }

                if (result.ResultTip == Enumeration.LoginResultTip.UserDeleted)
                {
                    return Json(ResultType.Failure, gotoViewModel, "账户被删除");
                }
            }

            string host = "";
            string returnUrl = "";


            switch (result.User.Type)
            {
                case Enumeration.UserType.Staff:
                    host = System.Configuration.ConfigurationManager.AppSettings["custom:WebBackUrl"];
                    //returnUrl = string.Format("{0}?returnUrl={1}", host, model.ReturnUrl);
                    returnUrl = string.Format("{0}", host);
                    break;
                case Enumeration.UserType.Merchant:
                    host = System.Configuration.ConfigurationManager.AppSettings["custom:WebMerchUrl"];
                    //returnUrl = string.Format("{0}?returnUrl={1}", host, model.ReturnUrl);
                    returnUrl = string.Format("{0}", host);
                    break;
            }


            UserInfo userInfo = new UserInfo();
            userInfo.UserId = result.User.Id;
            userInfo.UserName = result.User.UserName;

            string token = GuidUtil.New();
            SSOUtil.SetUserInfo(token, userInfo);

            gotoViewModel.Url = string.Format("{0}?token={1}", returnUrl, token);

            return Json(ResultType.Success, gotoViewModel, "登录成功");

        }



    }
}