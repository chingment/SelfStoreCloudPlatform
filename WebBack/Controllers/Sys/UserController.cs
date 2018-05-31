using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Lumos.Entity;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Common;
using WebBack.Models.Sys.User;
using Lumos.Mvc;

namespace WebBack.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.所有用户管理)]
    public class UserController : OwnBaseController
    {

        #region 视图

        public ViewResult List()
        {
            return View();
        }


        public ViewResult Details(int id)
        {
            DetailsViewModel model =new DetailsViewModel(id);
            return View(model);
        }
        #endregion

        #region 方法

        public CustomJsonResult GetList(SearchCondition condition)
        {
            var list = (from u in CurrentDb.SysUser
                        where (condition.UserName == null || u.UserName.Contains(condition.UserName)) &&
                        (condition.FullName == null || u.FullName.Contains(condition.FullName)) &&
                        u.IsDelete == false
                        select new { u.Id, u.UserName, u.FullName, u.Email, u.PhoneNumber, u.CreateTime, u.IsDelete });

            int total = list.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            list = list.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }


        #endregion

    }
}
