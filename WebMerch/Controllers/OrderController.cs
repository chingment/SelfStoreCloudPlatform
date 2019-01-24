using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Lumos.Entity;
using Lumos;
using Lumos.BLL.Biz;
using Lumos.BLL;
using Lumos.BLL.Service.Merch;

namespace WebMerch.Controllers
{
    public class OrderController : OwnBaseController
    {
        // GET: Order
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public CustomJsonResult List(RupOrderGetList rup)
        {
            var query = from o in CurrentDb.Order


                        where
                        (rup.Nickname == null || o.ClientUserName.Contains(rup.Nickname)) &&
                        (rup.OrderSn == null || o.Sn.Contains(rup.OrderSn))
                        &&
                        o.MerchantId == this.CurrentMerchantId
                        select new { o.Sn, o.Id, o.ClientUserId, o.ClientUserName, o.StoreName, o.Source, o.SubmitTime, o.ChargeAmount, o.DiscountAmount, o.OriginalAmount, o.CreateTime, o.Quantity, o.Status };

            int total = query.GroupBy(p => p.Sn).Select(o => o.FirstOrDefault()).Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.GroupBy(p => new { p.Sn }).Select(o => o.FirstOrDefault()).OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);


            var list = query.ToList();

            List<object> olist = new List<object>();

            foreach (var item in list)
            {

                olist.Add(new
                {
                    Id = item.Id,
                    Sn = item.Sn,
                    ClientUserName = item.ClientUserName,
                    ClientUserId = item.ClientUserId,
                    StoreName = item.StoreName,
                    SubmitTime = item.SubmitTime.ToUnifiedFormatDateTime(),
                    ChargeAmount = item.ChargeAmount.ToF2Price(),
                    DiscountAmount = item.DiscountAmount.ToF2Price(),
                    OriginalAmount = item.OriginalAmount.ToF2Price(),
                    Quantity = item.Quantity,
                    CreateTime = item.CreateTime,
                    StatusName = item.Status.GetCnName(),
                    SourceName = item.Source.GetCnName()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        public ViewResult Details()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.Order.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }
    }
}