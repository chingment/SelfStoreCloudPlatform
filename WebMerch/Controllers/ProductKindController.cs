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

        public CustomJsonResult GetAll(int pId)
        {
            ProductKind[] arr;
            if (pId == 0)
            {
                arr = CurrentDb.ProductKind.Where(m => m.MerchantId == this.CurrentUserId && m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();
            }
            else
            {
                arr = CurrentDb.ProductKind.Where(m => m.MerchantId == this.CurrentUserId && m.PId == "" && m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();
            }

            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);

        }

        public CustomJsonResult GetDetails(string kindId)
        {
            return BizFactory.ProductKind.GetDetails(this.CurrentUserId, this.CurrentUserId, kindId);
        }


        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(RopProductKindAdd rop)
        {
            return BizFactory.ProductKind.Add(this.CurrentUserId, this.CurrentUserId, rop);
        }



        [HttpPost]
        public CustomJsonResult Edit(RopProductKindEdit rop)
        {
            return BizFactory.ProductKind.Edit(this.CurrentUserId, this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string[] kindIds)
        {
            return BizFactory.ProductKind.Delete(this.CurrentUserId, this.CurrentUserId, kindIds);
        }

        public CustomJsonResult GetProductSkuList(RupProductSkuGetList rup)
        {
            var kinds = BizFactory.ProductKind.GetProductKind(rup.KindId);

            string[] kindIds = kinds.Select(m => m.Id).ToArray();



            string name = rup.Name.ToSearchString();
            var query = (from p in CurrentDb.ProductSku

                         join c in CurrentDb.ProductKindSku on p.Id equals c.ProductSkuId

                         where
(from d in CurrentDb.ProductKindSku
 where (kindIds.Contains(d.ProductKindId) || d.ProductKindId == rup.KindId)
 select d.ProductSkuId).Contains(p.Id)
   && (name.Length == 0 || p.Name.Contains(name))
   &&
   p.MerchantId == this.CurrentUserId
                         select new { c.ProductKindId, c.ProductSkuId, p.Name, p.CreateTime, p.KindNames, p.DispalyImgUrls, p.SalePrice, p.ShowPrice });

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderBy(r => r.ProductSkuId).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                olist.Add(new
                {
                    Id = item.ProductSkuId,
                    KindId = item.ProductKindId,
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
        public CustomJsonResult RemoveProductSku(string kindId, string skuId)
        {
            return BizFactory.ProductKind.RemoveProductSku(this.CurrentUserId, kindId, skuId);
        }
    }
}