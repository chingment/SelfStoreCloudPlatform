using Lumos;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMerch.Models.ProductSubject;

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
            AddViewModel mode = new AddViewModel();
            return View(mode);
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

        public CustomJsonResult GetDetails(string id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model, "");
        }


        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(AddViewModel model)
        {
            model.ProductSubject.MerchantId = this.CurrentUserId;
            return BizFactory.ProductSubject.Add(this.CurrentUserId, model.ProductSubject);
        }



        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.ProductSubject.Edit(this.CurrentUserId, model.ProductSubject);
        }

        [HttpPost]
        public CustomJsonResult Delete(string[] ids)
        {
            return BizFactory.ProductSubject.Delete(this.CurrentUserId, ids);
        }

        public CustomJsonResult GetProductSkuList(ProductSearchCondition condition)
        {

            string name = condition.Name.ToSearchString();
            var query = (from p in CurrentDb.ProductSku
                         where
(from d in CurrentDb.ProductSubjectSku
 where d.ProductSubjectId == condition.SubjectId
 select d.ProductSkuId).Contains(p.Id)
   && (name.Length == 0 || p.Name.Contains(name))
                         select new { p.Id, p.Name, p.CreateTime, p.KindNames, p.SubjectNames, p.DispalyImgUrls, p.SalePrice, p.ShowPrice });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
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
                    SubjectNames = item.SubjectNames,
                    SalePrice = item.SalePrice.ToF2Price(),
                    ShowPrice = item.ShowPrice.ToF2Price(),
                    CreateTime = item.CreateTime,
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity);
        }
    }
}
