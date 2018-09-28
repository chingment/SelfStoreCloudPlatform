using Lumos.Entity;
using Lumos.Mvc;
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

        public CustomJsonResult Add(string pOperater, ProductKind pProductKind)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExistProductKind = CurrentDb.ProductKind.Where(m => m.MerchantId == pProductKind.MerchantId && m.Name == pProductKind.Name).FirstOrDefault();
                if (isExistProductKind != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }

                var lMerchant = CurrentDb.MerchantConfig.Where(m => m.MerchantId == pProductKind.MerchantId).FirstOrDefault();

                pProductKind.Id = GuidUtil.New();
                pProductKind.Creator = pOperater;
                pProductKind.CreateTime = DateTime.Now;
                int depth = 0;
                GetDepth(pProductKind.PId, ref depth);
                pProductKind.Depth = depth;
                CurrentDb.ProductKind.Add(pProductKind);
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

        public CustomJsonResult Edit(string pOperater, ProductKind pProductKind)
        {
            var lProductKind = CurrentDb.ProductKind.Where(m => m.Id == pProductKind.Id).FirstOrDefault();

            var isExistProductKind = CurrentDb.ProductKind.Where(m => m.MerchantId == lProductKind.MerchantId && m.Id != lProductKind.Id && m.Name == lProductKind.Name).FirstOrDefault();
            if (isExistProductKind != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            }


            lProductKind.Name = pProductKind.Name;
            lProductKind.MainImg = pProductKind.MainImg;
            lProductKind.IconImg = pProductKind.IconImg;
            lProductKind.Status = pProductKind.Status;
            lProductKind.Description = pProductKind.Description;
            lProductKind.Mender = pOperater;
            lProductKind.MendTime = DateTime.Now;
            int depth = 0;
            GetDepth(lProductKind.PId, ref depth);
            lProductKind.Depth = depth;
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

        public CustomJsonResult Delete(string pOperater, string[] pIds)
        {
            if (pIds != null)
            {
                foreach (var id in pIds)
                {
                    var productKind = CurrentDb.ProductKind.Where(m => m.Id == id).FirstOrDefault();
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
