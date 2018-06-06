using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using log4net;
using Lumos.Common;
using System.Globalization;
using Lumos;

namespace Lumos.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BaseStatisticsTrackerAttribute : ActionFilterAttribute
    {
        public BaseStatisticsTrackerAttribute()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            MonitorLog.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            MonitorLog.OnActionExecuted(filterContext);

        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            MonitorLog.OnResultExecuting(filterContext);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            MonitorLog.OnResultExecuted(filterContext);
        }
    }

}