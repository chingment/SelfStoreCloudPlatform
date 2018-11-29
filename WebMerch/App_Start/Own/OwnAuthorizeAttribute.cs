﻿using System;
using System.Web;
using System.Web.Mvc;
using Lumos.Web;
using Lumos;

namespace WebMerch
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnAuthorizeAttribute : ActionFilterAttribute
    {
        public string[] _permissions;

        public OwnAuthorizeAttribute(params string[] permissions)
        {
            if (permissions != null)
            {
                if (permissions.Length > 0)
                {
                    _permissions = permissions;
                }
            }

        }

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
                    MessageBox messageBox = new MessageBox();
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

            if (_permissions != null)
            {
                MessageBox messageBox = new MessageBox();
                messageBox.No = Guid.NewGuid().ToString();
                messageBox.Type = MessageBoxTip.Warn;
                messageBox.Title = "温馨提示";
                messageBox.Content = "您没有权限";

                bool isHasPermission = OwnRequest.IsInPermission(_permissions);

                if (!isHasPermission)
                {
                    if (isAjaxRequest)
                    {
                        CustomJsonResult jsonResult = new CustomJsonResult(ResultType.Exception, ResultCode.Exception, messageBox.Title, messageBox);
                        //jsonResult.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                        filterContext.Result = jsonResult;
                        filterContext.Result.ExecuteResult(filterContext);
                        filterContext.HttpContext.Response.End();
                    }
                    else
                    {
                        filterContext.Result = new ViewResult { ViewName = "MessageBox", MasterName = "_Layout", ViewData = new ViewDataDictionary { Model = messageBox } };
                    }

                    return;
                }
            }

            OwnRequest.Postpone();

            base.OnActionExecuting(filterContext);
        }

    }

}