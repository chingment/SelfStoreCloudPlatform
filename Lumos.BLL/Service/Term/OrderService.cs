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


                var skuIds = pms.Details.Select(m => m.SkuId).ToArray();

                //检查是否有可买的商品
                List<MachineStock> skusByStock;

                //取货方式
                Enumeration.ReceptionMode receptionMode = Enumeration.ReceptionMode.Unknow;
                string receiver = null;
                string receiverPhone = null;
                string receptionAddress = null;

                //pms.MachineId 为空表示线上商城购买，不为空在线下机器购买
                if (string.IsNullOrEmpty(pms.MachineId))
                {
                    skusByStock = CurrentDb.MachineStock.Where(m => m.UserId == pms.UserId && skuIds.Contains(m.ProductSkuId)).ToList();
                }
                else
                {
                    skusByStock = CurrentDb.MachineStock.Where(m => m.UserId == pms.UserId && m.MachineId == pms.MachineId && skuIds.Contains(m.ProductSkuId)).ToList();
                }

                if (skusByStock.Count == 0)
                {
                    //todo 提示已没有可买的商品
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "可预定的商品为空，请选择商品");
                }

                //检查是否有下架的商品
                var skusByOffSell = skusByStock.Where(m => m.IsOffSell == true).ToList();
                if (skusByOffSell.Count > 0)
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

                var order = new Order();
                order.Id = GuidUtil.New();
                order.Sn = SnUtil.Build(Enumeration.BizSnType.Order, pms.UserId);
                order.UserId = pms.UserId;
                order.StoreId = pms.StoreId;
                order.OriginalAmount = 0;
                order.DiscountAmount = 0;
                order.ChargeAmount = 0;
                order.Status = Enumeration.OrderStatus.WaitPay;
                order.SubmitTime = this.DateTime;
                order.Creator = pOperater;
                order.CreateTime = this.DateTime;

                var reserveDetails = new List<OrderReserveResult.Detail>();

                foreach (var detail in pms.Details)
                {
                    var skuByStock = skusByStock.Where(m => m.ProductSkuId == detail.SkuId).ToList();
                    var reserveDetail = GetReserveDetail(detail, skuByStock);
                    reserveDetails.AddRange(reserveDetail);
                }


                string[] machineIds = reserveDetails.Select(m => m.MachineId).Distinct<string>().ToArray();

                foreach (var machineId in machineIds)
                {
                    var l_ReserveDetail = reserveDetails.Where(m => m.MachineId == machineId).ToList();

                    var orderDetails = new OrderDetails();
                    orderDetails.Id = GuidUtil.New();
                    orderDetails.Sn = SnUtil.Build(Enumeration.BizSnType.Order, pms.UserId);
                    orderDetails.UserId = pms.UserId;
                    orderDetails.StoreId = pms.StoreId;
                    orderDetails.MachineId = machineId;
                    orderDetails.OrderId = order.Id;
                    orderDetails.OrderSn = order.Sn;
                    orderDetails.OriginalAmount = 0;
                    orderDetails.DiscountAmount = 0;
                    orderDetails.ChargeAmount = 0;
                    orderDetails.Quantity = l_ReserveDetail.Sum(m => m.Quantity);
                    orderDetails.Receiver = receiver;
                    orderDetails.ReceiverPhone = receiverPhone;
                    orderDetails.ReceptionAddress = receptionAddress;
                    orderDetails.ReceptionMode = receptionMode;
                    orderDetails.SubmitTime = this.DateTime;
                    orderDetails.Creator = pOperater;
                    orderDetails.CreateTime = this.DateTime;
                    CurrentDb.OrderDetails.Add(orderDetails);


                    var skuIdsByReserve = l_ReserveDetail.Select(m => m.SkuId).Distinct<string>().ToList();

                    foreach (var skuId in skuIdsByReserve)
                    {

                        var orderDetailsChild = new OrderDetailsChild();
                        orderDetailsChild.Id = GuidUtil.New();
                        orderDetailsChild.UserId = pms.UserId;
                        orderDetailsChild.StoreId = pms.StoreId;
                        orderDetailsChild.MachineId = machineId;
                        orderDetailsChild.OrderId = order.Id;
                        orderDetailsChild.OrderSn = order.Sn;
                        orderDetailsChild.OrderDetailsId = orderDetails.Id;
                        orderDetailsChild.OrderDetailsSn = orderDetails.Sn;
                        orderDetailsChild.ProductSkuId = skuId;
                        orderDetailsChild.SalesPrice = 0;
                        orderDetailsChild.Quantity = l_ReserveDetail.Where(m => m.SkuId == skuId).Sum(m => m.Quantity);
                        orderDetailsChild.OriginalAmount = 0;
                        orderDetailsChild.DiscountAmount = 0;
                        orderDetailsChild.ChargeAmount = 0;
                        orderDetailsChild.SubmitTime = this.DateTime;
                        orderDetailsChild.Creator = pOperater;
                        orderDetailsChild.CreateTime = this.DateTime;
                        CurrentDb.OrderDetailsChild.Add(orderDetailsChild);
                    }


                    foreach (var reserveDetail in l_ReserveDetail)
                    {
                        var machineStock = skusByStock.Where(m => m.ProductSkuId == reserveDetail.SkuId && m.SlotId == reserveDetail.SlotId && m.MachineId == reserveDetail.MachineId).FirstOrDefault();

                        machineStock.LockQuantity += reserveDetail.Quantity;
                        machineStock.SellQuantity -= reserveDetail.Quantity;
                        machineStock.Mender = pOperater;
                        machineStock.MendTime = this.DateTime;


                        //var orderDetailsChild = new OrderDetailsChild();
                        //orderDetailsChild.Id = GuidUtil.New();
                        //orderDetailsChild.UserId = pms.UserId;
                        //orderDetailsChild.StoreId = pms.StoreId;
                        //orderDetailsChild.MachineId=


                        var machineStockLog = new MachineStockLog();
                        machineStockLog.Id = GuidUtil.New();
                        machineStockLog.UserId = pms.UserId;
                        machineStockLog.StoreId = pms.StoreId;
                        machineStockLog.MachineId = reserveDetail.MachineId;
                        machineStockLog.SlotId = reserveDetail.SlotId;
                        machineStockLog.ProductSkuId = reserveDetail.SkuId;
                        machineStockLog.Quantity = machineStock.Quantity;
                        machineStockLog.LockQuantity = machineStock.LockQuantity;
                        machineStockLog.SellQuantity = machineStock.SellQuantity;
                        machineStockLog.ChangeType = Enumeration.MachineStockLogChangeTpye.Lock;
                        machineStockLog.ChangeQuantity = reserveDetail.Quantity;
                        machineStockLog.Creator = pOperater;
                        machineStockLog.CreateTime = this.DateTime;
                        machineStockLog.RemarkByDev = string.Format("预定锁定库存：{0}", reserveDetail.Quantity);
                        CurrentDb.MachineStockLog.Add(machineStockLog);
                    }

                }






                result_Data.OrderSn = "1";
                result_Data.PayUrl = "http://www.baidu.com";

                ///CurrentDb.Order.Add(order);
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
                        childDetail.Add(new OrderReserveResult.ChildDetail { MachineId = item.MachineId, Quantity = 1, SkuId = item.ProductSkuId, SlotId = item.SlotId });
                    }
                }
            }

            var detailsGroup = (from c in childDetail
                                group c by new
                                {
                                    c.MachineId,
                                    c.SkuId,
                                    c.SlotId
                                }
                           into b
                                select new
                                {
                                    MachineId = b.Select(m => m.MachineId).First(),
                                    SkuId = b.Select(m => m.SkuId).First(),
                                    SlotId = b.Select(m => m.SlotId).First(),
                                    Quantity = b.Sum(p => p.Quantity),
                                }).ToList();


            List<OrderReserveResult.Detail> details = new List<OrderReserveResult.Detail>();

            foreach (var item in detailsGroup)
            {
                var detail = new OrderReserveResult.Detail();

                detail.MachineId = item.MachineId;
                detail.SkuId = item.SkuId;
                detail.SlotId = item.SlotId;
                detail.Quantity = item.Quantity;
                detail.Details = childDetail.Where(m => m.MachineId == item.MachineId && m.SkuId == item.SkuId && m.SlotId == item.SlotId).ToList();

                details.Add(detail);
            }

            return details;
        }

    }
}
