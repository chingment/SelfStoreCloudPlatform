using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppMobile
{
    public class IndexService : BaseProvider
    {
        public IndexPageModel GetPageData(string operater, string pClientId, string pStoreId)
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

            var productSubjects = CurrentDb.ProductSubject.Where(m => m.MerchantId == store.MerchantId & m.IsDelete == false).ToList();

            foreach (var productSubject in productSubjects)
            {

                var query = (from o in CurrentDb.ProductSubjectSku where o.ProductSubjectId == productSubject.Id select new { o.Id, o.ProductSkuId, o.ProductSubjectId, o.CreateTime });

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
                        var model = BizFactory.ProductSku.GetModel(i.ProductSkuId);
                        tab.List.Add(model);
                    }

                    pdAreaModel.Tabs.Add(tab);
                }

            }

            var selectedCount = pdAreaModel.Tabs.Where(m => m.Selected == true).Count();
            if (selectedCount == 0)
            {
                if (pdAreaModel.Tabs.Count > 0)
                {
                    pdAreaModel.Tabs[0].Selected = true;
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
