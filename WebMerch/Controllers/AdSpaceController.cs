using Lumos;
using Lumos.BLL.Service.Merch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMerch.Controllers
{
    public class AdSpaceController : OwnBaseController
    {
        public ActionResult List()
        {
            return View();
        }

        public ViewResult ListByRelease()
        {
            return View();
        }

        public ViewResult AddRelease()
        {
            return View();
        }

        public CustomJsonResult GetList(RupAdSpaceGetList rup)
        {

            var query = (from m in CurrentDb.AdSpace
                         where

                                m.Belong == rup.Belong
                         select new { m.Id, m.Name, m.Belong, m.Type, m.CreateTime, m.Status, m.Description });

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
                    Name = item.Name,
                    BelongName = item.Belong.GetCnName(),
                    TypeName = item.Type.GetCnName(),
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime(),
                    StatusName = item.Status.GetCnName(),
                    Description = item.Description
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }




        public CustomJsonResult GetListByRelease(RupAdSpaceGetListByRelease rup)
        {


            var query = (from m in CurrentDb.AdRelease
                         where
                                 m.MerchantId == this.CurrentMerchantId &&
                                 m.AdSpaceId == rup.AdSpaceId
                         select new { m.Id, m.Title, m.Url, m.Priority, m.Status, m.CreateTime });

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
                    Url = item.Url,
                    Status = item.Status,
                    StatusName = item.Status.GetCnName(),
                    CreateTime = item.CreateTime.ToUnifiedFormatDateTime()
                });

            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }
        [HttpPost]
        public CustomJsonResult AddAdRelease(RopAdReleaseAdd rop)
        {
            return MerchServiceFactory.AdRelease.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult DeleteRelease(string id)
        {
            return MerchServiceFactory.AdRelease.Delete(this.CurrentUserId, this.CurrentMerchantId, id);
        }
    }
}