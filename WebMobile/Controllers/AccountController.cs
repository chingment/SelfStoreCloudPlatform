using System.Web;
using System.Web.Mvc;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Common;
using Lumos;
using Lumos.Session;
using Lumos.Web;
using Newtonsoft.Json;
using Lumos.BLL.Sys;

namespace WebMobile.Controllers
{
    public class AccountController : OwnBaseController
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
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

            UserInfo userInfo = new UserInfo();
            userInfo.UserId = result.User.Id;
            userInfo.UserName = result.User.UserName;

            string accessToken = GuidUtil.New();

            SSOUtil.SetUserInfo(accessToken, userInfo);


            Response.Cookies.Add(new HttpCookie(OwnRequest.SESSION_NAME, accessToken));

            gotoViewModel.Url = (rop.ReturnUrl == null ? OwnWebSettingUtils.GetHomePage() : rop.ReturnUrl);

            return Json(ResultType.Success, gotoViewModel, "登录成功");

        }



    }
}