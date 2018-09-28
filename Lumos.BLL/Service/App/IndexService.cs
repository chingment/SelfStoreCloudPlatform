using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class IndexService : BaseProvider
    {
        public IndexPageModel GetPageData(string pOperater, string pClientId, string pStoreId)
        {
            var pageModel = new IndexPageModel();

            var store = CurrentDb.Store.Where(m => m.Id == pStoreId).FirstOrDefault();

            var storeModel = new StoreModel();
            storeModel.Id = store.Id;
            storeModel.Name = store.Name;
            storeModel.Address = store.Address;


            pageModel.Store = storeModel;

            var storeBanners = CurrentDb.StoreBanner.Where(m => m.StoreId == pStoreId && m.Type == Enumeration.StoreBannerType.IndexBanner).ToList();

            BannerModel bannerModel = new BannerModel();
            bannerModel.Autoplay = true;
            bannerModel.CurrentSwiper = 0;

            foreach (var item in storeBanners)
            {
                var imgModel = new BannerModel.ImgModel();
                imgModel.Id = item.Id;
                imgModel.Title = item.Title;
                imgModel.Link = "";
                imgModel.Url = item.ImgUrl;
                bannerModel.Imgs.Add(imgModel);
            }

            pageModel.Banner = bannerModel;


            var pdAreaModel = new PdAreaModel();

            var productSubjects = CurrentDb.ProductSubject.Where(m => m.MerchantId == store.MerchantId).ToList();

            foreach (var productSubject in productSubjects)
            {

                var query = (from o in CurrentDb.ProductSubjectSku select new { o.Id, o.ProductSkuId, o.ProductSubjectId, o.CreateTime });

                query = query.OrderByDescending(r => r.CreateTime).Take(6);


                var list = query.ToList();

                if (list.Count > 0)
                {
                    var tab = new PdAreaModel.Tab();
                    tab.Id = productSubject.Id;
                    tab.Name = productSubject.Name;
                    tab.Selected = true;
                    tab.BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner0.png";

                    foreach (var i in list)
                    {
                        var model = BizFactory.ProductSku.GetModel(i.Id);

                        var sku = new SkuModel();

                        sku.Id = model.Id;
                        sku.Name = model.Name;
                        sku.SalePrice = model.SalePrice.ToF2Price();
                        sku.ShowPrice = model.ShowPrice.ToF2Price();
                        sku.DetailsDes = model.DetailsDes;
                        sku.BriefInfo = model.BriefInfo;
                        sku.DispalyImgUrls = BizFactory.ProductSku.GetDispalyImgUrls(model.DispalyImgUrls);

                        tab.List.Add(sku);
                    }
                }

            }

            //pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "热门推荐", Selected = true, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner0.png" });
            // pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "休闲零食", Selected = false, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner1.png" });
            // pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "营养食品", Selected = false, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner2.png" });
            // pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "百货用品", Selected = false, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner3.png" });


            pageModel.PdArea = pdAreaModel;

            return pageModel;
        }
    }
}
