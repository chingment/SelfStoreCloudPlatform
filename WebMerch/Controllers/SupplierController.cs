using Lumos;
using Lumos.BLL;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMerch.Models.Supplier;

namespace WebMerch.Controllers
{
    public class SupplierController : OwnBaseController
    {
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            AddViewModel model = new AddViewModel();
            return View(model);
        }

        public ViewResult Edit(string id)
        {
            EditViewModel model = new EditViewModel();
            model.LoadData(id);
            return View(model);
        }

        [HttpPost]
        public CustomJsonResult GetList(SearchCondition condition)
        {
            string name = "";
            if (condition.Name != null)
            {
                name = condition.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Company
                         where (name.Length == 0 || u.Name.Contains(name)) &&
                         u.Class == Enumeration.CompanyClass.Supplier
                         select new { u.Id, u.Name, u.Address, u.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
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
                    Address = item.Address,
                    CreateTime = item.CreateTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult Add(AddViewModel model)
        {
            model.Company.UserId = this.CurrentUserId;
            model.Company.Class = Lumos.Entity.Enumeration.CompanyClass.Supplier;
            return BizFactory.Company.Add(this.CurrentUserId, model.Company);
        }

        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.Company.Edit(this.CurrentUserId, model.Company);
        }
    }
}