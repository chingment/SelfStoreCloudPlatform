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
    public class ProductSubjectProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetProductSubjectGetDetails();
            var productKind = CurrentDb.ProductSubject.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
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


        public CustomJsonResult Add(string operater, string merchantId, RopProductSubjectAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExistProductSubject = CurrentDb.ProductSubject.Where(m => m.MerchantId == merchantId && m.Name == rop.Name).FirstOrDefault();
                if (isExistProductSubject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }

                var productSubject = new ProductSubject();
                productSubject.Id = GuidUtil.New();
                productSubject.MerchantId = merchantId;
                productSubject.Name = rop.Name;
                productSubject.MainImg = rop.MainImg;
                productSubject.IconImg = rop.IconImg;
                productSubject.Description = rop.Description;
                productSubject.Status = Enumeration.ProductSubjectStatus.Valid;
                productSubject.Creator = operater;
                productSubject.CreateTime = DateTime.Now;
                CurrentDb.ProductSubject.Add(productSubject);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string operater, string merchantId, RopProductSubjectEdit rop)
        {
            var productSubject = CurrentDb.ProductSubject.Where(m => m.Id == rop.Id).FirstOrDefault();

            var isExistlProductSubject = CurrentDb.ProductSubject.Where(m => m.MerchantId == merchantId && m.Id != productSubject.Id && m.Name == rop.Name).FirstOrDefault();
            if (isExistlProductSubject != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            }


            productSubject.Name = rop.Name;
            productSubject.MainImg = rop.MainImg;
            productSubject.IconImg = rop.IconImg;
            productSubject.Status = rop.Status;
            productSubject.Description = rop.Description;
            productSubject.Mender = operater;
            productSubject.MendTime = DateTime.Now;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }

        public CustomJsonResult Delete(string operater, string merchantId, string[] ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var productSubject = CurrentDb.ProductSubject.Where(m => m.Id == id).FirstOrDefault();
                    if (productSubject != null)
                    {
                        productSubject.IsDelete = true;

                        var productSubjectSkus = CurrentDb.ProductSubjectSku.Where(m => m.ProductSubjectId == id).ToList();

                        foreach (var productKindSku in productSubjectSkus)
                        {
                            CurrentDb.ProductSubjectSku.Remove(productKindSku);
                        }

                        CurrentDb.SaveChanges();
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

    }
}
