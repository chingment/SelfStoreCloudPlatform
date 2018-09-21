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
        public IndexModel GetData(string pOperater,string pUserId, string pStoreId)
        {
            var model = new IndexModel();


            var banner = CurrentDb.StoreBanner.Where(m => m.StoreId == pStoreId).ToList();

            List<BannerModel> bannerModels = new List<BannerModel>();

            foreach (var m in banner)
            {
                var bannerModel = new BannerModel();
                bannerModel.Id = m.Id;
                bannerModel.Title = m.Title;
                bannerModel.LinkUrl = "";
                bannerModel.ImgUrl = m.ImgUrl;
                bannerModels.Add(bannerModel);
            }

            model.Banner = bannerModels;


            return model;
        }
    }
}
