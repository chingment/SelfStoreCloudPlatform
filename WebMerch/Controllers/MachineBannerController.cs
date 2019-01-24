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
    public class MachineBannerController : OwnBaseController
    {

        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }

        public CustomJsonResult GetList(RupMachineBannerGetList rup)
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
            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

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
        public CustomJsonResult Add(RopMachineBannerAdd rop)
        {
            return MerchServiceFactory.MachineBanner.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string id)
        {
            return MerchServiceFactory.MachineBanner.Delete(this.CurrentUserId, this.CurrentMerchantId, id);
        }
    }
}