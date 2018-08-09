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

        public CustomJsonResult Add(string operater, ProductKind productKind)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.ProductKind.Where(m => m.UserId == productKind.UserId && m.Name == productKind.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }

                var merchant = CurrentDb.Merchant.Where(m => m.UserId == productKind.UserId).FirstOrDefault();

                productKind.Id = GuidUtil.New();
                productKind.MerchantId = merchant.Id;
                productKind.Creator = operater;
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


        private void GetDepth(string pId, ref int level)
        {

            var l_productKind = CurrentDb.ProductKind.Where(m => m.Id == pId).FirstOrDefault();
            if (l_productKind != null)
            {
                level += 1;

                GetDepth(l_productKind.PId, ref level);
            }
        }

        public CustomJsonResult Edit(string operater, ProductKind productKind)
        {
            var l_productKind = CurrentDb.ProductKind.Where(m => m.Id == productKind.Id).FirstOrDefault();

            var existObject = CurrentDb.ProductKind.Where(m => m.UserId == l_productKind.UserId && m.Id != productKind.Id && m.Name == productKind.Name).FirstOrDefault();
            if (existObject != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            }


            l_productKind.Name = productKind.Name;
            l_productKind.MainImg = productKind.MainImg;
            l_productKind.IconImg = productKind.IconImg;
            l_productKind.Status = productKind.Status;
            l_productKind.Description = productKind.Description;
            l_productKind.Mender = operater;
            l_productKind.LastUpdateTime = DateTime.Now;
            int depth = 0;
            GetDepth(l_productKind.PId, ref depth);
            l_productKind.Depth = depth;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }

        public IEnumerable<ProductKind> GetProductKind(string p_id)
        {
            var query = from c in CurrentDb.ProductKind
                        where c.PId == p_id
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetProductKind(t.Id)));
        }

        public CustomJsonResult Delete(string operater, string[] ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var productKind = CurrentDb.ProductKind.Where(m => m.Id == id).FirstOrDefault();
                    if (productKind != null)
                    {
                        productKind.IsDelete = true;

                        var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductKindId == id).ToList();

                        foreach (var productKindSku in productKindSkus)
                        {
                            CurrentDb.ProductKindSku.Remove(productKindSku);

                            //var productSku = CurrentDb.ProductSku.Where(m => m.Id == productKindSku.ProductSkuId).FirstOrDefault();

                            //if (productSku != null)
                            //{
                            //    productSku.KindIds = null;
                            //    productSku.KindNames = null;
                            //    productKind.Mender = operater;
                            //    productKind.LastUpdateTime = this.DateTime;
                            //}
                        }

                        CurrentDb.SaveChanges();
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public CustomJsonResult RemoveProductFromKind(string operater, string kindId, string[] productIds)
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

        public CustomJsonResult ListAllKinds()
        {
            //var parentProductKinds = CurrentDb.ProductKind.Where(m => m.PId == 1).ToList();
            //List<SuperProductKind> superProductKinds = new List<SuperProductKind>();
            //foreach (var parentProductKind in parentProductKinds)
            //{
            //    SuperProductKind superProductKind = new SuperProductKind();
            //    superProductKind.Id = "k" + parentProductKind.Id.ToString();//前端组件要求id不能以数字开头
            //    superProductKind.Name = parentProductKind.Name;
            //    superProductKind.IconImg = parentProductKind.IconImg;
            //    superProductKind.MainImg = parentProductKind.MainImg;



            //    var childProductKinds = CurrentDb.ProductKind.Where(m => m.PId == parentProductKind.Id).ToList();
            //    foreach (var childProductKind in childProductKinds)
            //    {
            //        SubProductKind subProductKind = new SubProductKind();
            //        subProductKind.Id = "k" + childProductKind.Id.ToString();
            //        subProductKind.Name = childProductKind.Name;
            //        subProductKind.IconImg = childProductKind.IconImg;
            //        subProductKind.PId = "k" + parentProductKind.Id.ToString();
            //        subProductKind.SuperName = parentProductKind.Name;

            //        superProductKind.SubProductKinds.Add(subProductKind);
            //    }
            //    superProductKinds.Add(superProductKind);
            //}

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功", null);
        }


        private string GetRemoveKindId(string kindIds, string[] removekindIds)
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
