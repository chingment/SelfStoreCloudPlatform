using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class ProductSkuService : BaseProvider
    {
        public List<SkuModel> List(string pOperater, string pClientId, RupProductSkuList rup)
        {
            var olist = new List<SkuModel>();

            var query = (from o in CurrentDb.ProductSku

                         select new { o.Id, o.Name, o.KindIds, o.KindNames, o.DispalyImgUrls, o.BriefInfo, o.SalePrice, o.ShowPrice, o.ImgUrl, o.CreateTime }
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


            if (!string.IsNullOrEmpty(rup.KindId))
            {
                query = query.Where(p => (from d in CurrentDb.ProductKindSku
                                          where d.ProductKindId == rup.KindId
                                          select d.ProductSkuId).Contains(p.Id));
            }

            if (!string.IsNullOrEmpty(rup.SubjectId))
            {
                query = query.Where(p => (from d in CurrentDb.ProductSubjectSku
                                          where d.ProductSubjectId == rup.SubjectId
                                          select d.ProductSkuId).Contains(p.Id));
            }

            int pageSize = 10;

            query = query.OrderByDescending(r => r.CreateTime).Skip(pageSize * (rup.PageIndex)).Take(pageSize);

            var list = query.ToList();


            foreach (var item in list)
            {
                olist.Add(new SkuModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ImgUrl = Entity.ImgSet.GetMain(item.DispalyImgUrls),
                    SalePrice = item.SalePrice,
                    ShowPrice = item.ShowPrice,
                    BriefInfo = item.BriefInfo
                });
            }

            return olist;


        }

        public SkuModel Details(string skuId)
        {
            var skuModel = BizFactory.ProductSku.GetModel(skuId);
            return skuModel;
        }
    }
}
