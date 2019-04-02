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

        public ViewResult Stock()
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

        public ViewResult Banner()
        {
            return View();
        }

        public ViewResult BannerAdd()
        {
            return View();
        }

        public CustomJsonResult GetStock(string id)
        {
            return MerchServiceFactory.Machine.GetStock(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        public CustomJsonResult GetList(RupMachineGetList rup)
        {

            string machineId = rup.MachineId.ToSearchString();

            var query = (from m in CurrentDb.Machine
                         join s in CurrentDb.Store on m.StoreId equals s.Id into temp
                         from tt in temp.DefaultIfEmpty()
                         where
                               
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

        public CustomJsonResult GetBannerList(RupMachineBannerGetList rup)
        {
            var machine = CurrentDb.Machine.Where(m => m.Id == rup.MachineId).FirstOrDefault();

            var query = (from m in CurrentDb.MachineBanner
                         where
                                 m.MerchantId == this.CurrentMerchantId &&
                                 m.StoreId == machine.StoreId &&
                                 m.MachineId == machine.Id
                         select new { m.Id, m.Title, m.ImgUrl, m.Priority, m.Status, m.CreateTime });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(m => m.Status).OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                olist.Add(new
                {
                    Id = item.Id,
                    Title = item.Title,
                    ImgUrl = item.ImgUrl,
                    Status = item.Status,
                    StatusName = item.Status.GetCnName(),
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }
        [HttpPost]
        public CustomJsonResult AddBanner(RopMachineBannerAdd rop)
        {
            return MerchServiceFactory.MachineBanner.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult DeleteBanner(string id)
        {
            return MerchServiceFactory.MachineBanner.Delete(this.CurrentUserId, this.CurrentMerchantId, id);
        }

    }
}