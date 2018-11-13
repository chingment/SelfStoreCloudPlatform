using Lumos;
using Lumos.BLL;
using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebMerch.Controllers
{
    public class Order2StockOutController : OwnBaseController
    {
        public ActionResult List()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View();
        }

        public ActionResult Details()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string order2StockOutId)
        {
            return BizFactory.Order2StockOut.GetDetails(this.CurrentUserId, this.CurrentUserId, order2StockOutId);
        }

        [HttpPost]
        public CustomJsonResult GetList(RupOrder2StockOutGetList rup)
        {
            string sn = "";
            if (rup.Sn != null)
            {
                sn = rup.Sn.ToSearchString();
            }

            var query = (from u in CurrentDb.Order2StockOut
                         where (sn.Length == 0 || u.Sn.Contains(sn))
                         &&
                         u.MerchantId == this.CurrentUserId
                         select new { u.Id, u.Sn, u.TargetType, u.TargetName, u.Quantity, u.StockOutTime, u.CreateTime });

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
                    Sn = item.Sn,
                    TargetName = string.Format("[{0}]{1}", item.TargetType.GetCnName(), item.TargetName),
                    Quantity = item.Quantity,
                    StockOutTime = item.StockOutTime.ToUnifiedFormatDate(),
                    CreateTime = item.CreateTime
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult Add(RopOrder2StockOutAdd rop)
        {
            return BizFactory.Order2StockOut.Add(this.CurrentUserId, this.CurrentUserId, rop);
        }
    }
}