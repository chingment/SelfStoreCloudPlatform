using Lumos;
using Lumos.BLL;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.Admin;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebAdmin.Controllers.Sys
{
    public class PositionController : OwnBaseController
    {
        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string positionId)
        {
            return AdminServiceFactory.SysPosition.GetDetails(this.CurrentUserId, positionId);
        }

        [HttpPost]
        public CustomJsonResult Add(RopSysPositionAdd rop)
        {
            return AdminServiceFactory.SysPosition.Add(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopSysPositionEdit rop)
        {
            return AdminServiceFactory.SysPosition.Edit(this.CurrentUserId, rop);
        }
    }
}