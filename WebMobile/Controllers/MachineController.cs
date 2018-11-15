using Lumos;
using Lumos.BLL.Service.AppMobile;
using System;
using System.Web;
using System.Web.Mvc;

namespace WebMobile.Controllers
{
    public class MachineController : OwnBaseController
    {
        public ActionResult LoginByQrCode()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public CustomJsonResult GetLoginConfirmInfo(RupMachineGetLoginConfirmInfo rup)
        {
            return AppServiceFactory.Machine.GetLoginConfirmInfo(this.CurrentUserId, this.CurrentUserId, rup);
        }

        [HttpPost]
        public CustomJsonResult LoginByQrCode(RopMachineLoginByQrCode rop)
        {
            return AppServiceFactory.Machine.LoginByQrCode(this.CurrentUserId, this.CurrentUserId, rop);
        }
    }
}
