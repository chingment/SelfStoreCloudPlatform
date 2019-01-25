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

        public ViewResult ParamSet()
        {
            return View();
        }

        public ViewResult Bind()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.Machine.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        public CustomJsonResult GetList(RupMachineGetList rup)
        {

            string machineId = rup.MachineId.ToSearchString();

            var query = (from m in CurrentDb.Machine
                         join s in CurrentDb.Store on m.StoreId equals s.Id into temp
                         from tt in temp.DefaultIfEmpty()
                         where
                                 (machineId.Length == 0 || m.Id.Contains(machineId))
                                 &&
                                 m.MerchantId == this.CurrentMerchantId
                         select new { m.Id, m.Name, m.MacAddress, m.StoreId, m.CreateTime, StoreName = tt.Name });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                bool l_IsBindStore = string.IsNullOrEmpty(item.StoreId) ? false : true;
                string l_StoreName = string.IsNullOrEmpty(item.StoreId) ? "未绑定" : item.StoreName;
                string l_Status = "";
                string l_StatusName = "";
                //todo 未实现状态值

                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    StoreName = l_StoreName,
                    IsBindStore = l_IsBindStore,
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime(),
                    Status = l_Status,
                    StatusName = l_StatusName
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

        [HttpPost]
        public CustomJsonResult Bind(string id, Enumeration.MachineBindType bindType, string storeId)
        {
            if (bindType == Enumeration.MachineBindType.Off)
            {
                return MerchServiceFactory.Machine.BindOffStore(this.CurrentUserId, this.CurrentMerchantId, storeId, id);
            }
            else
            {
                return MerchServiceFactory.Machine.BindOnStore(this.CurrentUserId, this.CurrentMerchantId, storeId, id);
            }
        }
    }
}