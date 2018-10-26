using Lumos;
using Lumos.BLL;
using Lumos.Entity;
using Lumos.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMerch.Models.ProductKind;

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
            AddViewModel mode = new AddViewModel();
            return View(mode);
        }
        public ViewResult Sort()
        {
            return View();
        }

        public CustomJsonResult GetTreeList(int pId)
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

        public CustomJsonResult GetDetails(string id)
        {
            DetailsViewModel model = new DetailsViewModel(id);
            return Json(ResultType.Success, model, "");
        }


        [HttpPost]
        [OwnNoResubmit]
        public CustomJsonResult Add(AddViewModel model)
        {
            model.ProductKind.MerchantId = this.CurrentUserId;
            return BizFactory.ProductKind.Add(this.CurrentUserId, model.ProductKind);
        }



        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            return BizFactory.ProductKind.Edit(this.CurrentUserId, model.ProductKind);
        }

        [HttpPost]
        public CustomJsonResult Delete(string[] ids)
        {
            return BizFactory.ProductKind.Delete(this.CurrentUserId, ids);
        }

        public CustomJsonResult GetProductSkuList(ProductSearchCondition condition)
        {
            var kinds = BizFactory.ProductKind.GetProductKind(condition.KindId);

            string[] kindIds = kinds.Select(m => m.Id).ToArray();



            string name = condition.Name.ToSearchString();
            var query = (from p in CurrentDb.ProductSku

                         join c in CurrentDb.ProductKindSku on p.Id equals c.ProductSkuId

                         where
(from d in CurrentDb.ProductKindSku
 where (kindIds.Contains(d.ProductKindId) || d.ProductKindId == condition.KindId)
 select d.ProductSkuId).Contains(p.Id)
   && (name.Length == 0 || p.Name.Contains(name))
   &&
   p.MerchantId == this.CurrentUserId
                         select new { c.ProductKindId, c.ProductSkuId, p.Name, p.CreateTime, p.KindNames, p.DispalyImgUrls, p.SalePrice, p.ShowPrice });

            int total = query.Count();

            int pageIndex = condition.PageIndex;
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

        //[HttpPost]
        //public CustomJsonResult Sort(int pId)
        //{

        //    for (int i = 0; i < Request.Form.Count; i++)
        //    {
        //        string name = Request.Form.AllKeys[i].ToString();
        //        if (name.IndexOf("kindId") > -1)
        //        {
        //            int id = int.Parse(name.Split('_')[1].Trim());
        //            int priority = int.Parse(Request.Form[i].Trim());
        //            var model = CurrentDb.ProductKind.Where(m => m.Id == id).FirstOrDefault();
        //            if (model != null)
        //            {
        //                model.Priority = priority;
        //                CurrentDb.SaveChanges();
        //            }
        //        }
        //    }
        //    return Json(ResultType.Success, "保存成功");

        //}
    }
}