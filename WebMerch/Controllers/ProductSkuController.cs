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
    public class ProductSkuController : OwnBaseController
    {
        public ViewResult List()
        {
            return View();
        }
        public ViewResult Add()
        {
            return View();
        }

        public ViewResult Edit()
        {
            return View();
        }

        public ViewResult EditBySalePrice()
        {
            return View();
        }

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.ProductSku.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        [HttpPost]
        public CustomJsonResult Add(RopProducSkuAdd rop)
        {
            return MerchServiceFactory.ProductSku.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopProducSkuEdit rop)
        {
            return MerchServiceFactory.ProductSku.Edit(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(int id)
        {
            //return BizFactory.ProductSku.Delete(this.CurrentUserId, id);

            return null;
        }

        [HttpPost]
        public CustomJsonResult GetList(RupProductSkuGetList rup)
        {
            var query = (from u in CurrentDb.ProductSku
                         where (rup.Name == null || u.Name.Contains(rup.Name))
                         &&
                         u.MerchantId == this.CurrentMerchantId
                         select new { u.Id, u.Name, u.CreateTime, u.KindNames, u.SubjectNames, u.SalePrice, u.ShowPrice, u.DispalyImgUrls });

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
                    MainImg = ImgSet.GetMain(item.DispalyImgUrls),
                    KindNames = item.KindNames,
                    SubjectNames = item.SubjectNames,
                    SalePrice = item.SalePrice.ToF2Price(),
                    ShowPrice = item.ShowPrice.ToF2Price(),
                    CreateTime = item.CreateTime,
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult GetListBySalePrice(RupBaseGetList rup)
        {
            var query = (from u in CurrentDb.StoreSellStock
                         where u.ProductSkuId == rup.Id
                         && u.MerchantId == this.CurrentMerchantId
                         &&
                         (from d in CurrentDb.Machine
                          select d.StoreId).Contains(u.StoreId)
                         select new { u.StoreId, u.ProductSkuId, u.SalePrice }).Distinct();

            int total = query.Count();

            int pageIndex = rup.PageIndex;
            int pageSize = 10;
            query = query.OrderByDescending(r => r.SalePrice).Skip(pageSize * (pageIndex)).Take(pageSize);


            var list = query.ToList();

            List<object> olist = new List<object>();
            foreach (var item in list)
            {
                var store = CurrentDb.Store.Where(m => m.Id == item.StoreId).FirstOrDefault();
                var productSku = CurrentDb.ProductSku.Where(m => m.Id == item.ProductSkuId).FirstOrDefault();
                olist.Add(new
                {
                    ProductSkuId = productSku.Id,
                    ProductSkuName = productSku.Name,
                    ProductSkuImgUrl = ImgSet.GetMain(productSku.DispalyImgUrls),
                    StoreId = store.Id,
                    StoreName = store.Name,
                    ProductSkuSalePrice = item.SalePrice.ToF2Price()
                });
            }

            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }

        [HttpPost]
        public CustomJsonResult EditBySalePrice(RopProductSkuEditSalePrice rop)
        {
            return MerchServiceFactory.ProductSku.EditBySalePrice(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Search(RupProductSkuGetList rup)
        {

            var list = BizFactory.ProductSku.Search(this.CurrentMerchantId, rup.Name);

            List<object> olist = new List<object>();

            foreach (var item in list)
            {
                olist.Add(new
                {
                    Id = item.Id,
                    Name = item.Name,
                    BarCode = item.BarCode,
                    MainImg = ImgSet.GetMain(item.DispalyImgUrls),
                    SalePrice = item.SalePrice.ToF2Price()
                });
            }

            return Json(ResultType.Success, olist, "");
        }

        [HttpPost]
        public CustomJsonResult EditSort(RopProductSubjectEditSort rop)
        {
            return MerchServiceFactory.ProductSubject.EditSort(this.CurrentUserId, this.CurrentMerchantId, rop);
        }
    }
}