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
        public List<ProductParentKindModel> GetKinds(string operater, string userId, string machineId)
        {

            var productKindModels = new List<ProductParentKindModel>();

            var productKinds = CurrentDb.ProductKind.Where(m => m.UserId == userId  && m.Status == Entity.Enumeration.ProductKindStatus.Valid && m.IsDelete == false).ToList();
            var productSkuIds = CurrentDb.MachineStock.Where(m => m.UserId == userId  && m.IsOffSell == false).Select(m => m.ProductSkuId).ToArray();
            var productSkus = CurrentDb.ProductSku.Where(m => productSkuIds.Contains(m.Id)).ToList();

            if (productKinds.Count > 0)
            {
                var productTopKind = productKinds.Where(m => m.PId == GuidUtil.Empty()).FirstOrDefault();

                if (productTopKind != null)
                {
                    var productParentKinds = productKinds.Where(m => m.PId == productTopKind.Id).ToList();

                    foreach (var productParentKind in productParentKinds)
                    {
                        var productParentKindModel = new ProductParentKindModel();
                        productParentKindModel.Id = productParentKind.Id;
                        productParentKindModel.Name = productParentKind.Name;

                        var productChildKinds = productKinds.Where(m => m.PId == productParentKind.Id).ToList();

                        if (productChildKinds.Count > 0)
                        {
                            foreach (var productChildKind in productChildKinds)
                            {
                                var l_productSkuIds = CurrentDb.ProductKindSku.Where(m => m.ProductKindId == productChildKind.Id).Select(m => m.ProductSkuId).ToList();
                                if (l_productSkuIds.Count > 0)
                                {
                                    productParentKindModel.Childs.AddRange(l_productSkuIds);
                                }
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
