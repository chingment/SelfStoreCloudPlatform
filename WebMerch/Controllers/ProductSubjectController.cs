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
    public class ProductSubjectController : OwnBaseController
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
            var arr = CurrentDb.ProductSubject.Where(m => m.MerchantId == this.CurrentMerchantId && m.IsDelete == false).OrderBy(m => m.Priority).ToArray();
            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);

        }

        public CustomJsonResult GetDetails(string id)
        {
            return MerchServiceFactory.ProductSubject.GetDetails(this.CurrentUserId, this.CurrentMerchantId, id);
        }


        [HttpPost]
        public CustomJsonResult Add(RopProductSubjectAdd rop)
        {
            return MerchServiceFactory.ProductSubject.Add(this.CurrentUserId, this.CurrentMerchantId, rop);
        }



        [HttpPost]
        public CustomJsonResult Edit(RopProductSubjectEdit rop)
        {
            return MerchServiceFactory.ProductSubject.Edit(this.CurrentUserId, this.CurrentMerchantId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string id)
        {
            return MerchServiceFactory.ProductSubject.Delete(this.CurrentUserId, this.CurrentMerchantId, id);
        }

        public CustomJsonResult GetProductSkuList(RupProductSkuGetList rup)
        {

            string name = rup.Name.ToSearchString();
            var query = (from p in CurrentDb.ProductSku

                         join c in CurrentDb.ProductSubjectSku on p.Id equals c.ProductSkuId
                         where
                         c.ProductSubjectId == rup.SubjectId &&
   (name.Length == 0 || p.Name.Contains(name))
   && p.MerchantId == this.CurrentMerchantId
                         select new { p.Id, c.ProductSubjectId, p.Name, p.CreateTime, p.KindNames, p.SubjectNames, p.DispalyImgUrls, p.SalePrice, p.ShowPrice });

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
                    SubjectId = item.ProductSubjectId,
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

            return Json(ResultType.Success, pageEntity);
        }

        [HttpPost]
        public CustomJsonResult EditSort(RopProductSubjectEditSort rop)
        {
            return MerchServiceFactory.ProductSubject.EditSort(this.CurrentUserId, this.CurrentMerchantId, rop);
        }
    }
}
