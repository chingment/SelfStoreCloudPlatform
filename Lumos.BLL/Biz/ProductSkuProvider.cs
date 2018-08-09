using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Newtonsoft.Json;
using Lumos.Common;

namespace Lumos.BLL
{

    public class ProductSkuProvider : BaseProvider
    {
        public CustomJsonResult Add(string operater, ProductSku productSku)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                productSku.Id = GuidUtil.New();
                productSku.Creator = operater;
                productSku.CreateTime = this.DateTime;

                CurrentDb.ProductSku.Add(productSku);
                CurrentDb.SaveChanges();

                string[] arr_KindIds = productSku.KindIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var kindId in arr_KindIds)
                {
                    var productKindSku = new ProductKindSku();
                    productKindSku.Id = GuidUtil.New();
                    productKindSku.ProductKindId = kindId;
                    productKindSku.ProductSkuId = productSku.Id;
                    productKindSku.Creator = operater;
                    productKindSku.CreateTime = this.DateTime;
                    CurrentDb.ProductKindSku.Add(productKindSku);
                    CurrentDb.SaveChanges();
                }


                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string operater, ProductSku productSku)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var l_ProductSku = CurrentDb.ProductSku.Where(m => m.Id == productSku.Id).FirstOrDefault();

                l_ProductSku.Name = productSku.Name;
                l_ProductSku.KindIds = productSku.KindIds;
                l_ProductSku.KindNames = productSku.KindNames;
                l_ProductSku.RecipientModeIds = productSku.RecipientModeIds;
                l_ProductSku.RecipientModeNames = productSku.RecipientModeNames;
                l_ProductSku.ShowPrice = productSku.ShowPrice;
                l_ProductSku.SalePrice = productSku.SalePrice;
                l_ProductSku.BriefInfo = productSku.BriefInfo;
                l_ProductSku.DetailsDes = productSku.DetailsDes;
                l_ProductSku.Mender = operater;
                l_ProductSku.LastUpdateTime = this.DateTime;


                var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductSkuId == productSku.Id).ToList();

                foreach (var item in productKindSkus)
                {
                    CurrentDb.ProductKindSku.Remove(item);
                    CurrentDb.SaveChanges();
                }

                string[] arr_KindIds = productSku.KindIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var kindId in arr_KindIds)
                {
                    var productKindSku = new ProductKindSku();
                    productKindSku.Id = GuidUtil.New();
                    productKindSku.ProductKindId = kindId;
                    productKindSku.ProductSkuId = productSku.Id;
                    productKindSku.Creator = operater;
                    productKindSku.CreateTime = this.DateTime;
                    CurrentDb.ProductKindSku.Add(productKindSku);
                    CurrentDb.SaveChanges();
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }
    }
}
