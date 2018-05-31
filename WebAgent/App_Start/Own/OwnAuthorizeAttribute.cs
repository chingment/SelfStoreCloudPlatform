using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using log4net;
using Lumos.Common;
using System.Globalization;
using Lumos.Mvc;

namespace WebAgent
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnAuthorizeAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.RequestContext.HttpContext.Request;
            var response = filterContext.RequestContext.HttpContext.Response;
            var isAjaxRequest = request.IsAjaxRequest();
            var userAgent = request.UserAgent;
            var returnUrl = isAjaxRequest == true ? request.UrlReferrer.AbsoluteUri : request.Url.AbsoluteUri;

            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true) || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);
            if (skipAuthorization)
            {
                return;
            }

            string token = request.QueryString["token"];

            if (token != null)
            {
                HttpCookie cookie_session = request.Cookies[OwnRequest.SESSION_NAME];
                if (cookie_session != null)
                {
                    cookie_session.Value = token;
                    response.AppendCookie(cookie_session);
                }
                else
                {
                    response.Cookies.Add(new HttpCookie(OwnRequest.SESSION_NAME, token));
                }
            }

            var userInfo = OwnRequest.GetUserInfo();

            if (userInfo == null)
            {
                if (isAjaxRequest)
                {
                    MessageBoxModel messageBox = new MessageBoxModel();
                    messageBox.No = Guid.NewGuid().ToString();
                    messageBox.Type = MessageBoxTip.Exception;
                    messageBox.Title = "温馨提示";
                    messageBox.GoToUrl = OwnWebSettingUtils.GetLoginPage(returnUrl);
                    messageBox.Content = "请先<a href=\"javascript:void(0)\" onclick=\"window.top.location.href='" + OwnWebSettingUtils.GetLoginPage(returnUrl) + "'\">登录</a>后打开";
                    messageBox.IsTop = true;

                    filterContext.Result = new ViewResult { ViewName = "MessageBox", MasterName = "_Layout", ViewData = new ViewDataDictionary { Model = messageBox } };
                }
                else
                {
                    filterContext.Result = new RedirectResult(OwnWebSettingUtils.GetLoginPage(returnUrl));
                }

                return;
            }

            OwnRequest.Postpone();

            base.OnActionExecuting(filterContext);
        }

    }

}