using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class ProductKindService : BaseProvider
    {
        public ProductKindPageModel GetPageData(string pOperater, string pUserId, string pStoreId)
        {
            var pageModel = new ProductKindPageModel();

            var productParentKindModels = new List<ProductParentKindModel>();

            var store = CurrentDb.Store.Where(m => m.Id == pStoreId).FirstOrDefault();

            var productKinds = CurrentDb.ProductKind.Where(m => m.UserId == store.UserId && m.Status == Entity.Enumeration.ProductKindStatus.Valid && m.IsDelete == false).ToList();
            var top = productKinds.Where(m => m.PId == GuidUtil.Empty()).FirstOrDefault();
            var productParentKinds = productKinds.Where(m => m.PId == top.Id).ToList();
            foreach (var item in productParentKinds)
            {
                var productParentKindModel = new ProductParentKindModel();
                productParentKindModel.Id = item.Id;
                productParentKindModel.Name = item.Name;
                productParentKindModel.ImgUrl = item.MainImg;
                productParentKindModel.Selected = false;
                var productChildKinds = productKinds.Where(m => m.PId == item.Id).ToList();

                foreach (var item2 in productChildKinds)
                {
                    var productChildKindModel = new ProductChildKindModel();

                    productChildKindModel.Id = item2.Id;
                    productChildKindModel.Name = item2.Name;
                    productChildKindModel.ImgUrl = item2.MainImg;
                    productParentKindModel.Selected = false;

                    productParentKindModel.Child.Add(productChildKindModel);
                }

                productParentKindModels.Add(productParentKindModel);

            }

            var selectedCount = productParentKindModels.Where(m => m.Selected == true).Count();
            if (selectedCount == 0)
            {
                if (productParentKindModels.Count > 0)
                {
                    productParentKindModels[0].Selected = true;
                }
            }

            pageModel.List = productParentKindModels;

            return pageModel;
        }
    }
}
