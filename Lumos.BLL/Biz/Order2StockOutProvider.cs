using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class Order2StockOutProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string pOperater, string pMerchantId, string pOrder2StockOutId)
        {
            var ret = new RetOrder2StockOutGetDetails();


            var order2StockOut = CurrentDb.Order2StockOut.Where(m => m.Id == pOrder2StockOutId).FirstOrDefault();

            if (order2StockOut != null)
            {
                ret.Order2StockOutId = order2StockOut.Id;
                ret.Sn = order2StockOut.Sn;
                ret.StockOutTime = order2StockOut.StockOutTime.ToUnifiedFormatDateTime();
                ret.WarehouseName = order2StockOut.WarehouseName;
                ret.StoreName = order2StockOut.StoreName;
                ret.Description = order2StockOut.Description;
                ret.Quantity = order2StockOut.Quantity;

                var order2StockOutDetails = CurrentDb.Order2StockOutDetails.Where(m => m.Order2StockOutId == order2StockOut.Id).ToList();

                foreach (var item in order2StockOutDetails)
                {
                    ret.Skus.Add(new RetOrder2StockOutGetDetails.Sku { SkuId = item.ProductSkuId, BarCode = item.ProductSkuBarCode, Name = item.ProductSkuName, Quantity = item.Quantity });
                }
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, string pMerchantId, RopOrder2StockOutAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var warehouse = CurrentDb.Warehouse.Where(m => m.Id == rop.WarehouseId).FirstOrDefault();
                var store = CurrentDb.Store.Where(m => m.Id == rop.StoreId).FirstOrDefault();

                var order2StockOut = new Order2StockOut();
                order2StockOut.Id = GuidUtil.New();
                order2StockOut.MerchantId = pMerchantId;
                order2StockOut.Sn = SnUtil.Build(Enumeration.BizSnType.Order2StockOut, order2StockOut.MerchantId);
                order2StockOut.Quantity = rop.Skus.Select(m => m.Quantity).Sum();
                order2StockOut.WarehouseId = warehouse.Id;
                order2StockOut.WarehouseName = warehouse.Name;
                order2StockOut.StoreId = store.Id;
                order2StockOut.StoreName = store.Name;
                order2StockOut.StockOutTime = rop.StockOutTime;
                order2StockOut.Description = rop.Description;
                order2StockOut.Creator = pOperater;
                order2StockOut.CreateTime = this.DateTime;
                CurrentDb.Order2StockOut.Add(order2StockOut);
                CurrentDb.SaveChanges();

                foreach (var sku in rop.Skus)
                {
                    var productSKu = CurrentDb.ProductSku.Where(m => m.Id == sku.SkuId).FirstOrDefault();

                    var order2StockOutDetails = new Order2StockOutDetails();
                    order2StockOutDetails.Id = GuidUtil.New();
                    order2StockOutDetails.MerchantId = pMerchantId;
                    order2StockOutDetails.Order2StockOutId = order2StockOut.Id;
                    order2StockOutDetails.WarehouseId = order2StockOut.WarehouseId;
                    order2StockOutDetails.WarehouseName = order2StockOut.WarehouseName;
                    order2StockOutDetails.StoreId = order2StockOut.StoreId;
                    order2StockOutDetails.StoreName = order2StockOut.StoreName;
                    order2StockOutDetails.StockOutTime = order2StockOut.StockOutTime;
                    order2StockOutDetails.Creator = order2StockOut.Creator;
                    order2StockOutDetails.CreateTime = order2StockOut.CreateTime;
                    order2StockOutDetails.ProductSkuId = productSKu.Id;
                    order2StockOutDetails.ProductSkuBarCode = productSKu.BarCode;
                    order2StockOutDetails.ProductSkuName = productSKu.Name;
                    order2StockOutDetails.Quantity = sku.Quantity;
                    CurrentDb.Order2StockOutDetails.Add(order2StockOutDetails);
                    CurrentDb.SaveChanges();


                    var warehouseStock = CurrentDb.WarehouseStock.Where(m => m.MerchantId == pMerchantId && m.WarehouseId == order2StockOut.WarehouseId && m.ProductSkuId == sku.SkuId).FirstOrDefault();

                    if (warehouseStock == null)
                    {
                        warehouseStock = new WarehouseStock();
                        warehouseStock.Id = GuidUtil.New();
                        warehouseStock.MerchantId = order2StockOutDetails.MerchantId;
                        warehouseStock.WarehouseId = order2StockOutDetails.WarehouseId;
                        warehouseStock.WarehouseName = order2StockOutDetails.WarehouseName;
                        warehouseStock.ProductSkuId = order2StockOutDetails.ProductSkuId;
                        warehouseStock.ProductSkuName = order2StockOutDetails.ProductSkuName;
                        warehouseStock.Quantity = order2StockOutDetails.Quantity;
                        warehouseStock.CreateTime = order2StockOutDetails.CreateTime;
                        warehouseStock.Creator = order2StockOutDetails.Creator;
                        CurrentDb.WarehouseStock.Add(warehouseStock);
                        CurrentDb.SaveChanges();
                    }
                    else
                    {
                        warehouseStock.Quantity += order2StockOutDetails.Quantity;
                        warehouseStock.MendTime = order2StockOutDetails.CreateTime;
                        warehouseStock.Mender = order2StockOutDetails.Creator;
                    }


                    var warehouseStockLog = new WarehouseStockLog();
                    warehouseStockLog.Id = GuidUtil.New();
                    warehouseStockLog.MerchantId = order2StockOutDetails.MerchantId;
                    warehouseStockLog.WarehouseId = order2StockOutDetails.WarehouseId;
                    warehouseStockLog.WarehouseName = order2StockOutDetails.WarehouseName;
                    warehouseStockLog.ProductSkuId = order2StockOutDetails.ProductSkuId;
                    warehouseStockLog.ProductSkuName = order2StockOutDetails.ProductSkuName;
                    warehouseStockLog.Quantity = warehouseStock.Quantity;
                    warehouseStockLog.ChangeType = Enumeration.WarehouseStockLogChangeTpye.StockOut;
                    warehouseStockLog.ChangeQuantity = order2StockOutDetails.Quantity;
                    warehouseStockLog.CreateTime = order2StockOutDetails.CreateTime;
                    warehouseStockLog.Creator = order2StockOutDetails.Creator;
                    CurrentDb.WarehouseStockLog.Add(warehouseStockLog);
                    CurrentDb.SaveChanges();


                }

                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "保存成功");
            }

            return result;

        }
    }
}
