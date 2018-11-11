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
    public class ProductSubjectProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string pOperater, string pMerchantId, string pSubjectId)
        {
            var ret = new RetProductSubjectGetDetails();
            var productKind = CurrentDb.ProductSubject.Where(m => m.MerchantId == pMerchantId && m.Id == pSubjectId).FirstOrDefault();
            if (productKind != null)
            {
                ret.SubjectId = productKind.Id ?? "";
                ret.Name = productKind.Name ?? "";
                ret.MainImg = productKind.MainImg ?? "";
                ret.IconImg = productKind.IconImg ?? "";
                ret.Description = productKind.Description ?? "";
                ret.Status = productKind.Status;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }


        public CustomJsonResult Add(string pOperater, string pMerchantId, RopProductSubjectAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExistProductSubject = CurrentDb.ProductSubject.Where(m => m.MerchantId == pMerchantId && m.Name == rop.Name).FirstOrDefault();
                if (isExistProductSubject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }

                var productSubject = new ProductSubject();
                productSubject.Id = GuidUtil.New();
                productSubject.MerchantId = pMerchantId;
                productSubject.Name = rop.Name;
                productSubject.MainImg = rop.MainImg;
                productSubject.IconImg = rop.IconImg;
                productSubject.Description = rop.Description;
                productSubject.Status = Enumeration.ProductSubjectStatus.Valid;
                productSubject.Creator = pOperater;
                productSubject.CreateTime = DateTime.Now;
                CurrentDb.ProductSubject.Add(productSubject);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, string pMerchantId, RopProductSubjectEdit rop)
        {
            var productSubject = CurrentDb.ProductSubject.Where(m => m.Id == rop.SubjectId).FirstOrDefault();

            var isExistlProductSubject = CurrentDb.ProductSubject.Where(m => m.MerchantId == pMerchantId && m.Id != productSubject.Id && m.Name == rop.Name).FirstOrDefault();
            if (isExistlProductSubject != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            }


            productSubject.Name = rop.Name;
            productSubject.MainImg = rop.MainImg;
            productSubject.IconImg = rop.IconImg;
            productSubject.Status = rop.Status;
            productSubject.Description = rop.Description;
            productSubject.Mender = pOperater;
            productSubject.MendTime = DateTime.Now;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }

        public CustomJsonResult Delete(string pOperater, string pMerchantId, string[] pKindIds)
        {
            if (pKindIds != null)
            {
                foreach (var id in pKindIds)
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

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

    }
}
