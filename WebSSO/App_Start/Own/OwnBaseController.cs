using System.Web.Mvc;
using Lumos.Web.Mvc;

namespace WebSSO
{

    /// <summary>
    /// BaseController用来扩展Controller,凡是在都该继承BaseController
    /// </summary>
    //[OwnException]
    [ValidateInput(false)]
    public abstract class OwnBaseController : BaseController
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }
    }
}