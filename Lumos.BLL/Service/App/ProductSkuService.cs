﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class ProductSkuService : BaseProvider
    {
        public List<SkuModel> List(string operater, string userId, RupProductSkuList rup)
        {
            List<SkuModel> olist = new List<SkuModel>();

            var query = (from o in CurrentDb.ProductSku

                         select new { o.Id, o.Name, o.KindIds, o.KindNames, o.DispalyImgUrls, o.ImgUrl, o.CreateTime, o.SalePrice }
             );

            if (rup.Name != null && rup.Name.Length > 0)
            {
                query = query.Where(p => p.Name.Contains(rup.Name));
            }

            //if (type != Enumeration.ProductType.Unknow)
            //{
            //    //query = query.Where(p => p.ProductCategoryId.ToString().StartsWith(categoryId.ToString()));
            //}

            //if (categoryId != 0)
            //{
            //    query = query.Where(p => p.ProductCategoryId.ToString().StartsWith(categoryId.ToString()));
            //}

            if (string.IsNullOrWhiteSpace(rup.KindId))
            {

                //string strkindId = BizFactory.Product.BuildProductKindIdForSearch(kindId.ToString());

                //query = query.Where(p =>p.Id.c);
            }

            int pageSize = 10;

            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (rup.PageIndex)).Take(pageSize);

            var list = query.ToList();


            foreach (var item in list)
            {
                olist.Add(new SkuModel
                {
                    SkuId = item.Id,
                    SkuName = item.Name,
                    SkuImgUrl = BizFactory.ProductSku.GetMainImg(item.DispalyImgUrls),
                    SalePrice = item.SalePrice.ToF2Price()
                });
            }

            return olist;


        }

        public SkuModel Details(string skuId)
        {
            var model = BizFactory.ProductSku.GetModel(skuId);

            var sku = new SkuModel();

            sku.SkuId = model.Id;
            sku.SkuName = model.Name;
            sku.SalePrice = model.SalePrice.ToF2Price();
            sku.ShowPrice = model.ShowPrice.ToF2Price();
            sku.DetailsDes = model.DetailsDes;
            sku.BriefIntro = model.BriefInfo;
            sku.DispalyImgUrls = BizFactory.ProductSku.GetDispalyImgUrls(model.DispalyImgUrls);

            return sku;
        }
    }
}
