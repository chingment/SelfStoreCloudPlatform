﻿using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Common;
using Lumos.BLL;
using WebBack.Models.Biz.Merchant;
using Lumos;

namespace WebBack.Controllers.Biz
{
    public class MerchantController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult ListBySelect()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit(string id)
        {
            EditViewModel model = new EditViewModel(id);
            return View(model);
        }


        public CustomJsonResult GetList(SearchCondition condition)
        {

            string name = condition.Name.ToSearchString();
            var query = (from m in CurrentDb.Merchant
                         join u in CurrentDb.SysUser on m.UserId equals u.Id
                         where
                                 (name.Length == 0 || m.Name.Contains(name))
                         select new { m.Id, u.UserName, m.Name, m.ContactName, m.ContactPhone, m.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            List<object> list = new List<object>();

            foreach (var item in query)
            {

                list.Add(new
                {
                    item.Id,
                    item.UserName,
                    item.Name,
                    item.ContactName,
                    item.ContactPhone,
                    item.CreateTime
                });


            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Add(AddViewModel model)
        {
            model.SysMerchatUser.UserName = string.Format("{0}{1}", "M", model.SysMerchatUser.UserName);

            return BizFactory.Merchant.Add(this.CurrentUserId, model.SysMerchatUser, model.Merchant);
        }

        [HttpPost]

        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.Merchant.Edit(this.CurrentUserId, model.SysMerchatUser, model.Merchant);
        }

    }
}