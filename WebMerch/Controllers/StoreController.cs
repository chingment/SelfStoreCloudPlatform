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

namespace WebMerch.Controllers
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

        public ViewResult MachineListByBindable()
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
                         && u.MerchantId == this.CurrentUserId
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
            model.Store.MerchantId = this.CurrentUserId;
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
                         join p in CurrentDb.MerchantMachine on u.Id equals p.MachineId
                         where (from d in CurrentDb.StoreMachine
                                where d.StoreId == condition.Id && d.IsBind == true
                                select d.MachineId).Contains(u.Id)
                         &&
                         (name.Length == 0 || u.Name.Contains(name))
                         && p.IsBind == true
                         && p.MerchantId == this.CurrentUserId
                         select new { u.Id, p.MachineName, u.DeviceId }).Distinct();

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
                    item.MachineName,
                    item.DeviceId
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }

        public CustomJsonResult GetMachineListByBindable(SearchCondition condition)
        {

            string name = "";
            if (condition.Name != null)
            {
                name = condition.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Machine
                         join p in CurrentDb.MerchantMachine on u.Id equals p.MachineId
                         where
                                !(from d2 in CurrentDb.StoreMachine
                                  where d2.MerchantId == this.CurrentUserId &&
                                  d2.IsBind == true
                                  select d2.MachineId).Contains(u.Id)
                         &&
                         (name.Length == 0 || u.Name.Contains(name))
                         && p.MerchantId == this.CurrentUserId
                         && p.IsBind == true
                         select new { u.Id, p.MachineName, u.DeviceId }).Distinct();

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
                    item.MachineName,
                    item.DeviceId
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        public CustomJsonResult BindOnMachine(string storeId, string[] machineIds)
        {
            return BizFactory.Store.BindOnMachine(this.CurrentUserId, storeId, machineIds);
        }

        [HttpPost]
        public CustomJsonResult BindOffMachine(string storeId, string machineId)
        {
            return BizFactory.Store.BindOffMachine(this.CurrentUserId, storeId, machineId);
        }
    }
}