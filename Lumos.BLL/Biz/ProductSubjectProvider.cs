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
    public class ProductSubjectProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, ProductSubject pProductSubject)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var isExistProductSubject = CurrentDb.ProductSubject.Where(m => m.MerchantId == pProductSubject.MerchantId && m.Name == pProductSubject.Name).FirstOrDefault();
                if (isExistProductSubject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
                }
                pProductSubject.Id = GuidUtil.New();
                pProductSubject.Creator = pOperater;
                pProductSubject.CreateTime = DateTime.Now;
                CurrentDb.ProductSubject.Add(pProductSubject);
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
            }

            return result;
        }

        public CustomJsonResult Edit(string pOperater, ProductSubject pProductSubject)
        {
            var lProductSubject = CurrentDb.ProductSubject.Where(m => m.Id == pProductSubject.Id).FirstOrDefault();

            var isExistlProductSubject = CurrentDb.ProductSubject.Where(m => m.MerchantId == lProductSubject.MerchantId && m.Id != lProductSubject.Id && m.Name == lProductSubject.Name).FirstOrDefault();
            if (isExistlProductSubject != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            }


            lProductSubject.Name = pProductSubject.Name;
            lProductSubject.MainImg = pProductSubject.MainImg;
            lProductSubject.IconImg = pProductSubject.IconImg;
            lProductSubject.Status = pProductSubject.Status;
            lProductSubject.Description = pProductSubject.Description;
            lProductSubject.Mender = pOperater;
            lProductSubject.MendTime = DateTime.Now;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }

        //public IEnumerable<ProductKind> GetProductProductSubject(string pPId)
        //{
        //    var query = from c in CurrentDb.ProductKind
        //                where c.PId == pPId
        //                select c;

        //    return query.ToList().Concat(query.ToList().SelectMany(t => GetProductKind(t.Id)));
        //}

        public CustomJsonResult Delete(string pOperater, string[] pIds)
        {
            if (pIds != null)
            {
                foreach (var id in pIds)
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
