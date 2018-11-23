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
    [OwnAuthorize(AdminPermissionCode.组织机构管理)]
    public class OrganizationController : OwnBaseController
    {
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }


        public CustomJsonResult GetAll(string pId)
        {

            var arr = CurrentDb.SysOrganization.Where(m => m.BelongSite == Enumeration.BelongSite.Admin && m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();

            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);

        }

        public CustomJsonResult GetDetails(string organizationId)
        {
            return AdminServiceFactory.SysOrganization.GetDetails(this.CurrentUserId, organizationId);
        }


        public CustomJsonResult GetPositionList(RupSysPositionGetList rup)
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

        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(RopSysOrganizationAdd rop)
        {
            return AdminServiceFactory.SysOrganization.Add(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopSysOrganizationEdit rop)
        {
            return AdminServiceFactory.SysOrganization.Edit(this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string[] organizationIds)
        {
            return AdminServiceFactory.SysOrganization.Delete(this.CurrentUserId, organizationIds);
        }

    }
}
