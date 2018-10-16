using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lumos.Mvc
{
    public static class ApiMonitorLog
    {
        public static void OnActionExecuting(HttpActionContext filterContext)
        {
            LogUtil.Info(filterContext.Request.RequestUri.AbsoluteUri);
        }

        public static void OnActionExecuted(HttpActionExecutedContext filterContext)
        {

        }
    }
}
