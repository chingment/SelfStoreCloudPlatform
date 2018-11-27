using Lumos;
using Lumos.BLL;
using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebMerch.Controllers
{
    public class ProductKindController : OwnBaseController
    {
        public ViewResult List()
        {
            return View();
        }

        public ViewResult Add()
        {
            return View();
        }
        public ViewResult Sort()
        {
            return View();
        }

        public CustomJsonResult GetAll()
        {
            var arr = CurrentDb.ProductKind.Where(m => m.MerchantId == this.CurrentMerchantId && m.IsDelete == false).OrderBy(m => m.Priority).ToArray();
            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);

        }

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.ProductKind.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }


        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(RopProductKindAdd rop)
        {
            return MerchServiceFactory.ProductKind.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }



        [HttpPost]
        public CustomJsonResult Edit(RopProductKindEdit rop)
        {
            return MerchServiceFactory.ProductKind.Edit(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string id)
        {
            return MerchServiceFactory.ProductKind.Delete(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        public CustomJsonResult GetProductSkuList(RupProductSkuGetList rup)
        {
            var kinds = MerchServiceFactory.ProductKind.GetSons(this.CurrentMerchantId, rup.KindId);

            string[] kindIds = kinds.Select(m => m.Id).ToArray();

            string[] productSkuIds = (from d in CurrentDb.ProductKindSku
                                      where (kindIds.Contains(d.ProductKindId) || d.ProductKindId == rup.KindId)
                                      select d.ProductSkuId).ToArray<string>();


            string name = rup.Name.ToSearchString();
            var query = (from p in CurrentDb.ProductSku

                         where
   productSkuIds.Contains(p.Id)
   && (name.Length == 0 || p.Name.Contains(name))
   &&
   p.MerchantId == this.CurrentMerchantId
                         select new { p.Id, p.Name, p.CreateTime, p.KindNames, p.DispalyImgUrls, p.SalePrice, p.ShowPrice });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    MainImg = ImgSet.GetMain(item.DispalyImgUrls),
                    KindNames = item.KindNames,
                    SalePrice = item.SalePrice.ToF2Price(),
                    ShowPrice = item.ShowPrice.ToF2Price(),
                    CreateTime = item.CreateTime,
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }


        [HttpPost]
        public CustomJsonResult EditSort(RopProductKindEditSort rop)
        {
            return MerchServiceFactory.ProductKind.EditSort(this.CurrentUserId, this.CurrentMerchantId, rop);
        }
    }
}