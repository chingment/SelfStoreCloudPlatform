using System.Web.Mvc;
using Lumos;
using Lumos.BLL.Service.App;

namespace WebMobile.Controllers
{
    public class OrderController : OwnBaseController
    {
        public ActionResult Confirm()
        {
            return View();
        }

        public ActionResult PayResult()
        {

            return View();
        }

        [HttpPost]
        public CustomJsonResult Confirm(RopOrderConfirm rop)
        {

            return AppServiceFactory.Order.Confrim(this.CurrentUserId, this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult GetJsApiPaymentPms(RupOrderGetJsApiPaymentPms rop)
        {
            return AppServiceFactory.Order.GetJsApiPaymentPms(this.CurrentUserId, this.CurrentUserId, this.CurrentAppInfo, rop);
        }

    }
}
