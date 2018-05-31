using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WeiXinSdkWebTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Ip = CommonUtils.GetIP();
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
