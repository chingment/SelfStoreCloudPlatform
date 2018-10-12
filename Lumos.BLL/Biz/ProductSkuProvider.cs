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
using Lumos.BLL.Biz.RModels;
namespace Lumos.BLL
{

    public class ProductSkuProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, string pMerchantId, RopProducSkuAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();


            if (string.IsNullOrEmpty(rop.Name))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "商品名称不能为空");
            }

            if (rop.KindIds == null || rop.KindIds.Count == 0)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "商品模块分类不能为空");
            }

            if (rop.RecipientModeIds == null || rop.RecipientModeIds.Count == 0)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "取货模式不能为空");
            }

            if (rop.DispalyImgUrls == null || rop.DispalyImgUrls.Count == 0)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "商品图片不能为空");
            }

            using (TransactionScope ts = new TransactionScope())
            {

                var productSku = new ProductSku();
                productSku.Id = GuidUtil.New();
                productSku.MerchantId = pMerchantId;
                productSku.Name = rop.Name;
                productSku.BarCode = rop.BarCode;
                Encoding gb2312 = Encoding.GetEncoding("GB2312");
                string s = Pinyin.ConvertEncoding(productSku.Name, Encoding.UTF8, gb2312);
                string simpleCode = Pinyin.GetInitials(s, gb2312);
                productSku.SimpleCode = simpleCode;
                productSku.DispalyImgUrls = rop.DispalyImgUrls.ToJsonString();
                productSku.ImgUrl = ImgSet.GetMain(productSku.DispalyImgUrls);
                productSku.ShowPrice = rop.ShowPrice;
                productSku.SalePrice = rop.SalePrice;
                productSku.DetailsDes = rop.DetailsDes;
                productSku.SpecDes = rop.SpecDes;
                productSku.BriefInfo = rop.BriefInfo;
                productSku.Creator = pOperater;
                productSku.CreateTime = this.DateTime;


                var recipientModes = BizFactory.RecipientMode.GetList().Where(m => rop.RecipientModeIds.Contains(m.Id)).ToList();

                productSku.RecipientModeIds = string.Join(",", recipientModes.Select(m => m.Id).ToArray());
                productSku.RecipientModeNames = string.Join(",", recipientModes.Select(m => m.Name).ToArray());


                var productKinds = CurrentDb.ProductKind.Where(m => rop.KindIds.Contains(m.Id)).ToList();

                productSku.KindIds = string.Join(",", productKinds.Select(m => m.Id).ToArray());
                productSku.KindNames = string.Join(",", productKinds.Select(m => m.Name).ToArray());

                foreach (var productKind in productKinds)
                {
                    var productKindSku = new ProductKindSku();
                    productKindSku.Id = GuidUtil.New();
                    productKindSku.ProductKindId = productKind.Id;
                    productKindSku.ProductSkuId = productSku.Id;
                    productKindSku.Creator = pOperater;
                    productKindSku.CreateTime = this.DateTime;
                    CurrentDb.ProductKindSku.Add(productKindSku);
                }


                var productSubjects = CurrentDb.ProductSubject.Where(m => rop.SubjectIds.Contains(m.Id)).ToList();

                productSku.SubjectIds = string.Join(",", productSubjects.Select(m => m.Id).ToArray());
                productSku.SubjectNames = string.Join(",", productSubjects.Select(m => m.Name).ToArray());

                foreach (var productSubject in productSubjects)
                {
                    var productSubjectSku = new ProductSubjectSku();
                    productSubjectSku.Id = GuidUtil.New();
                    productSubjectSku.ProductSubjectId = productSubject.Id;
                    productSubjectSku.ProductSkuId = productSku.Id;
                    productSubjectSku.Creator = pOperater;
                    productSubjectSku.CreateTime = this.DateTime;
                    CurrentDb.ProductSubjectSku.Add(productSubjectSku);
                }

                CurrentDb.ProductSku.Add(productSku);
                CurrentDb.SaveChanges(true);
                ts.Complete();


                var ret = new RetProducSkuAdd();

                ret.Id = productSku.Id;

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功", ret);
            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, string pMerchantId, RopProducSkuEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            if (string.IsNullOrEmpty(rop.Id))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "商品Id不能为空");
            }

            if (string.IsNullOrEmpty(rop.Name))
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "商品名称不能为空");
            }

            if (rop.KindIds == null || rop.KindIds.Count == 0)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "商品模块分类不能为空");
            }

            if (rop.RecipientModeIds == null || rop.RecipientModeIds.Count == 0)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "取货模式不能为空");
            }

            if (rop.DispalyImgUrls == null || rop.DispalyImgUrls.Count == 0)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "商品图片不能为空");
            }

            using (TransactionScope ts = new TransactionScope())
            {
                var productSku = CurrentDb.ProductSku.Where(m => m.Id == rop.Id).FirstOrDefault();

                productSku.Name = rop.Name;

                Encoding gb2312 = Encoding.GetEncoding("GB2312");
                string s = Pinyin.ConvertEncoding(productSku.Name, Encoding.UTF8, gb2312);
                string simpleCode = Pinyin.GetInitials(s, gb2312);
                productSku.SimpleCode = simpleCode;
                productSku.ShowPrice = rop.ShowPrice;
                productSku.SalePrice = rop.SalePrice;
                productSku.BriefInfo = rop.BriefInfo;
                productSku.DetailsDes = rop.DetailsDes;
                productSku.Mender = pOperater;
                productSku.MendTime = this.DateTime;

                var recipientModes = BizFactory.RecipientMode.GetList().Where(m => rop.RecipientModeIds.Contains(m.Id)).ToList();

                productSku.RecipientModeIds = string.Join(",", recipientModes.Select(m => m.Id).ToArray());
                productSku.RecipientModeNames = string.Join(",", recipientModes.Select(m => m.Name).ToArray());

                var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductSkuId == rop.Id).ToList();

                foreach (var item in productKindSkus)
                {
                    CurrentDb.ProductKindSku.Remove(item);
                }

                var productKinds = CurrentDb.ProductKind.Where(m => rop.KindIds.Contains(m.Id)).ToList();

                productSku.KindIds = string.Join(",", productKinds.Select(m => m.Id).ToArray());
                productSku.KindNames = string.Join(",", productKinds.Select(m => m.Name).ToArray());

                foreach (var productKind in productKinds)
                {
                    var productKindSku = new ProductKindSku();
                    productKindSku.Id = GuidUtil.New();
                    productKindSku.ProductKindId = productKind.Id;
                    productKindSku.ProductSkuId = productSku.Id;
                    productKindSku.Creator = pOperater;
                    productKindSku.CreateTime = this.DateTime;
                    CurrentDb.ProductKindSku.Add(productKindSku);
                }

                var productSubjectSkus = CurrentDb.ProductSubjectSku.Where(m => m.ProductSkuId == productSku.Id).ToList();

                foreach (var item in productSubjectSkus)
                {
                    CurrentDb.ProductSubjectSku.Remove(item);
                }


                var productSubjects = CurrentDb.ProductSubject.Where(m => rop.SubjectIds.Contains(m.Id)).ToList();

                productSku.SubjectIds = string.Join(",", productSubjects.Select(m => m.Id).ToArray());
                productSku.SubjectNames = string.Join(",", productSubjects.Select(m => m.Name).ToArray());

                foreach (var productSubject in productSubjects)
                {
                    var productSubjectSku = new ProductSubjectSku();
                    productSubjectSku.Id = GuidUtil.New();
                    productSubjectSku.ProductSubjectId = productSubject.Id;
                    productSubjectSku.ProductSkuId = productSku.Id;
                    productSubjectSku.Creator = pOperater;
                    productSubjectSku.CreateTime = this.DateTime;
                    CurrentDb.ProductSubjectSku.Add(productSubjectSku);
                }

                CurrentDb.SaveChanges(true);
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }

        public CustomJsonResult EditBySalePrice(string pOperater, string pStoreId, string pProductSkuId, decimal pProductSkuSalePrice)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMachineStocks = CurrentDb.StoreSellStock.Where(m => m.StoreId == pStoreId && m.ProductSkuId == pProductSkuId).ToList();

                foreach (var machineStock in lMachineStocks)
                {
                    machineStock.SalePrice = pProductSkuSalePrice;
                    machineStock.Mender = pOperater;
                    machineStock.MendTime = this.DateTime;
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }


        //public SkuModel Details(string skuId)
        //{
        //    var model = BizFactory.ProductSku.GetModel(skuId);

        //    var sku = new SkuModel();

        //    sku.Id = model.Id;
        //    sku.Name = model.Name;
        //    sku.SalePrice = model.SalePrice.ToF2Price();
        //    sku.ShowPrice = model.ShowPrice.ToF2Price();
        //    sku.DetailsDes = model.DetailsDes;
        //    sku.BriefInfo = model.BriefInfo;
        //    sku.DispalyImgUrls = model.DispalyImgUrls.ToJsonObject<List<ImgSet>>();

        //    return sku;
        //}

        public SkuModel GetModel(string pId)
        {
            var skuModel = new SkuModel();
            var sku = ProductSkuCacheUtil.GetOne(pId);

            if (sku == null)
            {
                sku = CurrentDb.ProductSku.Where(m => m.Id == pId).FirstOrDefault();
                if (sku != null)
                {
                    ProductSkuCacheUtil.Add(sku);
                }
            }

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

            var sysUsers = CurrentDb.SysUser.Where(m => m.Type == Enumeration.UserType.Merchant).ToList();

            foreach (var user in sysUsers)
            {
                var productSkus = CurrentDb.ProductSku.Where(m => m.MerchantId == user.Id).ToList();

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

    }
}