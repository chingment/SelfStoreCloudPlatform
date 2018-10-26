using System.Web.Mvc;

namespace WebMobile.Controllers
{
    public class MachineController : OwnBaseController
    {
        public ActionResult AuthorizeLogin(string token, string merchantId, string storeId, string machineId)
        {
            return View();
        }
    }
}
