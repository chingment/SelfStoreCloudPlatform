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


namespace WebMobile.Controllers
{
    public class OrderController : OwnBaseController
    {
        [AllowAnonymous]
        public ActionResult Confirm()
        {
            return View();
        }

    }
}
