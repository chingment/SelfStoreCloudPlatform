using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lumos.Entity;
using Lumos.BLL;
using System.Data;
using Lumos;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;

namespace WebMerch.Controllers
{
    public class MachineController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Edit()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string merchantMachineId)
        {
            return MerchServiceFactory.MerchantMachine.GetDetails(this.CurrentUserId, this.CurrentUserId, merchantMachineId);
        }

        public CustomJsonResult GetList(RupMachineGetList rup)
        {
            string deviceId = rup.DeviceId.ToSearchString();

            var query = (from mp in CurrentDb.MerchantMachine
                         join p in CurrentDb.Machine on mp.MachineId equals p.Id
                         where
                                 (deviceId.Length == 0 || p.DeviceId.Contains(deviceId))
                                 &&
                                 mp.MerchantId == this.CurrentUserId
                                 &&
                                 mp.IsBind == true
                         select new { mp.Id, MachineId = p.Id, MachineName = p.Name, p.DeviceId, p.MacAddress, p.IsUse, p.CreateTime });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                string machineName = item.MachineName;
                string storeName = "未绑定便利店";
                var storeMachine = CurrentDb.StoreMachine.Where(m => m.MachineId == item.MachineId && m.IsBind == true).FirstOrDefault();
                if (storeMachine != null)
                {
                    machineName = storeMachine.MachineName;

                    var store = CurrentDb.Store.Where(m => m.Id == storeMachine.StoreId).FirstOrDefault();
                    if (store != null)
                    {
                        storeName = store.Name;
                    }
                }

                olist.Add(new
                {
                    item.Id,
                    machineName,
                    item.DeviceId,
                    storeName,
                    item.CreateTime
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Edit(RopMerchantMachineEdit rop)
        {
            return MerchServiceFactory.MerchantMachine.Edit(this.CurrentUserId, this.CurrentUserId, rop);
        }
    }
}