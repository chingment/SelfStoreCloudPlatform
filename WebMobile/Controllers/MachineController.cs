using Lumos;
using Lumos.BLL.Service.App;
using System.Web.Mvc;

namespace WebMobile.Controllers
{
    public class MachineController : OwnBaseController
    {
        public ActionResult LoginByQrCode()
        {
            return View();
        }


        [HttpPost]
        public CustomJsonResult LoginByQrCode(RopMachineLoginByQrCode rop)
        {
            return AppServiceFactory.Machine.LoginByQrCode(this.CurrentUserId,this.CurrentUserId, rop);
        }
    }
}
