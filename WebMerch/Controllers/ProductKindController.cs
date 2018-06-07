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
                arr = CurrentDb.ProductKind.Where(m => m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();
            }
            else
            {
                arr = CurrentDb.ProductKind.Where(m => m.PId == "" && m.IsDelete == false).OrderByDescending(m => m.Priority).ToArray();
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

        //public CustomJsonResult GetProductList(ProductSearchCondition condition)
        //{
        //    string name = condition.Name.ToSearchString();
        //    string kindId = BizFactory.Product.BuildProductKindIdForSearch(condition.KindId);
        //    var list = (from p in CurrentDb.Product
        //                where SqlFunctions.CharIndex(kindId, p.ProductKindIds) > 0 &&
        //                         (name.Length == 0 || p.Name.Contains(name))
        //                select new { p.Id, p.Name, p.MainImg, p.CreateTime, p.Supplier, p.ProductCategory });

        //    int total = list.Count();

        //    int pageIndex = condition.PageIndex;
        //    int pageSize = 10;
        //    list = list.OrderBy(r => r.Id).Skip(pageSize * (pageIndex)).Take(pageSize);

        //    PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = list };

        //    return Json(ResultType.Success, pageEntity);
        //}


        //[HttpPost]
        //public CustomJsonResult RemoveProductFromKind(int kindId, int[] productIds)
        //{
        //    return BizFactory.ProductKind.RemoveProductFromKind(this.CurrentUserId, kindId, productIds);
        //}

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