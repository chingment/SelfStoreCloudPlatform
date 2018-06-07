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
            //if (productKind.PId == "0")
            //{
            //    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择上级分类");
            //}

            //using (TransactionScope ts = new TransactionScope())
            //{
            //    var existObject = CurrentDb.ProductKind.Where(m => m.Name == productKind.Name).FirstOrDefault();
            //    if (existObject != null)
            //    {
            //        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在");
            //    }

            //    existObject = CurrentDb.ProductKind.Where(m => m.PId == productKind.PId).OrderByDescending(m => m.Id).FirstOrDefault();
            //    if (existObject == null)//同级没有分类
            //    {
            //        productKind.Id = productKind.PId * 10000 + 1;
            //    }
            //    else//同级已存在分类则+1
            //    {
            //        productKind.Id = existObject.Id + 1;
            //    }

            //    productKind.Creator = operater;
            //    productKind.CreateTime = DateTime.Now;
            //    CurrentDb.ProductKind.Add(productKind);
            //    CurrentDb.SaveChanges();
            //    ts.Complete();
            //    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
            //}

            return result;
        }

        public CustomJsonResult Edit(string operater, ProductKind productKind)
        {

            var _productKind = CurrentDb.ProductKind.Where(m => m.Id == productKind.Id).FirstOrDefault();

            _productKind.Name = productKind.Name;
            _productKind.MainImg = productKind.MainImg;
            _productKind.IconImg = productKind.IconImg;
            _productKind.Status = productKind.Status;
            _productKind.Description = productKind.Description;
            _productKind.Mender = operater;
            _productKind.LastUpdateTime = DateTime.Now;

            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");

        }

        public CustomJsonResult Delete(string operater, string[] ids)
        {

            //if (ids != null)
            //{
            //    foreach (var id in ids)
            //    {
            //        var productKind = CurrentDb.ProductKind.Where(m => m.Id == id).FirstOrDefault();
            //        if (productKind != null)
            //        {
            //            productKind.IsDelete = true;

            //            string str_id = id.ToString();

            //            string search_id = BizFactory.Product.BuildProductKindIdForSearch(str_id);

            //            var products = CurrentDb.Product.Where(m => SqlFunctions.CharIndex(search_id, m.ProductKindIds) > 0).ToList();

            //            foreach (var product in products)
            //            {
            //                product.ProductKindIds = GetRemoveKindId(product.ProductKindIds, new string[1] { str_id });

            //                CurrentDb.SaveChanges();
            //            }

            //            CurrentDb.SaveChanges();
            //        }

            //    }

            //}

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
