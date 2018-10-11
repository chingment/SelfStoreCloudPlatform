using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using WebMobile.Models;
using Lumos.Common;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Redis;
using System.ComponentModel.DataAnnotations;
using Lumos;
using WebMobile.Models.Account;
using Lumos.Session;
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
        public CustomJsonResult UnifiedOrder(RopUnifiedOrder rop)
        {
            return AppServiceFactory.Order.UnifiedOrder(this.CurrentUserId, this.CurrentUserId, rop);
        }

        public CustomJsonResult GetPayResult(RupPayResult rup)
        {
            return AppServiceFactory.Order.PayResult(this.CurrentUserId, rup);
        }
    }
}
