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
        public CustomJsonResult Add(string pOperater, ProductSku pProductSku)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                pProductSku.Id = GuidUtil.New();
                pProductSku.Creator = pOperater;
                pProductSku.CreateTime = this.DateTime;

                CurrentDb.ProductSku.Add(pProductSku);

                string[] arr_KindIds = pProductSku.KindIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var kindId in arr_KindIds)
                {
                    var productKindSku = new ProductKindSku();
                    productKindSku.Id = GuidUtil.New();
                    productKindSku.ProductKindId = kindId;
                    productKindSku.ProductSkuId = pProductSku.Id;
                    productKindSku.Creator = pOperater;
                    productKindSku.CreateTime = this.DateTime;
                    CurrentDb.ProductKindSku.Add(productKindSku);
                }

                //CachUtil.Save<ProductSku>(string, ProductSku);
                //CachUtil.GetList<ProductSku>();
                //CachUtil.Get<ProductSku>(string);

                CurrentDb.SaveChanges(true);
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, ProductSku pProductSku)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lProductSku = CurrentDb.ProductSku.Where(m => m.Id == pProductSku.Id).FirstOrDefault();

                lProductSku.Name = pProductSku.Name;
                lProductSku.KindIds = pProductSku.KindIds;
                lProductSku.KindNames = pProductSku.KindNames;
                lProductSku.RecipientModeIds = pProductSku.RecipientModeIds;
                lProductSku.RecipientModeNames = pProductSku.RecipientModeNames;
                lProductSku.ShowPrice = pProductSku.ShowPrice;
                lProductSku.SalePrice = pProductSku.SalePrice;
                lProductSku.BriefInfo = pProductSku.BriefInfo;
                lProductSku.DetailsDes = pProductSku.DetailsDes;
                lProductSku.Mender = pOperater;
                lProductSku.MendTime = this.DateTime;


                var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductSkuId == pProductSku.Id).ToList();

                foreach (var item in productKindSkus)
                {
                    CurrentDb.ProductKindSku.Remove(item);
                }

                string[] arr_KindIds = pProductSku.KindIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var kindId in arr_KindIds)
                {
                    var productKindSku = new ProductKindSku();
                    productKindSku.Id = GuidUtil.New();
                    productKindSku.ProductKindId = kindId;
                    productKindSku.ProductSkuId = pProductSku.Id;
                    productKindSku.Creator = pOperater;
                    productKindSku.CreateTime = this.DateTime;
                    CurrentDb.ProductKindSku.Add(productKindSku);
                }

                CurrentDb.SaveChanges(true);
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }

        public CustomJsonResult EditBySalePrice(string pOperater, string storeId, string productSkuId, decimal productSkuSalePrice)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMachineStocks = CurrentDb.MachineStock.Where(m => m.StoreId == storeId && m.ProductSkuId == productSkuId).ToList();

                foreach (var machineStock in lMachineStocks)
                {
                    machineStock.SalesPrice = productSkuSalePrice;
                    machineStock.Mender = pOperater;
                    machineStock.MendTime = this.DateTime;
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }

        public ProductSku GetModel(string id)
        {
            var model = ProductSkuCacheUtil.GetOne(id);
            if (model == null)
            {
                model = CurrentDb.ProductSku.Where(m => m.Id == id).FirstOrDefault();
                if (model != null)
                {
                    ProductSkuCacheUtil.Add(model);
                }
            }

            return model;
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

        public List<ProductSku> Search(string pUserId, string pKey)
        {
            List<ProductSku> list = new List<ProductSku>();
            var hs = RedisManager.Db.HashGetAll("search_productskus_u_" + pUserId);

            var d = (from i in hs select i).Where(x => x.Name.ToString().Contains(pKey)).Take(10).ToList();

            foreach (var item in d)
            {
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ProductSku>(item.Value);
                list.Add(obj);
            }
            return list;
        }

        public string GetMainImg(string imgSetJson)
        {
            if (string.IsNullOrEmpty(imgSetJson))
                return "";

            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImgSet>>(imgSetJson);

            var main = list.Where(m => m.IsMain == true).FirstOrDefault();
            if (main != null)
                return main.ImgUrl;

            return "";
        }

        public List<ImgSet> GetDispalyImgUrls(string imgSetJson)
        {
            if (string.IsNullOrEmpty(imgSetJson))
                return new List<ImgSet>();

            List<ImgSet> imgs = new List<ImgSet>();
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ImgSet>>(imgSetJson);

            foreach (var m in list)
            {
                if (!string.IsNullOrEmpty(m.ImgUrl))
                {
                    imgs.Add(m);
                }
            }

            return imgs;
        }
    }
}