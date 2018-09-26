using Lumos.BLL.Service.Term.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class ProductSkuProvider : BaseProvider
    {
        public List<ProductSkuModel> GetSkus(string pMerchantId)
        {
            var productSkuModels = new List<ProductSkuModel>();



            var productSkus = CurrentDb.ProductSku.Where(m => m.MerchantId == pMerchantId).ToList();





            return productSkuModels;
        }
    }
}
