﻿using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Lumos.Redis;
using NPinyin;
using System.Text;
using Lumos.BLL.Biz;

namespace Lumos.BLL.Service.Merch
{

    public class ProductSkuProvider : BaseProvider
    {

        public string[] ToArrary(string str)
        {

            if (string.IsNullOrWhiteSpace(str))
                return null;

            string[] arr = null;
            try
            {
                arr = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch
            {
                arr = null;
            }

            return arr;
        }
        public CustomJsonResult GetDetails(string pOperater, string pMerchantId, string pProductSkuId)
        {
            var ret = new RetProductSkuGetDetails();
            var productSku = CurrentDb.ProductSku.Where(m => m.MerchantId == pMerchantId && m.Id == pProductSkuId).FirstOrDefault();
            if (productSku != null)
            {
                ret.ProductSkuId = productSku.Id ?? "";
                ret.Name = productSku.Name ?? "";
                ret.SalePrice = productSku.SalePrice;
                ret.ShowPrice = productSku.ShowPrice;
                ret.DetailsDes = productSku.DetailsDes;
                ret.BriefInfo = productSku.BriefInfo;
                ret.KindIds = ToArrary(productSku.KindIds);
                ret.SubjectIds = ToArrary(productSku.SubjectIds);
                ret.RecipientModeIds = ToArrary(productSku.RecipientModeIds);
                ret.DispalyImgUrls = productSku.DispalyImgUrls.ToJsonObject<List<ImgSet>>(); ;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

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


                var recipientModes = MerchServiceFactory.RecipientMode.GetList().Where(m => rop.RecipientModeIds.Contains(m.Id)).ToList();

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

            if (string.IsNullOrEmpty(rop.ProductSkuId))
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
                var productSku = CurrentDb.ProductSku.Where(m => m.Id == rop.ProductSkuId).FirstOrDefault();

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

                var recipientModes = MerchServiceFactory.RecipientMode.GetList().Where(m => rop.RecipientModeIds.Contains(m.Id)).ToList();

                productSku.RecipientModeIds = string.Join(",", recipientModes.Select(m => m.Id).ToArray());
                productSku.RecipientModeNames = string.Join(",", recipientModes.Select(m => m.Name).ToArray());

                var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductSkuId == rop.ProductSkuId).ToList();

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


                if (rop.SubjectIds != null)
                {
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
                }

                CurrentDb.SaveChanges(true);
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }

        public CustomJsonResult EditBySalePrice(string pOperater, string pMerchantId, RopProductSkuEditSalePrice rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMachineStocks = CurrentDb.StoreSellStock.Where(m => m.MerchantId == pMerchantId && m.StoreId == rop.StoreId && m.ProductSkuId == rop.ProductSkuId).ToList();

                foreach (var machineStock in lMachineStocks)
                {
                    machineStock.SalePrice = rop.SalePrice;
                    machineStock.Mender = pOperater;
                    machineStock.MendTime = this.DateTime;
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }

    }
}