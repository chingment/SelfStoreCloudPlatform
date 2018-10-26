
using System;
using System.Web.Mvc;

namespace Lumos.Web.Mvc
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class BaseStatisticsTrackerAttribute : ActionFilterAttribute
    {
        public virtual string CurrentUserId { get; set; }


        public BaseStatisticsTrackerAttribute()
        {

        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Request.Headers.Add("CurrentUserId", this.CurrentUserId);

            MonitorLog.OnActionExecuting(filterContext);
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            MonitorLog.OnActionExecuted(filterContext);
        }
        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
          
        //}
        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
            
        //}
    }

}