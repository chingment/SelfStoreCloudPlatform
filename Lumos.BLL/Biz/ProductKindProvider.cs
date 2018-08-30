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
                var isExistProductKind = CurrentDb.ProductKind.Where(m => m.UserId == pProductKind.UserId && m.Name == pProductKind.Name).FirstOrDefault();
                if (isExistProductKind != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }

                var lMerchant = CurrentDb.Merchant.Where(m => m.UserId == pProductKind.UserId).FirstOrDefault();

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

            var isExistProductKind = CurrentDb.ProductKind.Where(m => m.UserId == lProductKind.UserId && m.Id != lProductKind.Id && m.Name == lProductKind.Name).FirstOrDefault();
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

        public CustomJsonResult RemoveProductFromKind(string pOperater, string pKindId, string[] pProductIds)
        {
            //CustomJsonResult result = new CustomJsonResult();

            //if (productIds != null)
            //{
            //    string str_id = kindId.ToString();

            //    var products = CurrentDb.Product.Where(m => productIds.Contains(m.Id)).ToList();

            //    foreach (var product in products)
            //    {
            //        product.ProductKindIds = GetRemoveKindId(product.ProductKindIds, new string[1] { str_id });

            //        CurrentDb.SaveChanges();
            //    }

            //}

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }


        private string GetRemoveKindId(string pKindIds, string[] pProductKindIds)
        {
            //if (kindIds == null)
            //    return null;

            //string[] arrKindId = kindIds.Split(',');

            //ArrayList ar = new ArrayList(arrKindId);

            //if (removekindIds != null)
            //{
            //    foreach (var id in removekindIds)
            //    {
            //        ar.Remove(id);
            //    }
            //}

            //string str = string.Join(",", ar.ToArray());


            //str = BizFactory.Product.BuildProductKindIds(str);

            //return str;
            return null;
        }
    }
}
