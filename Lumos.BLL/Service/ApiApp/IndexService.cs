using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class IndexService : BaseProvider
    {
        public IndexPageModel GetPageData(string operater, string clientId, string storeId)
        {
            var pageModel = new IndexPageModel();

            var store = CurrentDb.Store.Where(m => m.Id == storeId).FirstOrDefault();

            var storeModel = new StoreModel();
            storeModel.Id = store.Id;
            storeModel.Name = store.Name;
            storeModel.Address = store.Address;


            pageModel.Store = storeModel;

            var storeBanners = CurrentDb.AdRelease.Where(m => m.AdSpaceId== Enumeration.AdSpaceId.AppHomeTop).ToList();

            BannerModel bannerModel = new BannerModel();
            bannerModel.Autoplay = true;
            bannerModel.CurrentSwiper = 0;

            foreach (var item in storeBanners)
            {
                var imgModel = new BannerModel.ImgModel();
                imgModel.Id = item.Id;
                imgModel.Title = item.Title;
                imgModel.Link = "";
                imgModel.Url = item.Url;
                bannerModel.Imgs.Add(imgModel);
            }

            pageModel.Banner = bannerModel;


            var pdAreaModel = new PdAreaModel();

            var productSubjects = CurrentDb.ProductSubject.Where(m => m.MerchantId == store.MerchantId & m.IsDelete == false && m.Dept == 1).OrderBy(m => m.Priority).ToList();

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
                    tab.ImgUrl = productSubject.MainImg;

                    foreach (var i in list)
                    {
                        var model = BizFactory.ProductSku.GetModel(i.ProductSkuId);
                        tab.List.Add(model);
                    }

                    pdAreaModel.Tabs.Add(tab);
                }

            }

            pageModel.PdArea = pdAreaModel;

            return pageModel;
        }
    }
}
