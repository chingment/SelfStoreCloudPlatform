using Lumos.BLL.Service.Term.Models;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Term
{
    public class OrderService : BaseProvider
    {
        public CustomJsonResult Reserve(string operater, OrderReservePms pms)
        {

            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                OrderReserveResult result_Data = new OrderReserveResult();

                //检查是否有可买的商品
                var skuIds = pms.Details.Select(m => m.SkuId).ToArray();

                var machineStocks = CurrentDb.MachineStock.Where(m => m.UserId == pms.UserId && m.MachineId == pms.MachineId && m.MerchantId == pms.MerchantId && skuIds.Contains(m.ProductSkuId) && m.IsOffSell == false).ToList();

                if (machineStocks.Count == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "可预定的商品为空，请删除后，选择其它商品");
                }

                //检查是否有预定的商品与库存不对应

                string skuNamesByEmpty = "";
                foreach (var skuId in skuIds)
                {
                    var machineStock = machineStocks.Where(m => m.ProductSkuId == skuId).FirstOrDefault();

                    if (machineStock == null)
                    {
                        skuNamesByEmpty += machineStock.ProductSkuName + ",";
                    }
                }


                if (!string.IsNullOrEmpty(skuNamesByEmpty))
                {
                    skuNamesByEmpty = skuNamesByEmpty.Substring(0, skuNamesByEmpty.Length - 1);

                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "可预定的商品(" + skuNamesByEmpty + ")为空，请删除后，选择其它商品");
                }

                //检查是否有预定的商品数量与库存数量不对应
                string skuNamesByQuantity = "";
                foreach (var item in pms.Details)
                {
                    var machineStock = machineStocks.Where(m => m.ProductSkuId == item.SkuId).FirstOrDefault();

                    var sellQuantity = machineStocks.Where(m => m.ProductSkuId == item.SkuId).Sum(m => m.SellQuantity);

                    if (item.Quantity > sellQuantity)
                    {
                        skuNamesByQuantity += machineStock.ProductSkuName + "最大库存：" + sellQuantity + ",";
                    }

                }

                if (!string.IsNullOrEmpty(skuNamesByQuantity))
                {
                    skuNamesByQuantity = skuNamesByQuantity.Substring(0, skuNamesByQuantity.Length - 1);

                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "可预定的商品数量不足(" + skuNamesByEmpty + ")，请删减后，再支付");
                }


                foreach (var item in pms.Details)
                {
                    var l_machineStocks = machineStocks.Where(m => m.ProductSkuId == item.SkuId).ToList();
                    var l_reserveDetails = GetReserveDetail(item, l_machineStocks);

                    result_Data.Details.AddRange(l_reserveDetails);

                    foreach (var item2 in l_reserveDetails)
                    {
                        foreach (var item3 in machineStocks)
                        {
                            if (item3.SlotId == item2.SlotId && item3.ProductSkuId == item2.SkuId)
                            {
                                item3.SellQuantity -= item2.Quantity;
                                item3.LockQuantity += item2.Quantity;


                                var machineStockLog = new MachineStockLog();
                                machineStockLog.Id = GuidUtil.New();
                                machineStockLog.UserId = item3.UserId;
                                machineStockLog.MerchantId = item3.MerchantId;
                                machineStockLog.MachineId = item3.MachineId;
                                machineStockLog.ProductSkuId = item3.ProductSkuId;
                                machineStockLog.StoreId = item3.StoreId;
                                machineStockLog.SlotId = item3.SlotId;
                                machineStockLog.ChangeType = Enumeration.MachineStockLogChangeTpye.Lock;
                                machineStockLog.ChangeQuantity = item2.Quantity;
                                machineStockLog.Quantity = item3.Quantity;
                                machineStockLog.LockQuantity = item3.LockQuantity;
                                machineStockLog.SellQuantity = item3.SellQuantity;
                                machineStockLog.CreateTime = this.DateTime;
                                machineStockLog.Creator = operater;
                                machineStockLog.RemarkByDev = string.Format("锁定库存：{0}", item2.Quantity);
                                CurrentDb.MachineStockLog.Add(machineStockLog);
                                CurrentDb.SaveChanges();
                            }
                        }
                    }
                }

                result_Data.OrderSn = "1";
                result_Data.PayUrl = "http://www.baidu.com";

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "预定成功", result_Data);

            }


            return result;

        }




        public List<OrderReserveResult.Detail> GetReserveDetail(OrderReservePms.Detail detail, List<MachineStock> machineStocks)
        {

            List<OrderReserveResult.Detail> detailsByPer = new List<OrderReserveResult.Detail>();

            foreach (var item in machineStocks)
            {

                for (var i = 0; i < item.Quantity; i++)
                {
                    int reservedQuantity = detailsByPer.Sum(m => m.Quantity);//已订的数量
                    int needReserveQuantity = detail.Quantity;//需要订的数量
                    if (reservedQuantity != needReserveQuantity)
                    {
                        detailsByPer.Add(new OrderReserveResult.Detail { Quantity = 1, SkuId = item.ProductSkuId, SlotId = item.SlotId });
                    }
                }
            }

            var list = (from c in detailsByPer
                        group c by new
                        {
                            c.SkuId,
                            c.SlotId
                        }
                           into b
                        select new
                        {
                            SkuId = b.Select(m => m.SkuId).First(),
                            SlotId = b.Select(m => m.SlotId).First(),
                            Quantity = b.Sum(p => p.Quantity),
                        }).ToList();


            List<OrderReserveResult.Detail> details = new List<OrderReserveResult.Detail>();

            foreach (var item in list)
            {
                details.Add(new OrderReserveResult.Detail { Quantity = item.Quantity, SkuId = item.SkuId, SlotId = item.SlotId });
            }

            return details;
        }

    }
}
