using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Lumos.Entity;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Common;
using Lumos.Web.Mvc;
using Lumos.DAL;
using Lumos.BLL;
using Lumos;
using Lumos.BLL.Service.Admin;
using System.Collections.Generic;

namespace WebAdmin.Controllers.Sys
{
    [OwnAuthorize(AdminPermissionCode.职位管理)]
    public class PositionController : OwnBaseController
    {

        #region 视图

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit()
        {
            return View();
        }
        #endregion

        public CustomJsonResult GetList(RupSysPositionGetList rup)
        {
            var query = (from u in CurrentDb.SysPosition
                         where (rup.Name == null || u.Name.Contains(rup.Name)) &&
                          u.BelongSite == rup.BelongSite
                         select new { u.Id, u.Name, u.CreateTime });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetDetails(Enumeration.SysPositionId id)
        {
            return AdminServiceFactory.SysPosition.GetDetails(this.CurrentUserId, id);
        }

        //[HttpPost]
        //public CustomJsonResult Add(RopSysPositionAdd rop)
        //{
        //    return AdminServiceFactory.SysPosition.Add(this.CurrentUserId, rop);
        //}

        [HttpPost]
        public CustomJsonResult Edit(RopSysPositionEdit rop)
        {
            return AdminServiceFactory.SysPosition.Edit(this.CurrentUserId, rop);
        }

        //[HttpPost]
        //public CustomJsonResult Delete(string id)
        //{
        //    return AdminServiceFactory.SysPosition.Delete(this.CurrentUserId, id);
        //}

    }
}