using log4net;
using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace WebTermApi
{


    public class APIExceptionAttribute : ExceptionFilterAttribute
    {
        private static ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            LogicalThreadContext.Properties["trackid"] = Guid.NewGuid().ToString();
            log.Error("API调用出现异常", ex);

            APIResult result = new APIResult(ResultType.Exception, ResultCode.Exception, "程序发生异常");
            actionExecutedContext.Response = new APIResponse(result);

            base.OnException(actionExecutedContext);

        }
    }

}