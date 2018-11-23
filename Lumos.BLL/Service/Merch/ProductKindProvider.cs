using Lumos.Entity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class ProductKindProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetProductKindGetDetails();
            var productKind = CurrentDb.ProductKind.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (productKind != null)
            {
                ret.Id = productKind.Id ?? "";
                ret.Name = productKind.Name ?? "";
                ret.MainImg = productKind.MainImg ?? "";
                ret.IconImg = productKind.IconImg ?? "";
                ret.Description = productKind.Description ?? "";
                ret.Status = productKind.Status;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }


        public CustomJsonResult Add(string operater, string merchantId, RopProductKindAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExistProductKind = CurrentDb.ProductKind.Where(m => m.MerchantId == merchantId && m.Name == rop.Name).FirstOrDefault();
                if (isExistProductKind != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }

                var productKind = new ProductKind();

                productKind.Id = GuidUtil.New();
                productKind.MerchantId = merchantId;
                productKind.PId = rop.PId;
                productKind.Name = rop.Name;
                productKind.MainImg = rop.MainImg;
                productKind.IconImg = rop.IconImg;
                productKind.Description = rop.Description;
                productKind.Status = Enumeration.ProductKindStatus.Valid;
                productKind.Creator = operater;
                productKind.CreateTime = DateTime.Now;
                int depth = 0;
                GetDepth(productKind.PId, ref depth);
                productKind.Depth = depth;
                CurrentDb.ProductKind.Add(productKind);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
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

        public CustomJsonResult Edit(string operater, string merchantId, RopProductKindEdit rop)
        {
            var productKind = CurrentDb.ProductKind.Where(m => m.Id == rop.Id).FirstOrDefault();

            var isExistProductKind = CurrentDb.ProductKind.Where(m => m.MerchantId == merchantId && m.Id != productKind.Id && m.Name == rop.Name).FirstOrDefault();
            if (isExistProductKind != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            }


            productKind.Name = rop.Name;
            productKind.MainImg = rop.MainImg;
            productKind.IconImg = rop.IconImg;
            productKind.Status = rop.Status;
            productKind.Description = rop.Description;
            productKind.Mender = operater;
            productKind.MendTime = DateTime.Now;
            int depth = 0;
            GetDepth(productKind.PId, ref depth);
            productKind.Depth = depth;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }

        public IEnumerable<ProductKind> GetProductKind(string pId)
        {
            var query = from c in CurrentDb.ProductKind
                        where c.PId == pId
                        select c;

            return query.ToList().Concat(query.ToList().SelectMany(t => GetProductKind(t.Id)));
        }

        public CustomJsonResult Delete(string operater, string merchantId, string[] ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var productKind = CurrentDb.ProductKind.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
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

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

    }
}
