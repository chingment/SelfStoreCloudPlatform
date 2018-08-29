using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Newtonsoft.Json;
using Lumos.Common;
using Lumos.Redis;
using NPinyin;
using System.Text;

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
                l_ProductSku.MendTime = this.DateTime;


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

        public void InitSearchCache()
        {
            var tran = RedisManager.Db.CreateTransaction();

            var sysUsers = CurrentDb.SysUser.Where(m => m.Type == Enumeration.UserType.Merchant).ToList();

            foreach (var user in sysUsers)
            {
                var productSkus = CurrentDb.ProductSku.Where(m => m.UserId == user.Id).ToList();

                foreach (var sku in productSkus)
                {
                    Encoding gb2312 = Encoding.GetEncoding("GB2312");
                    string s = Pinyin.ConvertEncoding(sku.Name, Encoding.UTF8, gb2312);
                    string simpleCode = Pinyin.GetInitials(s, gb2312);

                    sku.SimpleCode = simpleCode;
                    CurrentDb.SaveChanges();

                    tran.HashSetAsync("search_productskus_u_" + user.Id, "barcode:" + sku.BarCode + ",name:" + sku.Name + ",simplecode:" + simpleCode, Newtonsoft.Json.JsonConvert.SerializeObject(sku), StackExchange.Redis.When.Always);
                }
            }

            tran.ExecuteAsync();
        }

        public List<ProductSku> Search(string userId, string key)
        {
            List<ProductSku> list = new List<ProductSku>();
            var hs = RedisManager.Db.HashGetAll("search_productskus_u_" + userId);

            var d = (from i in hs select i).Where(x => x.Name.ToString().Contains(key)).Take(10).ToList();

            foreach (var item in d)
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductSku>(item.Value);
                list.Add(obj);
            }
            return list;
        }
    }
}