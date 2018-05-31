using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Lumos.Entity;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Common;
using WebBack.Models.Sys.StaffUser;
using Lumos.Mvc;
using Lumos.DAL;
using Lumos.BLL;

namespace WebBack.Controllers.Sys
{
    [OwnAuthorize(PermissionCode.后台用户管理)]
    public class StaffUserController : OwnBaseController
    {

        #region 视图

        public ViewResult List()
        {
            return View();
        }

        public ViewResult SelectList()
        {
            return View();
        }

        public ViewResult Add()
        {
            AddViewModel model = new AddViewModel();
            return View();
        }

        public ViewResult Edit(int id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }
        #endregion

        public CustomJsonResult GetList(SearchCondition condition)
        {
            var list = (from u in CurrentDb.SysStaffUser
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


        public CustomJsonResult GetSelectList(SearchCondition condition)
        {
            var list = (from u in CurrentDb.SysStaffUser
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


        public CustomJsonResult GetUserRoleTree(int userId = 0)
        {
            var isCheckedIds = CurrentDb.SysUserRole.Where(x => x.UserId == userId).Select(x => x.RoleId);
            object json = ConvertToZTreeJson2(CurrentDb.SysRole.ToArray(), "id", "pid", "name", "role", isCheckedIds.ToArray());

            return Json(ResultType.Success, json);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Add(AddViewModel model)
        {

            var user = new SysStaffUser();
            user.UserName = string.Format("HYL{0}", model.SysStaffUser.UserName);
            user.FullName = model.SysStaffUser.FullName;
            user.PasswordHash = PassWordHelper.HashPassword(model.SysStaffUser.Password);
            user.Email = model.SysStaffUser.Email;
            user.PhoneNumber = model.SysStaffUser.PhoneNumber;
            user.Type = Enumeration.UserType.Staff;
            user.IsDelete = false;
            user.Status = Enumeration.UserStatus.Normal;
            int[] userRoleIds = model.UserRoleIds;

            return SysFactory.AuthorizeRelay.CreateUser<SysStaffUser>(this.CurrentUserId, user, userRoleIds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Edit(EditViewModel model)
        {
            var user = new SysStaffUser();
            user.Id = model.SysStaffUser.Id;
            user.Password = model.SysStaffUser.Password;
            user.FullName = model.SysStaffUser.FullName;
            user.Email = model.SysStaffUser.Email;
            user.PhoneNumber = model.SysStaffUser.PhoneNumber;
            int[] userRoleIds = model.UserRoleIds;
            return SysFactory.AuthorizeRelay.UpdateUser<SysStaffUser>(this.CurrentUserId, user, userRoleIds);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Delete(int[] userIds)
        {
            return SysFactory.AuthorizeRelay.DeleteUser(this.CurrentUserId, userIds);
        }

    }
}