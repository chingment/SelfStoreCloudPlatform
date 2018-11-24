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

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.Machine.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        public CustomJsonResult GetList(RupMachineGetList rup)
        {
            string deviceId = rup.DeviceId.ToSearchString();

            var query = (from m in CurrentDb.Machine
                         join s in CurrentDb.Store on m.StoreId equals s.Id into temp
                         from tt in temp.DefaultIfEmpty()
                         where
                                 (deviceId.Length == 0 || m.DeviceId.Contains(deviceId))
                                 &&
                                 m.MerchantId == this.CurrentMerchantId
                                 &&
                                 m.IsUse == true
                         select new { m.Id, m.Name, m.DeviceId, m.MacAddress, m.StoreId, m.CreateTime });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                string machineName = item.Name;
                string storeName = "未绑定便利店";

                ///string l_merchantName = item. == false ? "未绑定商户" : item.MerchantName;

                //todo 未实现状态值

                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    DeviceId = item.DeviceId,
                    storeName = storeName,
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime(),
                    Status = "",
                    StatusName = ""
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Edit(RopMachineEdit rop)
        {
            return MerchServiceFactory.Machine.Edit(this.CurrentUserId, this.CurrentMerchantId, rop);
        }
    }
}