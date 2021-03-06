﻿using Lumos;
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
    public class StoreController : OwnBaseController
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

        public ViewResult Edit()
        {
            return View();
        }

        [HttpPost]
        public CustomJsonResult GetList(RupStoreGetList rup)
        {
            string name = "";
            if (rup.Name != null)
            {
                name = rup.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Store
                         where (name.Length == 0 || u.Name.Contains(name))
                         && u.MerchantId == this.CurrentMerchantId
                         select new { u.Id, u.Name, u.Address, u.Status, u.CreateTime });

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
                    Status = item.Status.GetValueAndCnName(),
                    CreateTime = item.CreateTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.Store.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        [HttpPost]
        public CustomJsonResult Add(RopStoreAdd rop)
        {
            return MerchServiceFactory.Store.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopStoreEdit rop)
        {
            return MerchServiceFactory.Store.Edit(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        public CustomJsonResult GetMachineList(RupMachineGetList rup)
        {

            string name = "";
            if (rup.Name != null)
            {
                name = rup.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Machine
                         where
                         (name.Length == 0 || u.Name.Contains(name))
                         && u.StoreId == rup.StoreId
                         && u.MerchantId == this.CurrentMerchantId
                         select new { u.Id, u.Name }).Distinct();

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                olist.Add(new
                {
                    item.Id,
                    item.Name
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }

    }
}