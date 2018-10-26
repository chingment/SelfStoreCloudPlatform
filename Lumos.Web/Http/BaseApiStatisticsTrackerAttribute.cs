using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Lumos.Web.Http
{
    public class BaseApiStatisticsTrackerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionContext)
        {
            MonitorLog.OnActionExecuted(actionContext);
        }
       
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            MonitorLog.OnActionExecuting(actionContext);
        }
    }
}
