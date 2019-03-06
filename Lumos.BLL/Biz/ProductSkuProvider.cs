using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Lumos.Redis;
using NPinyin;
using System.Text;


namespace Lumos.BLL.Biz
{

    public class ProductSkuProvider : BaseProvider
    {
        public SkuModel GetModel(string id)
        {
            SkuModel skuModel = null;

            var sku = ProductSkuCacheUtil.GetOne(id);
            if (sku == null)
            {
                sku = CurrentDb.ProductSku.Where(m => m.Id == id).First();

                ProductSkuCacheUtil.Add(sku);

            }


            skuModel = new SkuModel();
            skuModel.Id = sku.Id;
            skuModel.Name = sku.Name;
            skuModel.SalePrice = sku.SalePrice;
            skuModel.ShowPrice = sku.ShowPrice;
            skuModel.BriefInfo = sku.BriefInfo;
            skuModel.DispalyImgUrls = sku.DispalyImgUrls.ToJsonObject<List<ImgSet>>();
            skuModel.ImgUrl = ImgSet.GetMain(sku.DispalyImgUrls);
            skuModel.DetailsDes = sku.DetailsDes;


            return skuModel;
        }

        public void InitSearchCache()
        {
            var tran = RedisManager.Db.CreateTransaction();

            var merchants = CurrentDb.Merchant.ToList();

            foreach (var merchant in merchants)
            {
                var productSkus = CurrentDb.ProductSku.Where(m => m.MerchantId == merchant.Id).ToList();

                foreach (var sku in productSkus)
                {
                    Encoding gb2312 = Encoding.GetEncoding("GB2312");
                    string s = Pinyin.ConvertEncoding(sku.Name, Encoding.UTF8, gb2312);
                    string simpleCode = Pinyin.GetInitials(s, gb2312);

                    sku.SimpleCode = simpleCode;
                    CurrentDb.SaveChanges();

                    tran.HashSetAsync("search_productskus_u_" + merchant.Id, "barcode:" + sku.BarCode + ",name:" + sku.Name + ",simplecode:" + simpleCode, Newtonsoft.Json.JsonConvert.SerializeObject(sku), StackExchange.Redis.When.Always);
                }
            }

            tran.ExecuteAsync();
        }

        public List<ProductSku> Search(string merchantId, string key)
        {
            List<ProductSku> list = new List<ProductSku>();
            var hs = RedisManager.Db.HashGetAll("search_productskus_u_" + merchantId);

            key = key.ToUpper();

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