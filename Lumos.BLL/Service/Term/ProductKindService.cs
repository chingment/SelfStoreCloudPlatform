using Lumos.BLL.Service.Term.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class ProductKindService : BaseProvider
    {
        public List<ProductKindModel> GetKinds(string operater, string userId, string merchantId)
        {

            var productKindModels = new List<ProductKindModel>();

            var productKinds = CurrentDb.ProductKind.Where(m => m.UserId == userId && m.MerchantId == merchantId && m.Status == Entity.Enumeration.ProductKindStatus.Valid && m.IsDelete == false).ToList();

            if (productKinds.Count > 0)
            {

                var productParentKind = productKinds.Where(m => m.PId == GuidUtil.Empty()).FirstOrDefault();

                if (productParentKind != null)
                {
                    var productParentKinds = productKinds.Where(m => m.PId == productParentKind.Id).ToList();

                    foreach (var item in productParentKinds)
                    {
                        var productParentKindModel = new ProductKindModel();
                        productParentKindModel.Id = item.Id;
                        productParentKindModel.Name = item.Name;

                        productParentKindModel.Banners.Add(new BannerModel() { ImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/banner1.png" });
                        productParentKindModel.Banners.Add(new BannerModel() { ImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/banner2.png" });

                        var productChildKinds = productKinds.Where(m => m.PId == item.Id).ToList();

                        if (productChildKinds.Count > 0)
                        {

                            foreach (var item2 in productChildKinds)
                            {
                                var productChildKindModel = new ProductChildKindModel();

                                productChildKindModel.Id = item2.Id;
                                productChildKindModel.Name = item2.Name;
                                productParentKindModel.Childs.Add(productChildKindModel);
                            }
                        }

                        productKindModels.Add(productParentKindModel);

                    }
                }
            }

            return productKindModels;
        }
    }
}
