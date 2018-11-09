using Lumos;
using Lumos.BLL;
using Lumos.BLL.Biz;
using Lumos.BLL.Biz.RModels;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMerch.Models.ProductSku;

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
            AddViewModel model = new AddViewModel();
            model.LoadData();

            return View(model);
        }

        public ViewResult Edit(string id)
        {
            EditViewModel model = new EditViewModel();
            model.LoadData(id);

            return View(model);
        }

        public ViewResult EditBySalePrice(string id)
        {
            EditViewModel model = new EditViewModel();
            model.LoadData(id);

            return View(model);
        }

        [HttpPost]
        public CustomJsonResult Add(RopProducSkuAdd rop)
        {
            return BizFactory.ProductSku.Add(this.CurrentUserId, this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Edit(RopProducSkuEdit rop)
        {
            return BizFactory.ProductSku.Edit(this.CurrentUserId, this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(int id)
        {
            //return BizFactory.ProductSku.Delete(this.CurrentUserId, id);

            return null;
        }

        [HttpPost]
        public CustomJsonResult GetList(SearchCondition condition)
        {
            var query = (from u in CurrentDb.ProductSku
                         where (condition.Name == null || u.Name.Contains(condition.Name))
                         &&
                         u.MerchantId == this.CurrentUserId
                         select new { u.Id, u.Name, u.CreateTime, u.KindNames, u.SubjectNames, u.SalePrice, u.ShowPrice, u.DispalyImgUrls });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
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
        public CustomJsonResult GetListBySalePrice(SearchCondition condition)
        {
            var query = (from u in CurrentDb.StoreSellStock
                         where u.ProductSkuId == condition.Id

                         &&
                         (from d in CurrentDb.StoreMachine
                          where d.IsBind == true
                          select d.StoreId).Contains(u.StoreId)

                         select new { u.StoreId, u.ProductSkuId, u.SalePrice }).Distinct();

            int total = query.Count();

            int pageIndex = condition.PageIndex;
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
        public CustomJsonResult EditBySalePrice(string storeId, string skuId, decimal salePrice)
        {
            return BizFactory.ProductSku.EditBySalePrice(this.CurrentUserId, storeId, skuId, salePrice);
        }

        [HttpPost]
        public CustomJsonResult Search(SearchCondition condition)
        {

            var list = BizFactory.ProductSku.Search(this.CurrentUserId, condition.Name);

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
    }
}