using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class ProductKindProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string pOperater, string pMerchantId, string pKindId)
        {
            var ret = new RetProductKindGetDetails();
            var productKind = CurrentDb.ProductKind.Where(m => m.MerchantId == pMerchantId && m.Id == pKindId).FirstOrDefault();
            if (productKind != null)
            {
                ret.KindId = productKind.Id ?? "";
                ret.Name = productKind.Name ?? "";
                ret.MainImg = productKind.MainImg ?? "";
                ret.IconImg = productKind.IconImg ?? "";
                ret.Description = productKind.Description ?? "";
                ret.Status = productKind.Status;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }


        public CustomJsonResult Add(string pOperater, string pMerchantId, RopProductKindAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExistProductKind = CurrentDb.ProductKind.Where(m => m.MerchantId == pMerchantId && m.Name == rop.Name).FirstOrDefault();
                if (isExistProductKind != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }

                var productKind = new ProductKind();

                productKind.Id = GuidUtil.New();
                productKind.MerchantId = pMerchantId;
                productKind.PId = rop.PKindId;
                productKind.Name = rop.Name;
                productKind.MainImg = rop.MainImg;
                productKind.IconImg = rop.IconImg;
                productKind.Description = rop.Description;
                productKind.Status = Enumeration.ProductKindStatus.Valid;
                productKind.Creator = pOperater;
                productKind.CreateTime = DateTime.Now;
                int depth = 0;
                GetDepth(productKind.PId, ref depth);
                productKind.Depth = depth;
                CurrentDb.ProductKind.Add(productKind);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
            }

            return result;
        }


        private void GetDepth(string pPId, ref int pLevel)
        {

            var l_productKind = CurrentDb.ProductKind.Where(m => m.Id == pPId).FirstOrDefault();
            if (l_productKind != null)
            {
                pLevel += 1;

                GetDepth(l_productKind.PId, ref pLevel);
            }
        }

        public CustomJsonResult Edit(string pOperater, string pMerchantId, RopProductKindEdit rop)
        {
            var productKind = CurrentDb.ProductKind.Where(m => m.Id == rop.KindId).FirstOrDefault();

            var isExistProductKind = CurrentDb.ProductKind.Where(m => m.MerchantId == pMerchantId && m.Id != productKind.Id && m.Name == rop.Name).FirstOrDefault();
            if (isExistProductKind != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            }


            productKind.Name = rop.Name;
            productKind.MainImg = rop.MainImg;
            productKind.IconImg = rop.IconImg;
            productKind.Status = rop.Status;
            productKind.Description = rop.Description;
            productKind.Mender = pOperater;
            productKind.MendTime = DateTime.Now;
            int depth = 0;
            GetDepth(productKind.PId, ref depth);
            productKind.Depth = depth;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }

        public IEnumerable<ProductKind> GetProductKind(string pPId)
        {
            var query = from c in CurrentDb.ProductKind
                        where c.PId == pPId
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetProductKind(t.Id)));
        }

        public CustomJsonResult Delete(string pOperater, string pMerchantId, string[] pKindIds)
        {
            if (pKindIds != null)
            {
                foreach (var id in pKindIds)
                {
                    var productKind = CurrentDb.ProductKind.Where(m => m.MerchantId == pMerchantId && m.Id == id).FirstOrDefault();
                    if (productKind != null)
                    {
                        productKind.IsDelete = true;

                        var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductKindId == id).ToList();

                        foreach (var productKindSku in productKindSkus)
                        {
                            CurrentDb.ProductKindSku.Remove(productKindSku);
                        }

                        CurrentDb.SaveChanges();
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public CustomJsonResult RemoveProductSku(string pOperater, string pKindId, string pSkuId)
        {
            CustomJsonResult result = new CustomJsonResult();

            var productSku = CurrentDb.ProductSku.Where(m => m.Id == pSkuId).FirstOrDefault();

            var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductSkuId == pSkuId).ToList();

            foreach (var productKindSku in productKindSkus)
            {

                if (productKindSku.ProductKindId == pKindId)
                {
                    CurrentDb.ProductKindSku.Remove(productKindSku);
                }
            }

            var kindIds = productKindSkus.Select(m => m.ProductKindId).ToArray();
            var productKinds = CurrentDb.ProductKind.Where(m => kindIds.Contains(m.Id)).ToList();
            productSku.KindIds = string.Join(",", productKinds.Select(m => m.Id).ToArray());
            productSku.KindNames = string.Join(",", productKinds.Select(m => m.Name).ToArray());

            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

    }
}
