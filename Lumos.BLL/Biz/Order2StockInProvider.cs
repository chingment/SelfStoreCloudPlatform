using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class Order2StockInProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, Order2StockIn pOrder2StockIn, List<Order2StockInDetails> pOrder2StockInDetails)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lWarehouse = CurrentDb.Warehouse.Where(m => m.Id == pOrder2StockIn.WarehouseId).FirstOrDefault();
                var lSupplier = CurrentDb.Company.Where(m => m.Id == pOrder2StockIn.SupplierId).FirstOrDefault();

                pOrder2StockIn.Id = GuidUtil.New();
                pOrder2StockIn.Sn = SnUtil.Build(Enumeration.BizSnType.Order2StockIn, pOrder2StockIn.UserId);
                pOrder2StockIn.Quantity = pOrder2StockInDetails.Select(m => m.Quantity).Sum();
                pOrder2StockIn.Amount = pOrder2StockInDetails.Select(m => m.Amount).Sum();
                pOrder2StockIn.WarehouseName = lWarehouse.Name;
                pOrder2StockIn.SupplierName = lSupplier.Name;
                pOrder2StockIn.Creator = pOperater;
                pOrder2StockIn.CreateTime = this.DateTime;
                CurrentDb.Order2StockIn.Add(pOrder2StockIn);
                CurrentDb.SaveChanges();

                foreach (var item in pOrder2StockInDetails)
                {
                    var productSKu = CurrentDb.ProductSku.Where(m => m.Id == item.ProductSkuId).FirstOrDefault();

                    item.Id = GuidUtil.New();
                    item.UserId = pOrder2StockIn.UserId;
                    item.Order2StockInId = pOrder2StockIn.Id;
                    item.WarehouseId = pOrder2StockIn.WarehouseId;
                    item.WarehouseName = pOrder2StockIn.WarehouseName;
                    item.SupplierId = pOrder2StockIn.SupplierId;
                    item.SupplierName = pOrder2StockIn.SupplierName;
                    item.StockInTime = pOrder2StockIn.StockInTime;
                    item.Creator = pOrder2StockIn.Creator;
                    item.CreateTime = pOrder2StockIn.CreateTime;
                    item.ProductSkuName = productSKu.Name;
                    CurrentDb.Order2StockInDetails.Add(item);
                    CurrentDb.SaveChanges();


                    var warehouseStock = CurrentDb.WarehouseStock.Where(m => m.UserId == item.UserId && m.WarehouseId == item.WarehouseId && m.ProductSkuId == item.ProductSkuId).FirstOrDefault();

                    if (warehouseStock == null)
                    {
                        warehouseStock = new WarehouseStock();
                        warehouseStock.Id = GuidUtil.New();
                        warehouseStock.UserId = item.UserId;
                        warehouseStock.WarehouseId = item.WarehouseId;
                        warehouseStock.WarehouseName = item.WarehouseName;
                        warehouseStock.ProductSkuId = item.ProductSkuId;
                        warehouseStock.ProductSkuName = item.ProductSkuName;
                        warehouseStock.Quantity = item.Quantity;
                        warehouseStock.CreateTime = item.CreateTime;
                        warehouseStock.Creator = item.Creator;
                        CurrentDb.WarehouseStock.Add(warehouseStock);
                        CurrentDb.SaveChanges();
                    }
                    else
                    {
                        warehouseStock.Quantity += item.Quantity;
                        warehouseStock.MendTime = item.CreateTime;
                        warehouseStock.Mender = item.Creator;
                    }


                    var warehouseStockLog = new WarehouseStockLog();
                    warehouseStockLog.Id = GuidUtil.New();
                    warehouseStockLog.UserId = item.UserId;
                    warehouseStockLog.WarehouseId = item.WarehouseId;
                    warehouseStockLog.WarehouseName = item.WarehouseName;
                    warehouseStockLog.ProductSkuId = item.ProductSkuId;
                    warehouseStockLog.ProductSkuName = item.ProductSkuName;
                    warehouseStockLog.Quantity = warehouseStock.Quantity;
                    warehouseStockLog.ChangeType = Enumeration.WarehouseStockLogChangeTpye.StockIn;
                    warehouseStockLog.ChangeQuantity = item.Quantity;
                    warehouseStockLog.CreateTime = item.CreateTime;
                    warehouseStockLog.Creator = item.Creator;
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
