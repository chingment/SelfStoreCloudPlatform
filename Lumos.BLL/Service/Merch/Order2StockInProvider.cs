using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class Order2StockInProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetOrder2StockInGetDetails();


            var order2StockIn = CurrentDb.Order2StockIn.Where(m => m.Id == id).FirstOrDefault();

            if (order2StockIn != null)
            {
                ret.Id = order2StockIn.Id;
                ret.Sn = order2StockIn.Sn;
                ret.StockInTime = order2StockIn.StockInTime.ToUnifiedFormatDateTime();
                ret.WarehouseName = order2StockIn.WarehouseName;
                ret.SupplierName = order2StockIn.SupplierName;
                ret.Description = order2StockIn.Description;
                ret.Amount = order2StockIn.Amount;
                ret.Quantity = order2StockIn.Quantity;

                var order2StockInDetails = CurrentDb.Order2StockInDetails.Where(m => m.Order2StockInId == order2StockIn.Id).ToList();

                foreach (var item in order2StockInDetails)
                {
                    ret.Skus.Add(new RetOrder2StockInGetDetails.Sku { SkuId = item.ProductSkuId, BarCode = item.ProductSkuBarCode, Name = item.ProductSkuName, Amount = item.Amount, Quantity = item.Quantity });
                }
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string operater, string merchantId, RopOrder2StockInAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var warehouse = CurrentDb.Warehouse.Where(m => m.Id == rop.WarehouseId).FirstOrDefault();
                var supplier = CurrentDb.Company.Where(m => m.Id == rop.SupplierId).FirstOrDefault();

                var order2StockIn = new Order2StockIn();
                order2StockIn.Id = GuidUtil.New();
                order2StockIn.MerchantId = merchantId;
                order2StockIn.Sn = SnUtil.Build(Enumeration.BizSnType.Order2StockIn, order2StockIn.MerchantId);
                order2StockIn.Quantity = rop.Skus.Select(m => m.Quantity).Sum();
                order2StockIn.Amount = rop.Skus.Select(m => m.Amount).Sum();
                order2StockIn.WarehouseId = warehouse.Id;
                order2StockIn.WarehouseName = warehouse.Name;
                order2StockIn.SupplierId = supplier.Id;
                order2StockIn.SupplierName = supplier.Name;
                order2StockIn.StockInTime = rop.StockInTime;
                order2StockIn.Description = rop.Description;
                order2StockIn.Creator = operater;
                order2StockIn.CreateTime = this.DateTime;
                CurrentDb.Order2StockIn.Add(order2StockIn);
                CurrentDb.SaveChanges();

                foreach (var sku in rop.Skus)
                {
                    var productSKu = CurrentDb.ProductSku.Where(m => m.Id == sku.SkuId).FirstOrDefault();

                    var order2StockInDetails = new Order2StockInDetails();
                    order2StockInDetails.Id = GuidUtil.New();
                    order2StockInDetails.MerchantId = merchantId;
                    order2StockInDetails.Order2StockInId = order2StockIn.Id;
                    order2StockInDetails.WarehouseId = order2StockIn.WarehouseId;
                    order2StockInDetails.WarehouseName = order2StockIn.WarehouseName;
                    order2StockInDetails.SupplierId = order2StockIn.SupplierId;
                    order2StockInDetails.SupplierName = order2StockIn.SupplierName;
                    order2StockInDetails.StockInTime = order2StockIn.StockInTime;
                    order2StockInDetails.Creator = order2StockIn.Creator;
                    order2StockInDetails.CreateTime = order2StockIn.CreateTime;
                    order2StockInDetails.ProductSkuId = productSKu.Id;
                    order2StockInDetails.ProductSkuBarCode = productSKu.BarCode;
                    order2StockInDetails.ProductSkuName = productSKu.Name;
                    order2StockInDetails.Quantity = sku.Quantity;
                    order2StockInDetails.Amount = sku.Amount;
                    CurrentDb.Order2StockInDetails.Add(order2StockInDetails);
                    CurrentDb.SaveChanges();


                    var warehouseStock = CurrentDb.WarehouseStock.Where(m => m.MerchantId == merchantId && m.WarehouseId == order2StockIn.WarehouseId && m.ProductSkuId == sku.SkuId).FirstOrDefault();

                    if (warehouseStock == null)
                    {
                        warehouseStock = new WarehouseStock();
                        warehouseStock.Id = GuidUtil.New();
                        warehouseStock.MerchantId = order2StockInDetails.MerchantId;
                        warehouseStock.WarehouseId = order2StockInDetails.WarehouseId;
                        warehouseStock.WarehouseName = order2StockInDetails.WarehouseName;
                        warehouseStock.ProductSkuId = order2StockInDetails.ProductSkuId;
                        warehouseStock.ProductSkuName = order2StockInDetails.ProductSkuName;
                        warehouseStock.Quantity = order2StockInDetails.Quantity;
                        warehouseStock.CreateTime = order2StockInDetails.CreateTime;
                        warehouseStock.Creator = order2StockInDetails.Creator;
                        CurrentDb.WarehouseStock.Add(warehouseStock);
                        CurrentDb.SaveChanges();
                    }
                    else
                    {
                        warehouseStock.Quantity += order2StockInDetails.Quantity;
                        warehouseStock.MendTime = order2StockInDetails.CreateTime;
                        warehouseStock.Mender = order2StockInDetails.Creator;
                    }


                    var warehouseStockLog = new WarehouseStockLog();
                    warehouseStockLog.Id = GuidUtil.New();
                    warehouseStockLog.MerchantId = order2StockInDetails.MerchantId;
                    warehouseStockLog.WarehouseId = order2StockInDetails.WarehouseId;
                    warehouseStockLog.WarehouseName = order2StockInDetails.WarehouseName;
                    warehouseStockLog.ProductSkuId = order2StockInDetails.ProductSkuId;
                    warehouseStockLog.ProductSkuName = order2StockInDetails.ProductSkuName;
                    warehouseStockLog.Quantity = warehouseStock.Quantity;
                    warehouseStockLog.ChangeType = Enumeration.WarehouseStockLogChangeTpye.StockIn;
                    warehouseStockLog.ChangeQuantity = order2StockInDetails.Quantity;
                    warehouseStockLog.CreateTime = order2StockInDetails.CreateTime;
                    warehouseStockLog.Creator = order2StockInDetails.Creator;
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
