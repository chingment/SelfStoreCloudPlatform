using Lumos;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMerch.Models.Store;

namespace WebMerch.Controllers.Biz
{
    public class StoreController : OwnBaseController
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

        public ViewResult MachineListByNoBind()
        {
            return View();
        }


        [HttpPost]
        public CustomJsonResult GetList(SearchCondition condition)
        {
            string name = "";
            if (condition.Name != null)
            {
                name = condition.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Store
                         where (name.Length == 0 || u.Name.Contains(name))
                         select new { u.Id, u.Name, u.Address, u.Status, u.CreateTime });

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
                    Status = item.Status.GetValueAndCnName(),
                    CreateTime = item.CreateTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }


        [HttpPost]
        public CustomJsonResult Add(AddViewModel model)
        {
            var merchant = CurrentDb.Merchant.Where(m => m.UserId == this.CurrentUserId).FirstOrDefault();

            model.Store.UserId = this.CurrentUserId;
            model.Store.MerchantId = merchant.Id;
            return BizFactory.Store.Add(this.CurrentUserId, model.Store);
        }

        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.Store.Edit(this.CurrentUserId, model.Store);
        }

        public CustomJsonResult GetMachineListByBind(SearchCondition condition)
        {

            string name = "";
            if (condition.Name != null)
            {
                name = condition.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Machine
                         where (from d in CurrentDb.StoreMachine
                                where d.StoreId == condition.Id
                                select d.MachineId).Contains(u.Id)

                         where
                                               (name.Length == 0 || u.Name.Contains(name))
                         select new { u.Id, u.Name, u.DeviceId }).Distinct();

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                olist.Add(new
                {
                    item.Id,
                    item.Name,
                    item.DeviceId
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }


        public CustomJsonResult GetMachineListByNoBind(SearchCondition condition)
        {

            string name = "";
            if (condition.Name != null)
            {
                name = condition.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Machine
                         where (from d in CurrentDb.MerchantMachine
                                where d.UserId == this.CurrentUserId
                                select d.MachineId).Contains(u.Id)
                                &&
                                !(from d2 in CurrentDb.StoreMachine
                                  where d2.UserId == this.CurrentUserId &&
                                  d2.Status == Enumeration.StoreMachineStatus.Bind
                                  select d2.MachineId).Contains(u.Id)
                         where
                                               (name.Length == 0 || u.Name.Contains(name))
                         select new { u.Id, u.Name, u.DeviceId }).Distinct();

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                olist.Add(new
                {
                    item.Id,
                    item.Name,
                    item.DeviceId
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        public CustomJsonResult BindMachine(string storeId, string[] machineIds)
        {
            return BizFactory.Store.BindMachine(this.CurrentUserId, storeId, machineIds);
        }
    }
}