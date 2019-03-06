using Lumos.Entity;
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
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetProductSkuGetDetails();
            var productSku = CurrentDb.ProductSku.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (productSku != null)
            {
                ret.Id = productSku.Id ?? "";
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

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult Add(string operater, string merchantId, RopProducSkuAdd rop)
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
                productSku.MerchantId = merchantId;
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
                productSku.Creator = operater;
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
                    productKindSku.Creator = operater;
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
                    productSubjectSku.Creator = operater;
                    productSubjectSku.CreateTime = this.DateTime;
                    CurrentDb.ProductSubjectSku.Add(productSubjectSku);
                }

                CurrentDb.ProductSku.Add(productSku);
                CurrentDb.SaveChanges(true);
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string operater, string merchantId, RopProducSkuEdit rop)
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
                productSku.DispalyImgUrls = rop.DispalyImgUrls.ToJsonString();
                productSku.Mender = operater;
                productSku.MendTime = this.DateTime;

                var recipientModes = MerchServiceFactory.RecipientMode.GetList().Where(m => rop.RecipientModeIds.Contains(m.Id)).ToList();

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
                    productKindSku.Creator = operater;
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
                        productSubjectSku.Creator = operater;
                        productSubjectSku.CreateTime = this.DateTime;
                        CurrentDb.ProductSubjectSku.Add(productSubjectSku);
                    }
                }

                CurrentDb.SaveChanges(true);
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
        }

        public CustomJsonResult EditBySalePrice(string operater, string merchantId, RopProductSkuEditSalePrice rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var machineStocks = CurrentDb.StoreSellStock.Where(m => m.MerchantId == merchantId && m.StoreId == rop.StoreId && m.ProductSkuId == rop.ProductSkuId).ToList();

                foreach (var machineStock in machineStocks)
                {
                    machineStock.SalePrice = rop.SalePrice;
                    machineStock.Mender = operater;
                    machineStock.MendTime = this.DateTime;
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
        }

    }
}