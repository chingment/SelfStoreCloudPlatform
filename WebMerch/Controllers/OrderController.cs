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
        public ActionResult List(Enumeration.OrderStatus status)
        {
            return View(status);
        }

        [HttpPost]
        public CustomJsonResult List(RupOrderGetList rup)
        {
            var query = from o in CurrentDb.Order


                            //join u in CurrentDb.WxUserInfo

                            //on o.ClientId equals u.ClientId into wx
                            //from gci in wx.DefaultIfEmpty()

                        where
                        (rup.Nickname == null || o.ClientName.Contains(rup.Nickname)) &&
                        (rup.OrderStatus == Enumeration.OrderStatus.Unknow || o.Status == rup.OrderStatus) &&
                        (rup.OrderSn == null || o.Sn.Contains(rup.OrderSn))
                        &&
                        o.MerchantId == this.CurrentMerchantId
                        select new { o.Sn, o.Id, o.ClientId, o.ClientName, o.StoreName, o.Source, o.SubmitTime, o.ChargeAmount, o.DiscountAmount, o.OriginalAmount, o.CreateTime };

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
                    ClientName = item.ClientName,
                    ClientId = item.ClientId,
                    StoreName = item.StoreName,
                    SubmitTime = item.SubmitTime.ToUnifiedFormatDateTime(),
                    ChargeAmount = item.ChargeAmount.ToF2Price(),
                    DiscountAmount = item.DiscountAmount.ToF2Price(),
                    OriginalAmount = item.OriginalAmount.ToF2Price(),
                    CreateTime = item.CreateTime,
                    Source = item.Source
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