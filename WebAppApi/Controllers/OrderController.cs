﻿using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models.Order;
using WebAppApi.Models;
using Lumos.BLL.Service;
using System.Web;
using Lumos;
using Lumos.BLL.Service.App;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class OrderController : OwnBaseApiController
    {

        public APIResponse Confirm(OrderConfirmModel pms)
        {
            IResult result = AppServiceFactory.Order.Confrim(pms.UserId, pms);

            return new APIResponse(result);
        }
    }
}