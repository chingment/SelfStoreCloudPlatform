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


        public CustomJsonResult GetAll()
        {

            var arr = CurrentDb.SysOrganization.Where(m =>m.IsDelete == false).OrderBy(m => m.Priority).ToArray();

            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);

        }

        public CustomJsonResult GetDetails(string id)
        {
            return AdminServiceFactory.SysOrganization.GetDetails(this.CurrentUserId, id);
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
        public CustomJsonResult Delete(string id)
        {
            return AdminServiceFactory.SysOrganization.Delete(this.CurrentUserId, id);
        }

        [HttpPost]
        public CustomJsonResult EditSort(RopSysOrganizationEditSort rop)
        {
            return AdminServiceFactory.SysOrganization.EditSort(this.CurrentUserId, rop);
        }
    }
}
