using System;
using System.Web;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Lumos.Web.Http;
using Lumos;
using System.Web.Http;
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
                DateTime requestTime = DateTime.Now;
                var request = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
                var requestMethod = request.HttpMethod;

                request.Headers.Add("CurrentUserId", OwnApiRequest.GetCurrentUserId() ?? "");

                MonitorLog.OnActionExecuting(actionContext);


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
            MonitorLog.OnActionExecuted(actionContext);
        }
    }
}