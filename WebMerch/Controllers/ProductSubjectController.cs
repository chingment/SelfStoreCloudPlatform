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

        public CustomJsonResult GetTreeList(int pId)
        {
            ProductSubject[] arr;

            arr = CurrentDb.ProductSubject.Where(m => m.MerchantId == this.CurrentUserId && m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();



            object json = ConvertToZTreeJson(arr, "id", "pid", "name", "menu");
            return Json(ResultType.Success, json);

        }

        public CustomJsonResult GetDetails(string subjectId)
        {
            return BizFactory.ProductSubject.GetDetails(this.CurrentUserId, this.CurrentUserId, subjectId);
        }


        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(RopProductSubjectAdd rop)
        {
            return BizFactory.ProductSubject.Add(this.CurrentUserId, this.CurrentUserId, rop);
        }



        [HttpPost]
        public CustomJsonResult Edit(RopProductSubjectEdit rop)
        {
            return BizFactory.ProductSubject.Edit(this.CurrentUserId, this.CurrentUserId, rop);
        }

        [HttpPost]
        public CustomJsonResult Delete(string[] subjectIds)
        {
            return BizFactory.ProductSubject.Delete(this.CurrentUserId, this.CurrentUserId, subjectIds);
        }

        public CustomJsonResult GetProductSkuList(RupProductSkuGetList rup)
        {

            string name = rup.Name.ToSearchString();
            var query = (from p in CurrentDb.ProductSku

                         join c in CurrentDb.ProductSubjectSku on p.Id equals c.ProductSkuId
                         where
(from d in CurrentDb.ProductSubjectSku
 where d.ProductSubjectId == rup.SubjectId
 select d.ProductSkuId).Contains(p.Id)
   && (name.Length == 0 || p.Name.Contains(name))
   && p.MerchantId == this.CurrentUserId
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
        public CustomJsonResult RemoveProductSku(string subjectId, string skuId)
        {
            return BizFactory.ProductSubject.RemoveProductSku(this.CurrentUserId, subjectId, skuId);
        }
    }
}
