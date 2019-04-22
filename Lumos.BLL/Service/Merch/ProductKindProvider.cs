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
        private List<ProductKind> GetFathers(string merchantId, string id)
        {
            var productKind = CurrentDb.ProductKind.ToList();

            var list = new List<ProductKind>();
            var list2 = list.Concat(GetFatherList(productKind, id));
            return list2.ToList();
        }


        private IEnumerable<ProductKind> GetFatherList(IList<ProductKind> list, string pId)
        {
            var query = list.Where(p => p.Id == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetFatherList(list, t.PId)));
        }

        public List<ProductKind> GetSons(string merchantId, string id)
        {
            var productKinds = CurrentDb.ProductKind.Where(m => m.MerchantId == merchantId).ToList();
            var productKind = productKinds.Where(p => p.Id == id).ToList();
            var list2 = productKind.Concat(GetSonList(productKinds, id));
            return list2.ToList();
        }

        private IEnumerable<ProductKind> GetSonList(IList<ProductKind> list, string pId)
        {
            var query = list.Where(p => p.PId == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonList(list, t.Id)));
        }

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
                var fathter = GetFathers(merchantId, rop.PId);
                int dept = fathter.Count;
                var isExists = CurrentDb.ProductKind.Where(m => m.PId == rop.PId && m.MerchantId == merchantId && m.Name == rop.Name && m.Dept == dept).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "该名称在同一级别已经存在");
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
                productKind.Dept = dept;
                CurrentDb.ProductKind.Add(productKind);
                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

            }

            SdkFactory.PushService.UpdateMachineProductKind(merchantId);

            return result;
        }


        public CustomJsonResult Edit(string operater, string merchantId, RopProductKindEdit rop)
        {

            var productKind = CurrentDb.ProductKind.Where(m => m.Id == rop.Id).FirstOrDefault();
            if (productKind == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            var fathter = GetFathers(merchantId, productKind.PId);
            int dept = fathter.Count;
            var isExists = CurrentDb.ProductKind.Where(m => m.PId == productKind.PId && m.Name == rop.Name && m.Dept == dept && m.Id != rop.Id).FirstOrDefault();
            if (isExists != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("保存失败，该名称({0})已被同一级别使用", rop.Name));
            }

            productKind.Name = rop.Name;
            productKind.MainImg = rop.MainImg;
            productKind.IconImg = rop.IconImg;
            productKind.Status = rop.Status;
            productKind.Description = rop.Description;
            productKind.Mender = operater;
            productKind.MendTime = DateTime.Now;
            productKind.Dept = dept;
            CurrentDb.SaveChanges();

            SdkFactory.PushService.UpdateMachineProductKind(merchantId);

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }


        public CustomJsonResult Delete(string operater, string merchantId, string id)
        {

            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                var prodouctKind = CurrentDb.ProductKind.Where(m => m.Id == id).FirstOrDefault();

                if (prodouctKind == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }

                if (prodouctKind.Dept == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选（{0}）不允许删除", prodouctKind.Name));
                }

                var sons = GetSons(merchantId, id).ToList();


                foreach (var son in sons)
                {

                    CurrentDb.ProductKind.Remove(son);

                    var productKindSkus = CurrentDb.ProductKindSku.Where(m => m.ProductKindId == son.Id).ToList();

                    foreach (var productKindSku in productKindSkus)
                    {
                        CurrentDb.ProductKindSku.Remove(productKindSku);
                        CurrentDb.SaveChanges();

                        var productSku = CurrentDb.ProductSku.Where(m => m.Id == productKindSku.ProductSkuId).FirstOrDefault();
                        if (productSku != null)
                        {
                            var kindIds = CurrentDb.ProductKindSku.Where(m => m.ProductSkuId == productKindSku.ProductSkuId).Select(m => m.ProductKindId).ToArray();
                            if (kindIds != null && kindIds.Length > 0)
                            {
                                var productKinds = CurrentDb.ProductKind.Where(m => kindIds.Contains(m.Id)).ToList();
                                productSku.KindIds = string.Join(",", productKinds.Select(m => m.Id).ToArray());
                                productSku.KindNames = string.Join(",", productKinds.Select(m => m.Name).ToArray());

                            }
                            else
                            {
                                productSku.KindIds = null;
                                productSku.KindNames = null;
                            }

                            CurrentDb.SaveChanges();
                        }
                    }


                }


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            SdkFactory.PushService.UpdateMachineProductKind(merchantId);

            return result;

        }

        public CustomJsonResult EditSort(string operater, string merchantId, RopProductKindEditSort rop)
        {
            if (rop != null)
            {
                if (rop.Dics != null)
                {
                    foreach (var item in rop.Dics)
                    {
                        string id = item.Id;
                        int priority = item.Priority;
                        var sysRole = CurrentDb.ProductKind.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
                        if (sysRole != null)
                        {
                            sysRole.Priority = priority;
                            CurrentDb.SaveChanges();
                        }
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }

    }
}
