using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using WebSSO.Models.Home;
using System.Web.Mvc;
using Lumos.Session;
using System;

namespace WebSSO.Controllers
{

    public class HomeController : OwnBaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            Session["WebSSOLoginVerifyCode"] = null;

            LoginModel model = new LoginModel();
            model.ReturnUrl = returnUrl;

            return View(model);
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [CheckVerifyCode("WebSSOLoginVerifyCode")]
        public CustomJsonResult Login(LoginModel model)
        {
            GoToViewModel gotoViewModel = new GoToViewModel();

            var result = SysFactory.AuthorizeRelay.SignIn(model.UserName, model.Password, CommonUtils.GetIP(), Enumeration.LoginType.Website);

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
                case Enumeration.UserType.Client:
                    break;
                case Enumeration.UserType.Agent:
                    host = System.Configuration.ConfigurationManager.AppSettings["custom:WebAgentUrl"];
                    //returnUrl = string.Format("{0}?returnUrl={1}", host, model.ReturnUrl);
                    returnUrl = string.Format("{0}", host);
                    break;
                case Enumeration.UserType.Salesman:
                    break;
            }



            UserInfo userInfo = new UserInfo();
            userInfo.UserId = result.User.Id;
            userInfo.UserName = result.User.UserName;
            userInfo.Token = Guid.NewGuid().ToString().Replace("-", "");

            SSOUtil.SetUserInfo(userInfo);

            gotoViewModel.Url = string.Format("{0}?token={1}", returnUrl, userInfo.Token);

            return Json(ResultType.Success, gotoViewModel, "登录成功");

        }
    }
}