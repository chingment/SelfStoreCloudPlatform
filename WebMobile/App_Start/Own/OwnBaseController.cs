using log4net;
using Lumos;
using Lumos.Common;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

namespace WebMobile
{

    /// <summary>
    /// BaseController用来扩展Controller,凡是在都该继承BaseController
    /// </summary>
    [OwnException]
    [ValidateInput(false)]
    public abstract class OwnBaseController : BaseController
    {


        public OwnBaseController()
        {


        }

        public override string CurrentUserId
        {
            get
            {
                return OwnRequest.GetCurrentUserId();
            }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogUtil.SetTrackId();

            base.OnActionExecuting(filterContext);

            var request = filterContext.HttpContext.Request;

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (!skipAuthorization)
            {
                var userInfo = OwnRequest.GetUserInfo();

                bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();

                string userAgent = filterContext.HttpContext.Request.UserAgent;

                string returnUrl = "";

                if (isAjaxRequest)
                {
                    returnUrl = request.UrlReferrer.PathAndQuery;
                }
                else
                {
                    returnUrl = request.Url.PathAndQuery;
                }

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    LogUtil.Info("OwnBaseController1->returnUrl:" + returnUrl);

                    returnUrl = System.Web.HttpUtility.UrlEncode(returnUrl);

                    LogUtil.Info("OwnBaseController2->returnUrl:" + returnUrl);
                }

                if (userInfo == null)
                {
                    LogUtil.Info("用户没有登录或登录超时");

                    string loginPage = OwnWebSettingUtils.GetLoginPage(returnUrl);

                    if (userAgent.ToLower().Contains("micromessenger"))
                    {
                        LogUtil.Info("去往微信浏览器授权验证");
                        loginPage = OwnWebSettingUtils.WxOauth2(returnUrl);
                    }
                    else
                    {
                        LogUtil.Info("去往用户登录页面验证");
                    }

                    if (isAjaxRequest)
                    {
                        MessageBoxModel messageBox = new MessageBoxModel();
                        messageBox.No = Guid.NewGuid().ToString();
                        messageBox.Type = MessageBoxTip.Failure;
                        messageBox.Title = "请登录";
                        messageBox.LoginPage = loginPage;
                        CustomJsonResult jsonResult = new CustomJsonResult(ResultType.NoLogin, ResultCode.Failure, "", messageBox);
                        filterContext.Result = jsonResult;
                        filterContext.Result.ExecuteResult(filterContext);
                        filterContext.HttpContext.Response.End();
                    }
                    else
                    {
                        filterContext.Result = new RedirectResult(loginPage);
                    }
                }
                else
                {

                    LogUtil.Info("用户Id:" + this.CurrentUserId);
                }
            }
        }
    }

}