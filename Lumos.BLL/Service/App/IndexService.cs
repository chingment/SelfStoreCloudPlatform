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


            pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "热门推荐", Selected = true, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner0.png" });
            pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "休闲零食", Selected = false, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner1.png" });
            pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "营养食品", Selected = false, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner2.png" });
            pdAreaModel.Tabs.Add(new PdAreaModel.Tab { Name = "百货用品", Selected = false, BannerImgUrl = "https://demo.res.17fanju.com/Images/Resource/banner3.png" });


            pageModel.PdArea = pdAreaModel;

            return pageModel;
        }
    }
}
