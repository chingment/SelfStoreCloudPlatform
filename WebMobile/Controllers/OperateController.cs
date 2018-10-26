using System.Web.Mvc;
using Lumos;
using Lumos.BLL.Service.App;

namespace WebMobile.Controllers
{
    public class OperateController : OwnBaseController
    {
        public ActionResult Result()
        {
            return View();
        }

        public CustomJsonResult GetResult(RupOperateGetResult rup)
        {
            return AppServiceFactory.Operate.GetResult(this.CurrentUserId, this.CurrentUserId, rup);
        }
    }
}