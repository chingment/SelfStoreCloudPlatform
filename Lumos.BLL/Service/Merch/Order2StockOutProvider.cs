using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class Order2StockOutProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetOrder2StockOutGetDetails();


            var order2StockOut = CurrentDb.ImsOrder2StockOut.Where(m => m.Id == id).FirstOrDefault();

            if (order2StockOut != null)
            {
                ret.Id = order2StockOut.Id;
                ret.Sn = order2StockOut.Sn;
                ret.StockOutTime = order2StockOut.StockOutTime.ToUnifiedFormatDateTime();
                ret.WarehouseName = order2StockOut.WarehouseName;
                ret.TargetName = string.Format("[{0}]{1}", order2StockOut.TargetType.GetCnName(), order2StockOut.TargetName);
                ret.Description = order2StockOut.Description;
                ret.Quantity = order2StockOut.Quantity;

                var order2StockOutDetails = CurrentDb.ImsOrder2StockOutDetails.Where(m => m.Order2StockOutId == order2StockOut.Id).ToList();

                foreach (var item in order2StockOutDetails)
                {
                    ret.Skus.Add(new RetOrder2StockOutGetDetails.Sku { SkuId = item.ProductSkuId, BarCode = item.ProductSkuBarCode, Name = item.ProductSkuName, Quantity = item.Quantity });
                }
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult Add(string operater, string merchantId, RopOrder2StockOutAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var warehouse = CurrentDb.ImsWarehouse.Where(m => m.Id == rop.WarehouseId).FirstOrDefault();

                var order2StockOut = new ImsOrder2StockOut();
                order2StockOut.Id = GuidUtil.New();
                order2StockOut.MerchantId = merchantId;
                order2StockOut.Sn = SnUtil.Build(Enumeration.BizSnType.Order2StockOut, order2StockOut.MerchantId);
                order2StockOut.Quantity = rop.Skus.Select(m => m.Quantity).Sum();
                order2StockOut.WarehouseId = warehouse.Id;
                order2StockOut.WarehouseName = warehouse.Name;
                order2StockOut.TargetId = rop.TargetId;
                order2StockOut.TargetName = rop.TargetName;
                order2StockOut.TargetType = rop.TargetType;
                order2StockOut.StockOutTime = rop.StockOutTime;
                order2StockOut.Description = rop.Description;
                order2StockOut.Creator = operater;
                order2StockOut.CreateTime = this.DateTime;
                CurrentDb.ImsOrder2StockOut.Add(order2StockOut);
                CurrentDb.SaveChanges();

                foreach (var sku in rop.Skus)
                {
                    var productSKu = CurrentDb.ProductSku.Where(m => m.Id == sku.SkuId).FirstOrDefault();

                    var order2StockOutDetails = new ImsOrder2StockOutDetails();
                    order2StockOutDetails.Id = GuidUtil.New();
                    order2StockOutDetails.MerchantId = merchantId;
                    order2StockOutDetails.Order2StockOutId = order2StockOut.Id;
                    order2StockOutDetails.WarehouseId = order2StockOut.WarehouseId;
                    order2StockOutDetails.WarehouseName = order2StockOut.WarehouseName;
                    order2StockOutDetails.TargetId = order2StockOut.TargetId;
                    order2StockOutDetails.TargetName = order2StockOut.TargetName;
                    order2StockOutDetails.TargetType = order2StockOut.TargetType;
                    order2StockOutDetails.StockOutTime = order2StockOut.StockOutTime;
                    order2StockOutDetails.Creator = order2StockOut.Creator;
                    order2StockOutDetails.CreateTime = order2StockOut.CreateTime;
                    order2StockOutDetails.ProductSkuId = productSKu.Id;
                    order2StockOutDetails.ProductSkuBarCode = productSKu.BarCode;
                    order2StockOutDetails.ProductSkuName = productSKu.Name;
                    order2StockOutDetails.Quantity = sku.Quantity;
                    CurrentDb.ImsOrder2StockOutDetails.Add(order2StockOutDetails);
                    CurrentDb.SaveChanges();


                    var warehouseStock = CurrentDb.ImsWarehouseStock.Where(m => m.MerchantId == merchantId && m.WarehouseId == order2StockOut.WarehouseId && m.ProductSkuId == sku.SkuId).FirstOrDefault();

                    if (warehouseStock == null)
                    {
                        warehouseStock = new ImsWarehouseStock();
                        warehouseStock.Id = GuidUtil.New();
                        warehouseStock.MerchantId = order2StockOutDetails.MerchantId;
                        warehouseStock.WarehouseId = order2StockOutDetails.WarehouseId;
                        warehouseStock.WarehouseName = order2StockOutDetails.WarehouseName;
                        warehouseStock.ProductSkuId = order2StockOutDetails.ProductSkuId;
                        warehouseStock.ProductSkuName = order2StockOutDetails.ProductSkuName;
                        warehouseStock.Quantity = order2StockOutDetails.Quantity;
                        warehouseStock.CreateTime = order2StockOutDetails.CreateTime;
                        warehouseStock.Creator = order2StockOutDetails.Creator;
                        CurrentDb.ImsWarehouseStock.Add(warehouseStock);
                        CurrentDb.SaveChanges();
                    }
                    else
                    {
                        warehouseStock.Quantity += order2StockOutDetails.Quantity;
                        warehouseStock.MendTime = order2StockOutDetails.CreateTime;
                        warehouseStock.Mender = order2StockOutDetails.Creator;
                    }


                    var warehouseStockLog = new ImsWarehouseStockLog();
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
                    CurrentDb.ImsWarehouseStockLog.Add(warehouseStockLog);
                    CurrentDb.SaveChanges();


                }

                CurrentDb.SaveChanges();

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, "操作成功");
            }

            return result;

        }
    }
}
