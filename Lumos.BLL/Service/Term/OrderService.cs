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
        public CustomJsonResult Reserve(string pOperater, RopOrderReserve rop)
        {

            CustomJsonResult result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                RetOrderReserve ret = new RetOrderReserve();


                var skuIds = rop.Details.Select(m => m.SkuId).ToArray();

                //检查是否有可买的商品
                List<MachineStock> skusByStock;

                //取货方式
                Enumeration.ReceptionMode receptionMode = Enumeration.ReceptionMode.Unknow;

                //pms.MachineId 为空表示线上商城购买，不为空在线下机器购买
                if (string.IsNullOrEmpty(rop.MachineId))
                {
                    skusByStock = CurrentDb.MachineStock.Where(m => m.UserId == rop.UserId && skuIds.Contains(m.ProductSkuId)).ToList();
                }
                else
                {
                    skusByStock = CurrentDb.MachineStock.Where(m => m.UserId == rop.UserId && m.MachineId == rop.MachineId && skuIds.Contains(m.ProductSkuId)).ToList();
                }

                if (skusByStock.Count == 0)
                {
                    string tips = "";
                    foreach (var item in skusByStock)
                    {
                        var skuModel = BizFactory.ProductSku.GetModel(item.ProductSkuId);

                        tips += skuModel.Name + "、";
                    }

                    tips = tips.Substring(0, tips.Length - 1);

                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, tips + ",可预定数量不足");
                }

                //检查是否有下架的商品
                var skusByOffSell = skusByStock.Where(m => m.IsOffSell == true).ToList();
                if (skusByOffSell.Count > 0)
                {
                    string tips = "";
                    foreach (var item in skusByOffSell)
                    {
                        var skuModel = BizFactory.ProductSku.GetModel(item.ProductSkuId);

                        tips += skuModel.Name + "、";
                    }

                    tips = tips.Substring(0, tips.Length - 1);
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, tips + "已经下架");
                }

                //检查是否有预定的商品数量与库存数量不对应
                var skusByUnderStock = new List<SkuByUnderStock>();
                foreach (var item in rop.Details)
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
                    string tips = "";
                    foreach (var item in skusByUnderStock)
                    {
                        var skuModel = BizFactory.ProductSku.GetModel(item.SkuId);

                        tips += skuModel.Name + "的最大库存为" + item.SellQuantity + ";";
                    }

                    tips = tips.Substring(0, tips.Length - 1);
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, tips);
                }

                var store = CurrentDb.Store.Where(m => m.Id == rop.StoreId).FirstOrDefault();

                var order = new Order();
                order.Id = GuidUtil.New();
                order.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.UserId);
                order.UserId = rop.UserId;
                order.StoreId = rop.StoreId;
                order.Quantity = rop.Details.Sum(m => m.Quantity);
                order.Status = Enumeration.OrderStatus.WaitPay;
                order.SubmitTime = this.DateTime;
                order.Creator = pOperater;
                order.CreateTime = this.DateTime;

                //var discount50;
                var reserveDetails = GetReserveDetail(rop.Details, skusByStock);

                order.OriginalAmount = reserveDetails.Sum(m => m.OriginalAmount);
                order.DiscountAmount = reserveDetails.Sum(m => m.DiscountAmount);
                order.ChargeAmount = reserveDetails.Sum(m => m.ChargeAmount);

                foreach (var detail in reserveDetails)
                {
                    var orderDetails = new OrderDetails();
                    orderDetails.Id = GuidUtil.New();
                    orderDetails.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.UserId);
                    orderDetails.UserId = rop.UserId;
                    orderDetails.StoreId = rop.StoreId;
                    orderDetails.MachineId = detail.MachineId;
                    orderDetails.OrderId = order.Id;
                    orderDetails.OrderSn = order.Sn;
                    orderDetails.OriginalAmount = detail.OriginalAmount;
                    orderDetails.DiscountAmount = detail.DiscountAmount;
                    orderDetails.ChargeAmount = detail.ChargeAmount;
                    orderDetails.Quantity = detail.Quantity;
                    orderDetails.ReceptionMode = receptionMode;
                    orderDetails.SubmitTime = this.DateTime;
                    orderDetails.Creator = pOperater;
                    orderDetails.CreateTime = this.DateTime;

                    //detail.MachineId为空 则为快递商品
                    if (string.IsNullOrEmpty(detail.MachineId))
                    {
                        orderDetails.Receiver = rop.Receiver;
                        orderDetails.ReceiverPhone = rop.ReceiverPhone;
                        orderDetails.ReceptionAddress = rop.ReceptionAddress;
                    }
                    else
                    {
                        orderDetails.Receiver = null;
                        orderDetails.ReceiverPhone = null;
                        orderDetails.ReceptionAddress = store.Address;
                    }

                    CurrentDb.OrderDetails.Add(orderDetails);

                    foreach (var detailsChild in detail.Details)
                    {

                        var orderDetailsChild = new OrderDetailsChild();
                        orderDetailsChild.Id = GuidUtil.New();
                        orderDetailsChild.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.UserId);
                        orderDetailsChild.UserId = rop.UserId;
                        orderDetailsChild.StoreId = rop.StoreId;
                        orderDetailsChild.MachineId = detailsChild.MachineId;
                        orderDetailsChild.OrderId = order.Id;
                        orderDetailsChild.OrderSn = order.Sn;
                        orderDetailsChild.OrderDetailsId = orderDetails.Id;
                        orderDetailsChild.OrderDetailsSn = orderDetails.Sn;
                        orderDetailsChild.ProductSkuId = detailsChild.SkuId;
                        orderDetailsChild.ProductSkuName = detailsChild.SkuName;
                        orderDetailsChild.ProductSkuImgUrl = detailsChild.SkuImgUrl;
                        orderDetailsChild.SalesPrice = detailsChild.SalesPrice;
                        orderDetailsChild.Quantity = detailsChild.Quantity;
                        orderDetailsChild.OriginalAmount = detailsChild.OriginalAmount;
                        orderDetailsChild.DiscountAmount = detailsChild.DiscountAmount;
                        orderDetailsChild.ChargeAmount = detailsChild.ChargeAmount;
                        orderDetailsChild.SubmitTime = this.DateTime;
                        orderDetailsChild.Creator = pOperater;
                        orderDetailsChild.CreateTime = this.DateTime;
                        CurrentDb.OrderDetailsChild.Add(orderDetailsChild);

                        foreach (var detailsChildSon in detailsChild.Details)
                        {
                            var orderDetailsChildSon = new OrderDetailsChildSon();

                            orderDetailsChildSon.Id = detailsChildSon.Id;
                            orderDetailsChildSon.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.UserId);
                            orderDetailsChildSon.UserId = rop.UserId;
                            orderDetailsChildSon.StoreId = rop.StoreId;
                            orderDetailsChildSon.MachineId = detailsChildSon.MachineId;
                            orderDetailsChildSon.OrderId = order.Id;
                            orderDetailsChildSon.OrderSn = order.Sn;
                            orderDetailsChildSon.OrderDetailsId = orderDetails.Id;
                            orderDetailsChildSon.OrderDetailsSn = orderDetails.Sn;
                            orderDetailsChildSon.OrderDetailsChildId = orderDetailsChild.Id;
                            orderDetailsChildSon.OrderDetailsChildSn = orderDetailsChild.Sn;
                            orderDetailsChildSon.SlotId = detailsChildSon.SlotId;
                            orderDetailsChildSon.ProductSkuId = detailsChildSon.SkuId;
                            orderDetailsChildSon.ProductSkuName = detailsChildSon.SkuName;
                            orderDetailsChildSon.ProductSkuImgUrl = detailsChildSon.SkuImgUrl;
                            orderDetailsChildSon.SalesPrice = detailsChildSon.SalesPrice;
                            orderDetailsChildSon.Quantity = detailsChildSon.Quantity;
                            orderDetailsChildSon.OriginalAmount = detailsChildSon.OriginalAmount;
                            orderDetailsChildSon.DiscountAmount = detailsChildSon.DiscountAmount;
                            orderDetailsChildSon.ChargeAmount = detailsChildSon.ChargeAmount;
                            orderDetailsChildSon.SubmitTime = this.DateTime;
                            orderDetailsChildSon.Creator = pOperater;
                            orderDetailsChildSon.CreateTime = this.DateTime;
                            CurrentDb.OrderDetailsChildSon.Add(orderDetailsChildSon);
                        }



                        foreach (var slotStock in detailsChild.SlotStock)
                        {
                            var machineStock = skusByStock.Where(m => m.ProductSkuId == slotStock.SkuId && m.SlotId == slotStock.SlotId && m.MachineId == slotStock.MachineId).FirstOrDefault();

                            machineStock.LockQuantity += slotStock.Quantity;
                            machineStock.SellQuantity -= slotStock.Quantity;
                            machineStock.Mender = pOperater;
                            machineStock.MendTime = this.DateTime;


                            var machineStockLog = new MachineStockLog();
                            machineStockLog.Id = GuidUtil.New();
                            machineStockLog.UserId = rop.UserId;
                            machineStockLog.StoreId = rop.StoreId;
                            machineStockLog.MachineId = slotStock.MachineId;
                            machineStockLog.SlotId = slotStock.SlotId;
                            machineStockLog.ProductSkuId = slotStock.SkuId;
                            machineStockLog.Quantity = machineStock.Quantity;
                            machineStockLog.LockQuantity = machineStock.LockQuantity;
                            machineStockLog.SellQuantity = machineStock.SellQuantity;
                            machineStockLog.ChangeType = Enumeration.MachineStockLogChangeTpye.Lock;
                            machineStockLog.ChangeQuantity = slotStock.Quantity;
                            machineStockLog.Creator = pOperater;
                            machineStockLog.CreateTime = this.DateTime;
                            machineStockLog.RemarkByDev = string.Format("预定锁定库存：{0}", slotStock.Quantity);
                            CurrentDb.MachineStockLog.Add(machineStockLog);
                        }
                    }
                }

                order.PayExpireTime = this.DateTime.AddMinutes(5);

                var ret_UnifiedOrder = SdkFactory.Wx.Instance().UnifiedOrder(pOperater, order.Sn, order.ChargeAmount, "", Common.CommonUtils.GetIP(), "自助商品", order.PayExpireTime.Value);

                if (string.IsNullOrEmpty(ret_UnifiedOrder.CodeUrl))
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "支付二维码生成失败");
                }

                order.PayPrepayId = ret_UnifiedOrder.PrepayId;
                order.PayQrCodeUrl = ret_UnifiedOrder.CodeUrl;

                CurrentDb.Order.Add(order);
                CurrentDb.SaveChanges(true);

                ret.OrderSn = order.Sn;
                ret.PayQrCodeUrl = order.PayQrCodeUrl;

                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "预定成功", ret);

            }


            return result;

        }

        public List<OrderReserveDetail> GetReserveDetail(List<RopOrderReserve.Detail> reserveDetails, List<MachineStock> machineStocks)
        {
            List<OrderReserveDetail> details = new List<OrderReserveDetail>();

            List<OrderReserveDetail.DetailChildSon> detailChildSons = new List<OrderReserveDetail.DetailChildSon>();

            foreach (var reserveDetail in reserveDetails)
            {
                var l_machineStocks = machineStocks.Where(m => m.ProductSkuId == reserveDetail.SkuId).ToList();

                //bool isFlag = false;
                foreach (var item in l_machineStocks)
                {
                    for (var i = 0; i < item.SellQuantity; i++)
                    {
                        int reservedQuantity = detailChildSons.Where(m => m.SkuId == reserveDetail.SkuId).Sum(m => m.Quantity);//已订的数量
                        int needReserveQuantity = reserveDetail.Quantity;//需要订的数量
                        if (reservedQuantity != needReserveQuantity)
                        {

                            var product = BizFactory.ProductSku.GetModel(item.ProductSkuId);

                            var detailChildSon = new OrderReserveDetail.DetailChildSon();
                            detailChildSon.Id = GuidUtil.New();
                            detailChildSon.MachineId = item.MachineId;
                            detailChildSon.SkuId = item.ProductSkuId;
                            detailChildSon.SkuName = product.Name;
                            detailChildSon.SkuImgUrl = product.ImgUrl;
                            detailChildSon.SlotId = item.SlotId;
                            detailChildSon.Quantity = 1;
                            detailChildSon.SalesPrice = item.SalesPrice;
                            detailChildSon.OriginalAmount = detailChildSon.Quantity * item.SalesPrice;

                            detailChildSons.Add(detailChildSon);
                        }
                    }
                }
            }

            var sumOriginalAmount = detailChildSons.Sum(m => m.OriginalAmount);
            var sumDiscountAmount = 0m;
            for (int i = 0; i < detailChildSons.Count; i++)
            {
                decimal scale = (detailChildSons[i].OriginalAmount / sumOriginalAmount);
                detailChildSons[i].DiscountAmount = Decimal.Round(scale * sumDiscountAmount, 2);
                detailChildSons[i].ChargeAmount = detailChildSons[i].OriginalAmount - detailChildSons[i].DiscountAmount;
            }

            var sumDiscountAmount2 = detailChildSons.Sum(m => m.DiscountAmount);
            if (sumDiscountAmount != sumDiscountAmount2)
            {
                var diff = sumDiscountAmount - sumDiscountAmount2;

                detailChildSons[detailChildSons.Count - 1].DiscountAmount = detailChildSons[detailChildSons.Count - 1].DiscountAmount + diff;
                detailChildSons[detailChildSons.Count - 1].ChargeAmount = detailChildSons[detailChildSons.Count - 1].OriginalAmount - detailChildSons[detailChildSons.Count - 1].DiscountAmount;
            }


            var detailGroups = (from c in detailChildSons
                                select new
                                {
                                    c.MachineId
                                }).Distinct().ToList();



            foreach (var detailGroup in detailGroups)
            {
                var detail = new OrderReserveDetail();

                detail.MachineId = detailGroup.MachineId;
                detail.Quantity = detailChildSons.Where(m => m.MachineId == detailGroup.MachineId).Sum(m => m.Quantity);
                detail.OriginalAmount = detailChildSons.Where(m => m.MachineId == detailGroup.MachineId).Sum(m => m.OriginalAmount);
                detail.DiscountAmount = detailChildSons.Where(m => m.MachineId == detailGroup.MachineId).Sum(m => m.DiscountAmount);
                detail.ChargeAmount = detailChildSons.Where(m => m.MachineId == detailGroup.MachineId).Sum(m => m.ChargeAmount);


                var detailChildGroups = (from c in detailChildSons
                                         where c.MachineId == detailGroup.MachineId
                                         select new
                                         {
                                             c.MachineId,
                                             c.SkuId
                                         }).Distinct().ToList();

                foreach (var detailChildGroup in detailChildGroups)
                {

                    var detailChild = new OrderReserveDetail.DetailChild();

                    detailChild.MachineId = detailChildGroup.MachineId;
                    detailChild.SkuId = detailChildGroup.SkuId;
                    detailChild.SkuName = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).First().SkuName;
                    detailChild.SkuImgUrl = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).First().SkuImgUrl;
                    detailChild.SalesPrice = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).First().SalesPrice;
                    detailChild.Quantity = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.Quantity);
                    detailChild.OriginalAmount = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.OriginalAmount);
                    detailChild.DiscountAmount = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.DiscountAmount);
                    detailChild.ChargeAmount = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.ChargeAmount);

                    var detailChildSonGroups = (from c in detailChildSons
                                                where c.MachineId == detailChildGroup.MachineId
                                             && c.SkuId == detailChildGroup.SkuId
                                                select new
                                                {
                                                    c.Id,
                                                    c.MachineId,
                                                    c.SkuId,
                                                    c.SlotId,
                                                    c.Quantity,
                                                    c.SalesPrice,
                                                    c.SkuImgUrl,
                                                    c.SkuName,
                                                    c.ChargeAmount,
                                                    c.DiscountAmount,
                                                    c.OriginalAmount
                                                }).ToList();


                    foreach (var detailChildSonGroup in detailChildSonGroups)
                    {
                        var detailChildSon = new OrderReserveDetail.DetailChildSon();
                        detailChildSon.Id = detailChildSonGroup.Id;
                        detailChildSon.MachineId = detailChildSonGroup.MachineId;
                        detailChildSon.SkuId = detailChildSonGroup.SkuId;
                        detailChildSon.SlotId = detailChildSonGroup.SlotId;
                        detailChildSon.Quantity = detailChildSonGroup.Quantity;
                        detailChildSon.SkuName = detailChildSonGroup.SkuName;
                        detailChildSon.SkuImgUrl = detailChildSonGroup.SkuImgUrl;
                        detailChildSon.SalesPrice = detailChildSonGroup.SalesPrice;
                        detailChildSon.OriginalAmount = detailChildSonGroup.OriginalAmount;
                        detailChildSon.DiscountAmount = detailChildSonGroup.DiscountAmount;
                        detailChildSon.ChargeAmount = detailChildSonGroup.ChargeAmount;
                        detailChild.Details.Add(detailChildSon);
                    }



                    var slotStockGroups = (from c in detailChildSons
                                           where c.MachineId == detailChildGroup.MachineId
                                        && c.SkuId == detailChildGroup.SkuId
                                           select new
                                           {
                                               c.MachineId,
                                               c.SkuId,
                                               c.SlotId
                                           }).Distinct().ToList();


                    foreach (var slotStockGroup in slotStockGroups)
                    {
                        var slotStock = new OrderReserveDetail.SlotStock();
                        slotStock.MachineId = slotStockGroup.MachineId;
                        slotStock.SkuId = slotStockGroup.SkuId;
                        slotStock.SlotId = slotStockGroup.SlotId;
                        slotStock.Quantity = detailChildSons.Where(m => m.MachineId == slotStockGroup.MachineId && m.SkuId == slotStockGroup.SkuId && m.SlotId == slotStockGroup.SlotId).Sum(m => m.Quantity);
                        detailChild.SlotStock.Add(slotStock);
                    }

                    detail.Details.Add(detailChild);

                }

                details.Add(detail);
            }




            return details;
        }


    }
}
