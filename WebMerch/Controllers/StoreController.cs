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
    public class StoreController : OwnBaseController
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

        public ViewResult MachineListByBindable()
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
                         && u.MerchantId == this.CurrentUserId
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

        public CustomJsonResult GetDetails(string storeId)
        {
            return MerchServiceFactory.Store.GetDetails(this.CurrentUserId, this.CurrentUserId, storeId);
        }

        [HttpPost]
        public CustomJsonResult Add(RopStoreAdd rop)
        {
            return MerchServiceFactory.Store.Add(this.CurrentUserId, this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopStoreEdit rop)
        {
            return MerchServiceFactory.Store.Edit(this.CurrentUserId, this.CurrentUserId, rop);
        }

        public CustomJsonResult GetMachineListByBind(RupMachineGetList rup)
        {

            string name = "";
            if (rup.Name != null)
            {
                name = rup.Name.ToSearchString();
            }

            var query = (from u in CurrentDb.Machine
                         join p in CurrentDb.StoreMachine on u.Id equals p.MachineId
                         where
                         (name.Length == 0 || u.Name.Contains(name))
                         && p.IsBind == true
                         && p.MerchantId == this.CurrentUserId
                         select new { u.Id, Name = p.MachineName, u.DeviceId }).Distinct();

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
                    item.Name,
                    item.DeviceId
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }

        public CustomJsonResult GetMachineListByBindable(RupMachineGetList rup)
        {

            string name = "";
            if (rup.Name != null)
            {
                name = rup.Name.ToSearchString();
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
                         select new { u.Id, u.Name, u.DeviceId }).Distinct();

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
                    item.Name,
                    item.DeviceId
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        public CustomJsonResult BindOnMachine(string storeId, string[] machineIds)
        {
            return MerchServiceFactory.Store.BindOnMachine(this.CurrentUserId, storeId, machineIds);
        }

        [HttpPost]
        public CustomJsonResult BindOffMachine(string storeId, string machineId)
        {
            return MerchServiceFactory.Store.BindOffMachine(this.CurrentUserId, storeId, machineId);
        }
    }
}