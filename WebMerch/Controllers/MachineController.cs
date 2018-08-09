using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Data;
using System.Transactions;
using WebMerch.Models.Machine;
using Lumos;

namespace WebMerch.Controllers.Biz
{
    public class MachineController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Edit(string id)
        {
            EditViewModel model = new EditViewModel();
            model.LoadData(id);
            return View(model);
        }

        public CustomJsonResult GetList(SearchCondition condition)
        {
            string deviceId = condition.DeviceId.ToSearchString();

            var query = (from mp in CurrentDb.MerchantMachine
                         join p in CurrentDb.Machine on mp.MachineId equals p.Id
                         where
                                 (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))
                                 &&
                                 mp.UserId == this.CurrentUserId
                                 &&
                                 mp.Status == Enumeration.MerchantMachineStatus.Bind
                         select new { mp.Id, MachineId = p.Id, mp.Name, p.DeviceId, p.MacAddress, p.IsUse, p.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                string storeName = "未绑定便利店";
                var storeMachine = CurrentDb.StoreMachine.Where(m => m.MachineId == item.MachineId && m.Status == Enumeration.StoreMachineStatus.Bind).FirstOrDefault();
                if (storeMachine != null)
                {
                    var store = CurrentDb.Store.Where(m => m.Id == storeMachine.StoreId).FirstOrDefault();
                    if (store != null)
                    {
                        storeName = store.Name;
                    }
                }

                olist.Add(new
                {
                    item.Id,
                    item.Name,
                    item.DeviceId,
                    storeName,
                    item.CreateTime
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.MerchantMachine.Edit(this.CurrentUserId, model.MerchantMachine);
        }

        public CustomJsonResult GetSkuList(Models.MachineStock.SearchCondition condition)
        {

            string name = "";
            if (condition.Name != null)
            {
                name = condition.Name.ToSearchString();
            }

            var query = from u in CurrentDb.MachineStock
                        where
                        u.UserId == this.CurrentUserId &&
                        u.StoreId == condition.StoreId &&
                        u.MerchantId == condition.MerchantId &&
                        u.MachineId == condition.MerchantId &&
                        (name.Length == 0 || u.ProductSkuName.Contains(name))
                        select new { u.Id, u.SlotId, u.ProductSkuName, u.Quantity, u.LockQuantity, u.SellQuantity, u.SalesPrice };

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.Id);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                olist.Add(new
                {
                    item.Id,
                    item.SlotId,
                    item.ProductSkuName
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }
    }
}