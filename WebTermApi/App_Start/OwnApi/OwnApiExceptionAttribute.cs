using log4net;
using Lumos;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http.Filters;

namespace WebTermApi
{
    public class OwnApiExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogUtil.Error("API调用出现异常", actionExecutedContext.Exception);
            OwnApiHttpResult result = new OwnApiHttpResult(ResultType.Exception, ResultCode.Exception, "程序发生异常");
            actionExecutedContext.Response = new OwnApiHttpResponse(result);
            base.OnException(actionExecutedContext);
        }
    }

}