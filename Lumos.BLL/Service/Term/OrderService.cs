using Lumos.BLL.Service.Term.Models;
using Lumos.BLL.Task;
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
                    skusByStock = CurrentDb.MachineStock.Where(m => m.MerchantId == rop.MerchantId && skuIds.Contains(m.ProductSkuId)).ToList();
                }
                else
                {
                    skusByStock = CurrentDb.MachineStock.Where(m => m.MerchantId == rop.MerchantId && m.MachineId == rop.MachineId && skuIds.Contains(m.ProductSkuId)).ToList();
                }

                if (skusByStock.Count == 0)
                {
                    string tips = "";
                    foreach (var item in skusByStock)
                    {
                        var skuModel = BizFactory.ProductSku.GetModel(item.ProductSkuId);

                        tips += skuModel.Name + "、";
                    }

                    if (!string.IsNullOrEmpty(tips))
                    {
                        tips = tips.Substring(0, tips.Length - 1) + ",";
                    }

                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, tips + "可预定数量不足");
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

                    if (!string.IsNullOrEmpty(tips))
                    {
                        tips = tips.Substring(0, tips.Length - 1) + ",";
                    }
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, tips + "商品已经下架");
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

                    if (!string.IsNullOrEmpty(tips))
                    {
                        tips = tips.Substring(0, tips.Length - 1) + ",";
                    }
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, tips + "库存不足");
                }

                var store = CurrentDb.Store.Where(m => m.Id == rop.StoreId).FirstOrDefault();
                if (store == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "店铺无效");
                }

                var order = new Order();
                order.Id = GuidUtil.New();
                order.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.MerchantId);
                order.MerchantId = rop.MerchantId;
                order.StoreId = rop.StoreId;
                order.StoreName = store.Name;
                order.Quantity = rop.Details.Sum(m => m.Quantity);
                order.Status = Enumeration.OrderStatus.WaitPay;
                order.Source = rop.Source;
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
                    orderDetails.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.MerchantId);
                    //orderDetails.ClientId = rop.UserId;
                    orderDetails.MerchantId = rop.MerchantId;
                    orderDetails.StoreId = rop.StoreId;
                    orderDetails.MachineId = detail.MachineId;
                    orderDetails.ChannelId = detail.MachineId;
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
                    if (detail.MachineId == GuidUtil.Empty())
                    {
                        orderDetails.Receiver = rop.Receiver;
                        orderDetails.ReceiverPhone = rop.ReceiverPhone;
                        orderDetails.ReceptionAddress = rop.ReceptionAddress;
                        orderDetails.ChannelType = Enumeration.ChannelType.Express;
                        orderDetails.Id = GuidUtil.Empty();
                    }
                    else
                    {
                        orderDetails.Receiver = null;
                        orderDetails.ReceiverPhone = null;
                        orderDetails.ReceptionAddress = store.Address;
                        orderDetails.ChannelType = Enumeration.ChannelType.SelfPick;
                        orderDetails.ChannelId = detail.MachineId;
                    }

                    CurrentDb.OrderDetails.Add(orderDetails);

                    foreach (var detailsChild in detail.Details)
                    {

                        var orderDetailsChild = new OrderDetailsChild();
                        orderDetailsChild.Id = GuidUtil.New();
                        orderDetailsChild.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.MerchantId);
                        // orderDetailsChild.ClientId = rop.UserId;
                        orderDetailsChild.MerchantId = rop.MerchantId;
                        orderDetailsChild.StoreId = rop.StoreId;
                        orderDetailsChild.MachineId = detailsChild.MachineId;
                        orderDetailsChild.OrderId = order.Id;
                        orderDetailsChild.OrderSn = order.Sn;
                        orderDetailsChild.OrderDetailsId = orderDetails.Id;
                        orderDetailsChild.OrderDetailsSn = orderDetails.Sn;
                        orderDetailsChild.ProductSkuId = detailsChild.SkuId;
                        orderDetailsChild.ProductSkuName = detailsChild.SkuName;
                        orderDetailsChild.ProductSkuImgUrl = detailsChild.SkuImgUrl;
                        orderDetailsChild.SalePrice = detailsChild.SalePrice;
                        orderDetailsChild.SalePriceByVip = detailsChild.SalePriceByVip;
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
                            orderDetailsChildSon.Sn = SnUtil.Build(Enumeration.BizSnType.Order, rop.MerchantId);
                            // orderDetailsChildSon.ClientId = rop.UserId;
                            orderDetailsChildSon.MerchantId = rop.MerchantId;
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
                            orderDetailsChildSon.SalePrice = detailsChildSon.SalePrice;
                            orderDetailsChildSon.SalePriceByVip = detailsChildSon.SalePriceByVip;
                            orderDetailsChildSon.Quantity = detailsChildSon.Quantity;
                            orderDetailsChildSon.OriginalAmount = detailsChildSon.OriginalAmount;
                            orderDetailsChildSon.DiscountAmount = detailsChildSon.DiscountAmount;
                            orderDetailsChildSon.ChargeAmount = detailsChildSon.ChargeAmount;
                            orderDetailsChildSon.SubmitTime = this.DateTime;
                            orderDetailsChildSon.Creator = pOperater;
                            orderDetailsChildSon.CreateTime = this.DateTime;
                            orderDetailsChildSon.Status = Enumeration.OrderStatus.WaitPay;
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
                            machineStockLog.MerchantId = rop.MerchantId;
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

                order.PayExpireTime = this.DateTime.AddMinutes(2);

                //var ret_UnifiedOrder = SdkFactory.Wx.Instance().UnifiedOrder(pOperater, order.Sn, order.ChargeAmount, "", Common.CommonUtils.GetIP(), "自助商品", order.PayExpireTime.Value);

                //if (string.IsNullOrEmpty(ret_UnifiedOrder.CodeUrl))
                //{
                //    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "支付二维码生成失败");
                //}

                //order.PayPrepayId = ret_UnifiedOrder.PrepayId;
                //order.PayQrCodeUrl = ret_UnifiedOrder.CodeUrl;

                CurrentDb.Order.Add(order);
                CurrentDb.SaveChanges(true);
                ts.Complete();

                Task4Factory.Global.Enter(TimerTaskType.CheckOrderPay, order.PayExpireTime.Value, order);

                ret.OrderSn = order.Sn;
                ret.PayUrl = string.Format("http://mobile.17fanju.com/Order/Confirm?soure=machine&orderId=" + order.Id);


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "预定成功", ret);

            }


            return result;

        }

        public CustomJsonResult<RetOrderPayResultQuery> PayResultQuery(string operater, RupOrderPayResultQuery rup)
        {
            CustomJsonResult<RetOrderPayResultQuery> ret = new CustomJsonResult<RetOrderPayResultQuery>();


            var ret_Biz = BizFactory.Order.PayResultQuery(operater, rup.OrderSn);

            ret.Result = ret_Biz.Result;
            ret.Code = ret_Biz.Code;
            ret.Message = ret_Biz.Message;

            if (ret_Biz.Data != null)
            {
                ret.Data = new RetOrderPayResultQuery();
                ret.Data.OrderSn = ret_Biz.Data.OrderSn;
                ret.Data.Status = ret_Biz.Data.Status;
            }

            return ret;
        }

        public CustomJsonResult Cancle(string pOperater, RopOrderCancle rop)
        {
            CustomJsonResult result = new CustomJsonResult();


            return result;
        }


        private List<OrderReserveDetail> GetReserveDetail(List<RopOrderReserve.Detail> reserveDetails, List<MachineStock> machineStocks)
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
                            detailChildSon.SalePrice = item.SalePrice;
                            detailChildSon.SalePriceByVip = item.SalePriceByVip;
                            detailChildSon.OriginalAmount = detailChildSon.Quantity * item.SalePrice;

                            detailChildSons.Add(detailChildSon);
                        }
                    }
                }
            }

            var sumOriginalAmount = detailChildSons.Sum(m => m.OriginalAmount);
            var sumDiscountAmount = 0m;
            for (int i = 0; i < detailChildSons.Count; i++)
            {
                decimal scale = (sumOriginalAmount == 0 ? 0 : (detailChildSons[i].OriginalAmount / sumOriginalAmount));
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
                    detailChild.SalePrice = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).First().SalePrice;
                    detailChild.SalePriceByVip = detailChildSons.Where(m => m.MachineId == detailChildGroup.MachineId && m.SkuId == detailChildGroup.SkuId).First().SalePriceByVip;
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
                                                    c.SalePrice,
                                                    c.SalePriceByVip,
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
                        detailChildSon.SalePrice = detailChildSonGroup.SalePrice;
                        detailChildSon.SalePriceByVip = detailChildSonGroup.SalePriceByVip;
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
