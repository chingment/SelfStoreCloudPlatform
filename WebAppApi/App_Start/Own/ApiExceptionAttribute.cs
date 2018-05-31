using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace WebAppApi
{


    public class APIExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            Exception ex = actionExecutedContext.Exception;
            log.Error("API调用出现异常", ex);
            log.Error(ex.InnerException.Message);
            log.Error(ex.StackTrace);
        }
    }

}