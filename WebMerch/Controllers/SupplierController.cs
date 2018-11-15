using Lumos;
using Lumos.BLL;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            return View();
        }

        public ViewResult Edit()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string companyId)
        {
            return MerchServiceFactory.Company.GetDetails(this.CurrentUserId, this.CurrentUserId, companyId);
        }

        [HttpPost]
        public CustomJsonResult GetList(RupSupplierGetList rup)
        {
            string name = "";
            if (rup.Name != null)
            {
                name = rup.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Company
                         where (name.Length == 0 || u.Name.Contains(name)) &&
                         u.Class == Enumeration.CompanyClass.Supplier
                         && u.MerchantId == this.CurrentUserId
                         select new { u.Id, u.Name, u.Address, u.CreateTime });

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
                    Address = item.Address,
                    CreateTime = item.CreateTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult Add(RopCompanyAdd rop)
        {
            rop.Class = Lumos.Entity.Enumeration.CompanyClass.Supplier;
            return MerchServiceFactory.Company.Add(this.CurrentUserId, this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopCompanyEdit rop)
        {
            return MerchServiceFactory.Company.Edit(this.CurrentUserId, this.CurrentUserId, rop);
        }
    }
}