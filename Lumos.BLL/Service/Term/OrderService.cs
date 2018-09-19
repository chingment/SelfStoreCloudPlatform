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
    public class SkuByUnderStock
    {
        public string SkuId { get; set; }
        public int ReserveQuantity { get; set; }
        public int SellQuantity { get; set; }
    }

    public class OrderService : BaseProvider
    {
        public CustomJsonResult Reserve(string pOperater, OrderReservePms pms)
        {

            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                OrderReserveResult result_Data = new OrderReserveResult();

                //检查是否有可买的商品
                var skuIds = pms.Details.Select(m => m.SkuId).ToArray();

                var skusByStock = CurrentDb.MachineStock.Where(m => m.UserId == pms.UserId && m.MachineId == pms.MachineId && skuIds.Contains(m.ProductSkuId)).ToList();

                if (skusByStock.Count == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "可预定的商品为空，请选择商品");
                }

                var skusByoffSell = skusByStock.Where(m => m.IsOffSell == true).ToList();
                if (skusByStock.Count > 0)
                {
                    //todo 提示已下架的商品
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "可预定的商品已经下架");
                }

                //检查是否有预定的商品数量与库存数量不对应

                var skusByUnderStock = new List<SkuByUnderStock>();
                foreach (var item in pms.Details)
                {
                    var sellQuantity = skusByStock.Where(m => m.ProductSkuId == item.SkuId).Sum(m => m.SellQuantity);
                    if (item.Quantity > sellQuantity)
                    {
                        var skuByUnderStock = new SkuByUnderStock();
                        skuByUnderStock.SkuId = item.SkuId;
                        skuByUnderStock.ReserveQuantity = item.Quantity;
                        skuByUnderStock.SellQuantity = sellQuantity;
                        skusByUnderStock.Add(skuByUnderStock);
                    }
                }

                if (skusByUnderStock.Count > 0)
                {
                    //todo 提示库存不足
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "可预定的商品数量不足，再支付");
                }


                foreach (var item in pms.Details)
                {
                    var skuByStock = skusByStock.Where(m => m.ProductSkuId == item.SkuId).ToList();
                    var l_reserveDetails = GetReserveDetail(item, skuByStock);

                    //foreach (var item2 in l_reserveDetails)
                    //{
                    //    foreach (var item3 in skusByReserve)
                    //    {
                    //        if (item3.SlotId == item2.SlotId && item3.ProductSkuId == item2.SkuId)
                    //        {
                    //            item3.SellQuantity -= item2.Quantity;
                    //            item3.LockQuantity += item2.Quantity;


                    //            var machineStockLog = new MachineStockLog();
                    //            machineStockLog.Id = GuidUtil.New();
                    //            machineStockLog.UserId = item3.UserId;
                    //            machineStockLog.MachineId = item3.MachineId;
                    //            machineStockLog.ProductSkuId = item3.ProductSkuId;
                    //            machineStockLog.StoreId = item3.StoreId;
                    //            machineStockLog.SlotId = item3.SlotId;
                    //            machineStockLog.ChangeType = Enumeration.MachineStockLogChangeTpye.Lock;
                    //            machineStockLog.ChangeQuantity = item2.Quantity;
                    //            machineStockLog.Quantity = item3.Quantity;
                    //            machineStockLog.LockQuantity = item3.LockQuantity;
                    //            machineStockLog.SellQuantity = item3.SellQuantity;
                    //            machineStockLog.CreateTime = this.DateTime;
                    //            machineStockLog.Creator = pOperater;
                    //            machineStockLog.RemarkByDev = string.Format("锁定库存：{0}", item2.Quantity);
                    //            CurrentDb.MachineStockLog.Add(machineStockLog);
                    //            CurrentDb.SaveChanges();
                    //        }
                    //    }
                    //}
                }

                result_Data.OrderSn = "1";
                result_Data.PayUrl = "http://www.baidu.com";

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "预定成功", result_Data);

            }


            return result;

        }




        public List<OrderReserveResult.Detail> GetReserveDetail(OrderReservePms.Detail reserveDetail, List<MachineStock> machineStocks)
        {

            List<OrderReserveResult.ChildDetail> childDetail = new List<OrderReserveResult.ChildDetail>();

            foreach (var item in machineStocks)
            {

                for (var i = 0; i < item.Quantity; i++)
                {
                    int reservedQuantity = childDetail.Sum(m => m.Quantity);//已订的数量
                    int needReserveQuantity = reserveDetail.Quantity;//需要订的数量
                    if (reservedQuantity != needReserveQuantity)
                    {
                        childDetail.Add(new OrderReserveResult.ChildDetail { Quantity = 1, SkuId = item.ProductSkuId, SlotId = item.SlotId });
                    }
                }
            }

            var detailsGroup = (from c in childDetail
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

            foreach (var item in detailsGroup)
            {
                var detail = new OrderReserveResult.Detail();


                detail.SkuId = item.SkuId;
                detail.SlotId = item.SlotId;
                detail.Quantity = item.Quantity;
                detail.Details = childDetail.Where(m => m.SkuId == item.SkuId && m.SlotId == item.SlotId).ToList();

                details.Add(detail);
            }

            return details;
        }

    }
}
