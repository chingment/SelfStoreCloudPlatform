using log4net;
using Lumos;
using System;
using System.Reflection;
using System.Web.Http.Filters;

namespace WebAppApi
{


    public class OwnApiExceptionAttribute : ExceptionFilterAttribute, IExceptionFilter
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            LogicalThreadContext.Properties["trackid"] = Guid.NewGuid().ToString();
            log.Error("API调用出现异常", ex);

            OwnApiHttpResult result = new OwnApiHttpResult(ResultType.Exception, ResultCode.Exception, "程序发生异常");
            actionExecutedContext.Response = new OwnApiHttpResponse(result);

            base.OnException(actionExecutedContext);

        }
    }

}