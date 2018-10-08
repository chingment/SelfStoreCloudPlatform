using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lumos.Entity;
using Lumos.Mvc;
using Lumos.BLL;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using Lumos;
using WebMerch.Models.Order;

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
        public CustomJsonResult List(SearchCondition condition)
        {
            var query = from o in CurrentDb.Order


                            //join u in CurrentDb.WxUserInfo

                            //on o.ClientId equals u.ClientId into wx
                            //from gci in wx.DefaultIfEmpty()

                        where
                        (condition.Nickname == null || o.ClientName.Contains(condition.Nickname)) &&
                        (condition.OrderStatus == Enumeration.OrderStatus.Unknow || o.Status == condition.OrderStatus) &&
                        (condition.OrderSn == null || o.Sn.Contains(condition.OrderSn))
                        &&
                        o.MerchantId == this.CurrentUserId
                        select new { o.Sn, o.Id, o.ClientId, o.ClientName, o.StoreName, o.Source, o.SubmitTime, o.ChargeAmount, o.DiscountAmount, o.OriginalAmount, o.CreateTime };

            int total = query.GroupBy(p => p.Sn).Select(o => o.FirstOrDefault()).Count();

            int pageIndex = condition.PageIndex;
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

        public ViewResult Details(string id)
        {
            DetailsViewModel model = new DetailsViewModel();
            model.LoadData(id);

            return View(model);
        }
    }
}