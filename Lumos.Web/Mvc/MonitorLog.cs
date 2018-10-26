using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Lumos.Web.Mvc
{
    /// <summary>
    /// 监控日志对象
    /// </summary>
    public static class MonitorLog
    {
        public static void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogUtil.Info(filterContext.RequestContext.HttpContext.Request.RawUrl);
        }
        public static void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
        public static void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }
        public static void OnResultExecuted(ResultExecutedContext filterContext)
        {

        }
    }

}
