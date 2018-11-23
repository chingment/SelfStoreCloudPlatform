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
        public CustomJsonResult GetList(RupSysPositionGetList rup)
        {
            string name = rup.Name.ToSearchString();


            var list = (from u in CurrentDb.SysPosition
                        where u.OrganizationId == rup.OrganizationId &&
                            (name.Length == 0 || u.Name.Contains(name)) &&
                              u.IsDelete == false
                        select new { u.Id, u.Name, u.CreateTime }).Distinct();

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderBy(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity);
        }


        public CustomJsonResult GetDetails(string id)
        {
            return AdminServiceFactory.SysPosition.GetDetails(this.CurrentUserId, id);
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

        [HttpPost]
        public CustomJsonResult Delete(string[] ids)
        {
            return AdminServiceFactory.SysPosition.Delete(this.CurrentUserId, ids);
        }
    }
}