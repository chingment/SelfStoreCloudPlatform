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
        public IndexPageModel GetPageData(string pOperater, string pUserId, string pStoreId)
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

            return pageModel;
        }
    }
}
