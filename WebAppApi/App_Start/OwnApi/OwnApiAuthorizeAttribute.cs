using System;
using System.Web;
using System.Collections.Specialized;
using System.Net;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Collections.Generic;
using Lumos.Mvc;
using System.Globalization;
using log4net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Lumos;
using Lumos.BLL;
using Lumos.Common;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Lumos.Session;

namespace WebAppApi
{

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class OwnApiAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            try
            {
                ApiMonitorLog.OnActionExecuting(actionContext);

                DateTime requestTime = DateTime.Now;
                var request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
                var requestMethod = request.HttpMethod;

                bool skipAuthorization = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
                if (skipAuthorization)
                {
                    return;
                }

                var accessToken = request.QueryString["accessToken"];

                if (string.IsNullOrEmpty(accessToken))
                {
                    OwnApiHttpResult result = new OwnApiHttpResult(ResultType.Failure, ResultCode.FailureSign, "accessToken不能为空");
                    actionContext.Response = new OwnApiHttpResponse(result);
                    return;
                }

                var userInfo = SSOUtil.GetUserInfo(accessToken);

                if (userInfo == null)
                {
                    OwnApiHttpResult result = new OwnApiHttpResult(ResultType.Failure, ResultCode.FailureSign, "accessToken 已经超时");
                    actionContext.Response = new OwnApiHttpResponse(result);
                    return;
                }

                base.OnActionExecuting(actionContext);
            }
            catch (Exception ex)
            {
                LogUtil.Error(string.Format("API错误:{0}", ex.Message), ex);
                OwnApiHttpResult result = new OwnApiHttpResult(ResultType.Exception, ResultCode.Exception, "内部错误");
                actionContext.Response = new OwnApiHttpResponse(result);

                return;
            }
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            ApiMonitorLog.OnActionExecuted(actionContext);
        }
    }
}