using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lumos.Entity;
using Lumos.BLL;
using System.Data;
using WebMerch.Models.Machine;
using Lumos;

namespace WebMerch.Controllers
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
                                 mp.MerchantId == this.CurrentUserId
                                 &&
                                 mp.IsBind == true
                         select new { mp.Id, MachineId = p.Id, mp.MachineName, p.DeviceId, p.MacAddress, p.IsUse, p.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                string storeName = "未绑定便利店";
                var storeMachine = CurrentDb.StoreMachine.Where(m => m.MachineId == item.MachineId && m.IsBind == true).FirstOrDefault();
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
                    item.MachineName,
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

            var query = from u in CurrentDb.StoreSellStock
                        where
                        u.MerchantId == this.CurrentUserId &&
                        u.StoreId == condition.StoreId &&
                        u.ChannelId == condition.MachineId &&
                        (name.Length == 0 || u.ProductSkuName.Contains(name))
                        select new { u.Id, u.SlotId, u.ProductSkuName, u.Quantity, u.LockQuantity, u.SellQuantity, u.SalePrice };

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