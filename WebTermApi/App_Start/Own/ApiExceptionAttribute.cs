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
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            Exception ex = actionExecutedContext.Exception;
            LogUtil.Error("API调用出现异常", ex);
            LogUtil.Error(ex.InnerException.Message);
            LogUtil.Error(ex.StackTrace);
        }
    }

}