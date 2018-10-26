using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lumos.Web.Http
{
    public class BaseStatisticsTrackerAttribute : ActionFilterAttribute
    {
        public virtual string CurrentUserId { get; set; }

        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            MonitorLog.OnActionExecuted(this.CurrentUserId,actionContext);
        }
       
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            MonitorLog.OnActionExecuting(this.CurrentUserId,actionContext);
        }
    }
}
