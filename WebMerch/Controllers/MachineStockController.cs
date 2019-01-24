using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lumos.Entity;
using Lumos.BLL;
using System.Data;
using Lumos;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;

namespace WebMerch.Controllers
{
    public class MachineStockController : OwnBaseController
    {
        public ViewResult List()
        {
            return View();
        }
    }
}