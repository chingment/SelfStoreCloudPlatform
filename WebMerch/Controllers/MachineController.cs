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
            EditViewModel model = new EditViewModel(id);
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
                         select new { p.Id, mp.Name, p.DeviceId, p.MacAddress, p.IsUse, p.CreateTime });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {

                string storeId = "";
                string storeName = "未绑定便利店";
                var storeMachine = CurrentDb.StoreMachine.Where(m => m.MachineId == item.Id).FirstOrDefault();
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

    }
}