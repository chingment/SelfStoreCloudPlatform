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
        public APIResponse GetList(int userId, int merchantId, int posMachineId, int pageIndex, int type, int categoryId, int kindId, string name)
        {
            var query = (from o in CurrentDb.ProductSku

                         select new { o.Id, o.Name, o.KindIds, o.KindNames, o.DispalyImgUrls, o.CreateTime }
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

            if (kindId != 0)
            {

                //string strkindId = BizFactory.Product.BuildProductKindIdForSearch(kindId.ToString());

                //query = query.Where(p => SqlFunctions.CharIndex(strkindId, p.ProductKindIds) > 0);
            }

            int pageSize = 10;

            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (pageIndex)).Take(pageSize);

            var list = query.ToList();
            List<ProductSkuModel> model = new List<ProductSkuModel>();


            foreach (var m in list)
            {

                var productModel = new ProductSkuModel();
                productModel.Id = m.Id;
                productModel.Name = m.Name;
                model.Add(productModel);

            }

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }


        [HttpGet]
        public APIResponse GetSkuDetails(string userId, string productSkuId)
        {
            var model = BizFactory.ProductSku.GetModel(productSkuId);

            APIResult result = new APIResult() { Result = ResultType.Success, Code = ResultCode.Success, Message = "", Data = model };

            return new APIResponse(result);
        }

    }
}