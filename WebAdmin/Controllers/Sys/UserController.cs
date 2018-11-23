using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Lumos.Entity;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Common;
using Lumos.Web.Mvc;
using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service.Admin;

namespace WebAdmin.Controllers.Sys
{
    [OwnAuthorize(AdminPermissionCode.所有用户管理)]
    public class UserController : OwnBaseController
    {

        #region 视图

        public ViewResult List()
        {
            return View();
        }


        public ViewResult Details()
        {
            return View();
        }
        #endregion

        #region 方法

        public CustomJsonResult GetList(RupSysUserGetList rup)
        {
            var list = (from u in CurrentDb.SysUser
                        where (rup.UserName == null || u.UserName.Contains(rup.UserName)) &&
                        (rup.FullName == null || u.FullName.Contains(rup.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        public CustomJsonResult GetDetails(string userId)
        {
            return AdminServiceFactory.SysUser.GetDetails(this.CurrentUserId, userId);
        }

        #endregion

    }
}
