using Lumos.BLL;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBack.Models.Sys.AgentUser;

namespace WebBack.Controllers.Sys
{
    public class AgentUserController : OwnBaseController
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
            var list = (from u in CurrentDb.SysAgentUser
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
            var list = (from u in CurrentDb.SysAgentUser
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Add(AddViewModel model)
        {
            SysAgentUser user = new SysAgentUser();
            user.UserName = string.Format("AG{0}", model.SysAgentUser.UserName);
            user.FullName = model.SysAgentUser.FullName;
            user.PasswordHash = PassWordHelper.HashPassword("888888");
            user.Email = model.SysAgentUser.Email;
            user.PhoneNumber = model.SysAgentUser.PhoneNumber;
            user.IsDelete = false;
            user.Status = Enumeration.UserStatus.Normal;
            user.Type = Enumeration.UserType.Agent;
            user.WechatNumber = model.SysAgentUser.WechatNumber;
            return SysFactory.AuthorizeRelay.CreateUser<SysAgentUser>(this.CurrentUserId, user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public CustomJsonResult Edit(EditViewModel model)
        {
            SysAgentUser user = new SysAgentUser();
            user.Id = model.SysAgentUser.Id;
            user.Password = model.SysAgentUser.Password;
            user.FullName = model.SysAgentUser.FullName;
            user.Email = model.SysAgentUser.Email;
            user.PhoneNumber = model.SysAgentUser.PhoneNumber;
            user.WechatNumber = model.SysAgentUser.WechatNumber;
            return SysFactory.AuthorizeRelay.UpdateUser<SysAgentUser>(this.CurrentUserId, user);

        }
    }
}