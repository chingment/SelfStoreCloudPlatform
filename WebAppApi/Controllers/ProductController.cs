using Lumos;
using Lumos.BLL;
using Lumos.BLL.Service;
using Lumos.BLL.Service.App;
using Lumos.DAL;
using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAppApi.Models;
using WebAppApi.Models.Account;

namespace WebAppApi.Controllers
{
    [BaseAuthorizeAttribute]
    public class ProductController : OwnBaseApiController
    {

        [HttpGet]
        public APIResponse GetList(string storeId, int pageIndex, string kindId, string name)
        {

            var query = (from o in CurrentDb.ProductSku

                         select new { o.Id, o.Name, o.KindIds, o.KindNames, o.DispalyImgUrls, o.ImgUrl, o.CreateTime, o.SalePrice }
                         );

            if (name != null && name.Length > 0)
            {
                query = query.Where(p => p.Name.Contains(name));
            }

            //if (type != Enumeration.ProductType.Unknow)
            //{
            //    //query = query.Where(p => p.ProductCategoryId.ToString().StartsWith(categoryId.ToString()));
            //}

            //if (categoryId != 0)
            //{
            //    query = query.Where(p => p.ProductCategoryId.ToString().StartsWith(categoryId.ToString()));
            //}

            if (string.IsNullOrWhiteSpace(kindId))
            {

                //string strkindId = BizFactory.Product.BuildProductKindIdForSearch(kindId.ToString());

                //query = query.Where(p =>p.Id.c);
            }

            int pageSize = 10;

            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();

            List<object> olist = new List<object>();


            foreach (var item in list)
            {
                olist.Add(new
                {
                    SkuId = item.Id,
                    Name = item.Name,
                    ImgUrl = BizFactory.ProductSku.GetMainImg(item.DispalyImgUrls),
                    SalePrice = item.SalePrice.ToF2Price()
                });
            }

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = olist };

            return new APIResponse(result);
        }



        [HttpGet]
        public APIResponse GetSkuDetails(string userId, string productSkuId)
        {
            var model = BizFactory.ProductSku.GetModel(productSkuId);

            var sku = new ProductSkuDetailsModel();

            sku.Id = model.Id;
            sku.Name = model.Name;
            sku.SalePrice = model.SalePrice.ToF2Price();
            sku.ShowPrice = model.ShowPrice.ToF2Price();
            sku.DetailsDes = model.DetailsDes;
            sku.BriefIntro = model.BriefInfo;
            sku.DispalyImgUrls = BizFactory.ProductSku.GetDispalyImgUrls(model.DispalyImgUrls);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = sku };

            return new APIResponse(result);
        }

    }
}