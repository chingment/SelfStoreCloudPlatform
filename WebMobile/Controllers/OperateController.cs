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
    public class OperateController : OwnBaseController
    {
        // GET: Operate
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