using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lumos.Web.Http
{
    public class BaseStatisticsTrackerAttribute : ActionFilterAttribute
    {
        public virtual string CurrentUserId { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            actionContext.Request.Headers.Add("CurrentUserId", this.CurrentUserId);
            MonitorLog.OnActionExecuting(actionContext);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            MonitorLog.OnActionExecuted(actionContext);
        }
    }
}
