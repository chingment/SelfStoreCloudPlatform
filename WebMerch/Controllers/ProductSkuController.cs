using Lumos;
using Lumos.BLL;
using Lumos.Common;
using Lumos.Entity;
using Lumos.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
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
        public CustomJsonResult Add(AddViewModel model)
        {
            var displayimgs = model.DispalyImgs.Where(m => m.ImgUrl != null).ToList();
            model.ProductSku.DispalyImgUrls = Newtonsoft.Json.JsonConvert.SerializeObject(displayimgs);
            return BizFactory.ProductSku.Add(this.CurrentUserId, model.ProductSku);
        }

        [HttpPost]
        public CustomJsonResult Edit(EditViewModel model)
        {
            var displayimgs = model.DispalyImgs.Where(m => m.ImgUrl != null).ToList();
            model.ProductSku.DispalyImgUrls = Newtonsoft.Json.JsonConvert.SerializeObject(displayimgs);
            return BizFactory.ProductSku.Edit(this.CurrentUserId, model.ProductSku);
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
                         select new { u.Id, u.Name, u.CreateTime, u.KindNames, u.SalePrice, u.ShowPrice, u.DispalyImgUrls });

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
                    SalePrice = item.SalePrice.ToF2Price(),
                    ShowPrice = item.ShowPrice.ToF2Price(),
                    CreateTime = item.CreateTime,
                });
            }


            PageEntity pageEntity = new PageEntity { PageSize = pageSize, TotalRecord = total, Rows = olist };

            return Json(ResultType.Success, pageEntity, "");
        }
    }
}