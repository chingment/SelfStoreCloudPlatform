using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAgent.Controllers
{
    public class CompanyController : Controller
    {
        public ActionResult ListBySupplier()
        {
            return View();
        }
    }
}