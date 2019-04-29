using Lumos.BLL.Biz;
using Lumos.BLL.Task;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Biz
{
    public class OrderProvider : BaseProvider
    {
        public CustomJsonResult<RetOrderReserve> Reserve(string operater, RopOrderReserve rop)
        {
            CustomJsonResult<RetOrderReserve> result = new CustomJsonResult<RetOrderReserve>();

            if (rop.ReserveMode == Enumeration.ReserveMode.Unknow)
            {
                return new CustomJsonResult<RetOrderReserve>(ResultType.Failure, ResultCode.Failure, "未知预定方式", null);
            }

            if (rop.ReserveMode == Enumeration.ReserveMode.OffLine)
            {
                if (string.IsNullOrEmpty(rop.ChannelId))
                {
                    return new CustomJsonResult<RetOrderReserve>(ResultType.Failure, ResultCode.Failure, "机器ID不能为空", null);
                }
            }

            using (TransactionScope ts = new TransactionScope())
            {
                RetOrderReserve ret = new RetOrderReserve();

                var skuIds = rop.Skus.Select(m => m.Id).ToArray();

                //检查是否有可买的商品

                List<StoreSellStock> skusByStock = new List<StoreSellStock>();

                if (rop.ReserveMode == Enumeration.ReserveMode.OffLine)
                {
                    skusByStock = CurrentDb.StoreSellStock.Where(m => m.StoreId == rop.StoreId && m.ChannelType == Enumeration.ChannelType.Machine && m.ChannelId == rop.ChannelId && skuIds.Contains(m.ProductSkuId)).ToList();
                }
                else if (rop.ReserveMode == Enumeration.ReserveMode.Online)
                {
                    skusByStock = CurrentDb.StoreSellStock.Where(m => m.StoreId == rop.StoreId && skuIds.Contains(m.ProductSkuId)).ToList();
                }

                List<string> warn_tips = new List<string>();

                foreach (var sku in rop.Skus)
                {
                    var skuModel = BizFactory.ProductSku.GetModel(sku.Id);

                    var sellQuantity = 0;

                    if (rop.ReserveMode == Enumeration.ReserveMode.OffLine)
                    {
                        sellQuantity = skusByStock.Where(m => m.ProductSkuId == sku.Id && m.ChannelType == Enumeration.ChannelType.Machine && m.ChannelId == rop.ChannelId).Sum(m => m.SellQuantity);
                    }
                    else if (rop.ReserveMode == Enumeration.ReserveMode.Online)
                    {
                        if (sku.ReceptionMode == Enumeration.ReceptionMode.Machine)
                        {
                            sellQuantity = skusByStock.Where(m => m.ProductSkuId == sku.Id && m.ChannelType == Enumeration.ChannelType.Machine).Sum(m => m.SellQuantity);
                        }
                        else if (sku.ReceptionMode == Enumeration.ReceptionMode.Express)
                        {
                            sellQuantity = skusByStock.Where(m => m.ProductSkuId == sku.Id && m.ChannelType == Enumeration.ChannelType.Express).Sum(m => m.SellQuantity);
                        }
                    }

                    var hasOffSell = skusByStock.Where(m => m.ProductSkuId == sku.Id).Where(m => m.IsOffSell == true).FirstOrDefault();

                    if (hasOffSell == null)
                    {
                        if (sellQuantity < sku.Quantity)
                        {
                            warn_tips.Add(string.Format("{0}的可销售数量为{1}个", skuModel.Name, sellQuantity));
                        }
                    }
                    else
                    {
                        warn_tips.Add(string.Format("{0}已经下架", skuModel.Name));
                    }
                }

                if (warn_tips.Count > 0)
                {
                    return new CustomJsonResult<RetOrderReserve>(ResultType.Failure, ResultCode.Failure, string.Join(";", warn_tips.ToArray()), null);
                }

                var store = CurrentDb.Store.Where(m => m.Id == rop.StoreId).FirstOrDefault();
                if (store == null)
                {
                    return new CustomJsonResult<RetOrderReserve>(ResultType.Failure, ResultCode.Failure, "店铺无效", null);
                }

                var order = new Order();
                order.Id = GuidUtil.New();
                order.Sn = SnUtil.Build(Enumeration.BizSnType.Order, store.MerchantId);
                order.MerchantId = store.MerchantId;
                order.StoreId = rop.StoreId;
                order.StoreName = store.Name;
                order.ClientUserId = rop.ClientUserId;
                order.Quantity = rop.Skus.Sum(m => m.Quantity);
                order.Status = Enumeration.OrderStatus.WaitPay;
                order.Source = rop.Source;
                order.SubmitTime = this.DateTime;
                order.PayExpireTime = this.DateTime.AddSeconds(300);
                order.Creator = operater;
                order.CreateTime = this.DateTime;

                #region 更改购物车标识

                if (!string.IsNullOrEmpty(rop.ClientUserId))
                {
                    var cartsIds = rop.Skus.Select(m => m.CartId).Distinct().ToArray();
                    if (cartsIds != null)
                    {
                        var clientCarts = CurrentDb.ClientCart.Where(m => cartsIds.Contains(m.Id) && m.ClientUserId == rop.ClientUserId).ToList();
                        if (clientCarts != null)
                        {
                            foreach (var cart in clientCarts)
                            {
                                cart.Status = Enumeration.CartStatus.Settling;
                                cart.Mender = operater;
                                cart.MendTime = this.DateTime;
                                CurrentDb.SaveChanges();
                            }
                        }
                    }
                }
                #endregion 

                var reserveDetails = GetReserveDetail(rop.Skus, skusByStock);

                order.OriginalAmount = reserveDetails.Sum(m => m.OriginalAmount);
                order.DiscountAmount = reserveDetails.Sum(m => m.DiscountAmount);
                order.ChargeAmount = reserveDetails.Sum(m => m.ChargeAmount);

                foreach (var detail in reserveDetails)
                {
                    var orderDetails = new OrderDetails();
                    orderDetails.Id = GuidUtil.New();
                    orderDetails.Sn = order.Sn + reserveDetails.IndexOf(detail).ToString();
                    orderDetails.ClientUserId = rop.ClientUserId;
                    orderDetails.MerchantId = store.MerchantId;
                    orderDetails.StoreId = rop.StoreId;
                    orderDetails.ChannelType = detail.ChannelType;
                    orderDetails.ChannelId = detail.ChannelId;
                    switch (detail.ChannelType)
                    {
                        case Enumeration.ChannelType.Machine:
                            var machine = CurrentDb.Machine.Where(m => m.Id == detail.ChannelId).FirstOrDefault();
                            orderDetails.ChannelName = "【自提】 " + machine.Name;//todo 若 ChannelType 为1 则机器昵称，2为自取
                            break;
                        case Enumeration.ChannelType.Express:
                            orderDetails.ChannelName = "【快递】";
                            break;
                    }
                    orderDetails.OrderId = order.Id;
                    orderDetails.OrderSn = order.Sn;
                    orderDetails.OriginalAmount = detail.OriginalAmount;
                    orderDetails.DiscountAmount = detail.DiscountAmount;
                    orderDetails.ChargeAmount = detail.ChargeAmount;
                    orderDetails.Quantity = detail.Quantity;
                    orderDetails.ReceptionMode = detail.ReceptionMode;
                    orderDetails.SubmitTime = this.DateTime;
                    orderDetails.Creator = operater;
                    orderDetails.CreateTime = this.DateTime;

                    //detail.MachineId为空 则为快递商品
                    if (detail.ChannelId == GuidUtil.Empty())
                    {
                        orderDetails.Receiver = rop.Receiver;
                        orderDetails.ReceiverPhone = rop.ReceiverPhone;
                        orderDetails.ReceptionAddress = rop.ReceptionAddress;
                        orderDetails.ChannelType = Enumeration.ChannelType.Express;
                        orderDetails.ChannelId = GuidUtil.Empty();
                    }
                    else
                    {
                        orderDetails.Receiver = null;
                        orderDetails.ReceiverPhone = null;
                        orderDetails.ReceptionAddress = store.Address;
                        orderDetails.ChannelType = Enumeration.ChannelType.Machine;
                        orderDetails.ChannelId = detail.ChannelId;
                    }

                    CurrentDb.OrderDetails.Add(orderDetails);

                    foreach (var detailsChild in detail.Details)
                    {

                        var orderDetailsChild = new OrderDetailsChild();
                        orderDetailsChild.Id = GuidUtil.New();
                        orderDetailsChild.Sn = orderDetails.Sn + detail.Details.IndexOf(detailsChild).ToString();
                        orderDetailsChild.ClientUserId = rop.ClientUserId;
                        orderDetailsChild.MerchantId = store.MerchantId;
                        orderDetailsChild.StoreId = rop.StoreId;
                        orderDetailsChild.ChannelType = detailsChild.ChannelType;
                        orderDetailsChild.ChannelId = detailsChild.ChannelId;
                        orderDetailsChild.ReceptionMode = detail.ReceptionMode;
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
                        orderDetailsChild.Status = Enumeration.OrderStatus.WaitPay;
                        orderDetailsChild.Creator = operater;
                        orderDetailsChild.CreateTime = this.DateTime;
                        CurrentDb.OrderDetailsChild.Add(orderDetailsChild);

                        foreach (var detailsChildSon in detailsChild.Details)
                        {
                            var orderDetailsChildSon = new OrderDetailsChildSon();

                            orderDetailsChildSon.Id = GuidUtil.New();
                            orderDetailsChildSon.Sn = orderDetailsChild.Sn + detailsChild.Details.IndexOf(detailsChildSon);
                            orderDetailsChildSon.ClientUserId = rop.ClientUserId;
                            orderDetailsChildSon.MerchantId = store.MerchantId;
                            orderDetailsChildSon.StoreId = rop.StoreId;
                            orderDetailsChildSon.ChannelType = detailsChildSon.ChannelType;
                            orderDetailsChildSon.ChannelId = detailsChildSon.ChannelId;
                            orderDetailsChildSon.ReceptionMode = detail.ReceptionMode;
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
                            orderDetailsChildSon.Creator = operater;
                            orderDetailsChildSon.CreateTime = this.DateTime;
                            orderDetailsChildSon.Status = Enumeration.OrderDetailsChildSonStatus.WaitPay;
                            CurrentDb.OrderDetailsChildSon.Add(orderDetailsChildSon);
                        }



                        foreach (var slotStock in detailsChild.SlotStock)
                        {
                            var machineStock = skusByStock.Where(m => m.ProductSkuId == slotStock.SkuId && m.SlotId == slotStock.SlotId && m.ChannelId == slotStock.ChannelId).FirstOrDefault();

                            machineStock.LockQuantity += slotStock.Quantity;
                            machineStock.SellQuantity -= slotStock.Quantity;
                            machineStock.Mender = operater;
                            machineStock.MendTime = this.DateTime;


                            var storeSellStockLog = new StoreSellStockLog();
                            storeSellStockLog.Id = GuidUtil.New();
                            storeSellStockLog.MerchantId = store.MerchantId;
                            storeSellStockLog.StoreId = rop.StoreId;
                            storeSellStockLog.ChannelType = slotStock.ChannelType;
                            storeSellStockLog.ChannelId = slotStock.ChannelId;
                            storeSellStockLog.SlotId = slotStock.SlotId;
                            storeSellStockLog.ProductSkuId = slotStock.SkuId;
                            storeSellStockLog.Quantity = machineStock.Quantity;
                            storeSellStockLog.LockQuantity = machineStock.LockQuantity;
                            storeSellStockLog.SellQuantity = machineStock.SellQuantity;
                            storeSellStockLog.ChangeType = Enumeration.MachineStockLogChangeTpye.Lock;
                            storeSellStockLog.ChangeQuantity = slotStock.Quantity;
                            storeSellStockLog.Creator = operater;
                            storeSellStockLog.CreateTime = this.DateTime;
                            storeSellStockLog.RemarkByDev = string.Format("预定锁定库存：{0}", slotStock.Quantity);
                            CurrentDb.StoreSellStockLog.Add(storeSellStockLog);
                        }
                    }
                }

                CurrentDb.Order.Add(order);
                CurrentDb.SaveChanges(true);
                ts.Complete();

                Task4Factory.Global.Enter(TimerTaskType.CheckOrderPay, order.PayExpireTime.Value, order);

                ret.OrderId = order.Id;
                ret.OrderSn = order.Sn;

                result = new CustomJsonResult<RetOrderReserve>(ResultType.Success, ResultCode.Success, "预定成功", ret);

            }

            return result;

        }

        private List<OrderReserveDetail> GetReserveDetail(List<RopOrderReserve.Sku> reserveDetails, List<StoreSellStock> storeSellStocks)
        {
            List<OrderReserveDetail> details = new List<OrderReserveDetail>();

            List<OrderReserveDetail.DetailChildSon> detailChildSons = new List<OrderReserveDetail.DetailChildSon>();

            var receptionModes = reserveDetails.Select(m => m.ReceptionMode).Distinct().ToArray();

            foreach (var receptionMode in receptionModes)
            {
                var l_reserveDetails = reserveDetails.Where(m => m.ReceptionMode == receptionMode).ToList();

                foreach (var reserveDetail in l_reserveDetails)
                {
                    Entity.Enumeration.ChannelType channelType = receptionMode == Enumeration.ReceptionMode.Express ? Enumeration.ChannelType.Express : Enumeration.ChannelType.Machine;
                    var l_storeSellStocks = storeSellStocks.Where(m => m.ProductSkuId == reserveDetail.Id && m.ChannelType == channelType).ToList();

                    foreach (var item in l_storeSellStocks)
                    {
                        for (var i = 0; i < item.SellQuantity; i++)
                        {
                            int reservedQuantity = detailChildSons.Where(m => m.SkuId == reserveDetail.Id && m.ChannelType == channelType).Sum(m => m.Quantity);//已订的数量
                            int needReserveQuantity = reserveDetail.Quantity;//需要订的数量
                            if (reservedQuantity != needReserveQuantity)
                            {

                                var product = BizFactory.ProductSku.GetModel(item.ProductSkuId);

                                var detailChildSon = new OrderReserveDetail.DetailChildSon();
                                detailChildSon.Id = GuidUtil.New();
                                detailChildSon.ChannelId = item.ChannelId;
                                detailChildSon.ChannelType = item.ChannelType;
                                detailChildSon.ReceptionMode = receptionMode;
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
                                    c.ReceptionMode,
                                    c.ChannelType,
                                    c.ChannelId
                                }).Distinct().ToList();



            foreach (var detailGroup in detailGroups)
            {
                var detail = new OrderReserveDetail();
                detail.ReceptionMode = detailGroup.ReceptionMode;
                detail.ChannelType = detailGroup.ChannelType;
                detail.ChannelId = detailGroup.ChannelId;
                detail.Quantity = detailChildSons.Where(m => m.ChannelId == detailGroup.ChannelId).Sum(m => m.Quantity);
                detail.OriginalAmount = detailChildSons.Where(m => m.ChannelId == detailGroup.ChannelId).Sum(m => m.OriginalAmount);
                detail.DiscountAmount = detailChildSons.Where(m => m.ChannelId == detailGroup.ChannelId).Sum(m => m.DiscountAmount);
                detail.ChargeAmount = detailChildSons.Where(m => m.ChannelId == detailGroup.ChannelId).Sum(m => m.ChargeAmount);


                var detailChildGroups = (from c in detailChildSons
                                         where c.ChannelId == detailGroup.ChannelId
                                         select new
                                         {
                                             c.ChannelType,
                                             c.ChannelId,
                                             c.SkuId
                                         }).Distinct().ToList();

                foreach (var detailChildGroup in detailChildGroups)
                {

                    var detailChild = new OrderReserveDetail.DetailChild();
                    detailChild.ChannelType = detailChildGroup.ChannelType;
                    detailChild.ChannelId = detailChildGroup.ChannelId;
                    detailChild.SkuId = detailChildGroup.SkuId;
                    detailChild.SkuName = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).First().SkuName;
                    detailChild.SkuImgUrl = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).First().SkuImgUrl;
                    detailChild.SalePrice = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).First().SalePrice;
                    detailChild.SalePriceByVip = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).First().SalePriceByVip;
                    detailChild.Quantity = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.Quantity);
                    detailChild.OriginalAmount = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.OriginalAmount);
                    detailChild.DiscountAmount = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.DiscountAmount);
                    detailChild.ChargeAmount = detailChildSons.Where(m => m.ChannelId == detailChildGroup.ChannelId && m.SkuId == detailChildGroup.SkuId).Sum(m => m.ChargeAmount);

                    var detailChildSonGroups = (from c in detailChildSons
                                                where c.ChannelId == detailChildGroup.ChannelId
                                             && c.SkuId == detailChildGroup.SkuId
                                                select new
                                                {
                                                    c.Id,
                                                    c.ReceptionMode,
                                                    c.ChannelType,
                                                    c.ChannelId,
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
                        detailChildSon.ChannelType = detailChildSonGroup.ChannelType;
                        detailChildSon.ChannelId = detailChildSonGroup.ChannelId;
                        detailChildSon.ReceptionMode = detailChildSonGroup.ReceptionMode;
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
                                           where c.ChannelId == detailChildGroup.ChannelId
                                        && c.SkuId == detailChildGroup.SkuId
                                           select new
                                           {
                                               c.ChannelType,
                                               c.ChannelId,
                                               c.SkuId,
                                               c.SlotId
                                           }).Distinct().ToList();


                    foreach (var slotStockGroup in slotStockGroups)
                    {
                        var slotStock = new OrderReserveDetail.SlotStock();
                        slotStock.ChannelType = slotStockGroup.ChannelType;
                        slotStock.ChannelId = slotStockGroup.ChannelId;
                        slotStock.SkuId = slotStockGroup.SkuId;
                        slotStock.SlotId = slotStockGroup.SlotId;
                        slotStock.Quantity = detailChildSons.Where(m => m.ChannelType == slotStockGroup.ChannelType && m.ChannelId == slotStockGroup.ChannelId && m.SkuId == slotStockGroup.SkuId && m.SlotId == slotStockGroup.SlotId).Sum(m => m.Quantity);
                        detailChild.SlotStock.Add(slotStock);
                    }

                    detail.Details.Add(detailChild);

                }

                details.Add(detail);
            }

            return details;
        }

        private static readonly object lock_PayResultNotify = new object();
        public CustomJsonResult PayResultNotify(string operater, Enumeration.OrderNotifyLogNotifyFrom from, string content, string orderSn, out bool isPaySuccessed)
        {
            lock (lock_PayResultNotify)
            {
                bool m_isPaySuccessed = false;
                var mod_OrderNotifyLog = new OrderNotifyLog();

                switch (from)
                {
                    case Enumeration.OrderNotifyLogNotifyFrom.WebApp:
                        if (content == "chooseWXPay:ok")
                        {
                            // PayCompleted(operater, orderSn, this.DateTime);
                        }
                        break;
                    case Enumeration.OrderNotifyLogNotifyFrom.OrderQuery:
                        var dic1 = WeiXinSdk.CommonUtil.ToDictionary(content);
                        if (dic1.ContainsKey("out_trade_no") && dic1.ContainsKey("trade_state"))
                        {
                            orderSn = dic1["out_trade_no"].ToString();
                            string trade_state = dic1["trade_state"].ToString();
                            if (trade_state == "SUCCESS")
                            {
                                m_isPaySuccessed = true;
                                PayCompleted(operater, orderSn, this.DateTime);
                            }
                        }
                        break;
                    case Enumeration.OrderNotifyLogNotifyFrom.NotifyUrl:
                        var dic2 = WeiXinSdk.CommonUtil.ToDictionary(content);
                        if (dic2.ContainsKey("out_trade_no") && dic2.ContainsKey("result_code"))
                        {
                            orderSn = dic2["out_trade_no"].ToString();
                            string result_code = dic2["result_code"].ToString();

                            if (result_code == "SUCCESS")
                            {
                                m_isPaySuccessed = true;
                                PayCompleted(operater, orderSn, this.DateTime);
                            }
                        }
                        break;
                }

                var order = CurrentDb.Order.Where(m => m.Sn == orderSn).FirstOrDefault();
                if (order != null)
                {
                    mod_OrderNotifyLog.MerchantId = order.MerchantId;
                    mod_OrderNotifyLog.OrderId = order.Id;
                    mod_OrderNotifyLog.OrderSn = order.Sn;
                }
                mod_OrderNotifyLog.Id = GuidUtil.New();
                mod_OrderNotifyLog.NotifyContent = content;
                mod_OrderNotifyLog.NotifyFrom = from;
                mod_OrderNotifyLog.NotifyType = Enumeration.OrderNotifyLogNotifyType.Pay;
                mod_OrderNotifyLog.CreateTime = this.DateTime;
                mod_OrderNotifyLog.Creator = operater;
                CurrentDb.OrderNotifyLog.Add(mod_OrderNotifyLog);
                CurrentDb.SaveChanges();

                isPaySuccessed = m_isPaySuccessed;

                return new CustomJsonResult(ResultType.Success, ResultCode.Success, "");
            }
        }

        public CustomJsonResult PayCompleted(string operater, string orderSn, DateTime completedTime)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var order = CurrentDb.Order.Where(m => m.Sn == orderSn).FirstOrDefault();

                if (order == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("找不到该订单号({0})", orderSn));
                }

                if (order.Status == Enumeration.OrderStatus.Payed || order.Status == Enumeration.OrderStatus.Completed)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("订单号({0})已经支付通知成功", orderSn));
                }

                if (order.Status != Enumeration.OrderStatus.WaitPay)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("找不到该订单号({0})", orderSn));
                }

                order.Status = Enumeration.OrderStatus.Payed;
                order.PayTime = this.DateTime;
                order.MendTime = this.DateTime;
                order.Mender = operater;






                CurrentDb.SaveChanges();
                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, string.Format("支付完成通知：订单号({0})通知成功", orderSn));
            }

            return result;
        }

        public CustomJsonResult<RetPayResultQuery> PayResultQuery(string operater, string orderId)
        {
            var result = new CustomJsonResult<RetPayResultQuery>();

            var order = CurrentDb.Order.Where(m => m.Id == orderId).FirstOrDefault();

            if (order == null)
            {
                return new CustomJsonResult<RetPayResultQuery>(ResultType.Failure, ResultCode.Failure, "找不到订单", null);
            }

            var ret = new RetPayResultQuery();

            ret.OrderId = order.Id;
            ret.OrderSn = order.Sn;
            ret.Status = order.Status;

            result = new CustomJsonResult<RetPayResultQuery>(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }

        public CustomJsonResult Cancle(string operater, string orderId, string cancelReason)
        {
            var result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                var order = CurrentDb.Order.Where(m => m.Id == orderId).FirstOrDefault();

                if (order == null)
                {
                    LogUtil.Info(string.Format("该订单号:{0},找不到", orderId));
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("该订单号:{0},找不到", orderId));
                }

                if (order.Status == Enumeration.OrderStatus.Cancled)
                {
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经取消");
                }

                if (order.Status == Enumeration.OrderStatus.Payed)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单已经支付成功");
                }

                if (order.Status == Enumeration.OrderStatus.Completed)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单已经完成");
                }

                if (order.Status != Enumeration.OrderStatus.Payed && order.Status != Enumeration.OrderStatus.Completed)
                {
                    order.Status = Enumeration.OrderStatus.Cancled;
                    order.Mender = GuidUtil.Empty();
                    order.MendTime = this.DateTime;
                    order.CancledTime = this.DateTime;
                    order.CancelReason = cancelReason;

                    var orderDetails = CurrentDb.OrderDetails.Where(m => m.OrderId == order.Id).ToList();

                    foreach (var item in orderDetails)
                    {
                        item.Status = Enumeration.OrderStatus.Cancled;
                        item.Mender = GuidUtil.Empty();
                        item.MendTime = this.DateTime;
                    }


                    var orderDetailsChilds = CurrentDb.OrderDetailsChild.Where(m => m.OrderId == order.Id).ToList();

                    foreach (var item in orderDetailsChilds)
                    {
                        item.Status = Enumeration.OrderStatus.Cancled;
                        item.Mender = GuidUtil.Empty();
                        item.MendTime = this.DateTime;
                    }

                    var orderDetailsChildSons = CurrentDb.OrderDetailsChildSon.Where(m => m.OrderId == order.Id).ToList();

                    foreach (var item in orderDetailsChildSons)
                    {
                        item.Status = Enumeration.OrderDetailsChildSonStatus.Cancled;
                        item.Mender = GuidUtil.Empty();
                        item.MendTime = this.DateTime;

                        var machineStock = CurrentDb.StoreSellStock.Where(m => m.MerchantId == order.MerchantId && m.StoreId == order.StoreId && m.ProductSkuId == item.ProductSkuId && m.SlotId == item.SlotId && m.ChannelId == item.ChannelId && m.ChannelType == item.ChannelType).FirstOrDefault();

                        machineStock.LockQuantity -= item.Quantity;
                        machineStock.SellQuantity += item.Quantity;
                        machineStock.Mender = operater;
                        machineStock.MendTime = this.DateTime;

                        var storeSellStockLog = new StoreSellStockLog();
                        storeSellStockLog.Id = GuidUtil.New();
                        storeSellStockLog.MerchantId = item.MerchantId;
                        storeSellStockLog.StoreId = item.StoreId;
                        storeSellStockLog.ChannelType = item.ChannelType;
                        storeSellStockLog.ChannelId = item.ChannelId;
                        storeSellStockLog.SlotId = item.SlotId;
                        storeSellStockLog.ProductSkuId = item.ProductSkuId;
                        storeSellStockLog.Quantity = machineStock.Quantity;
                        storeSellStockLog.LockQuantity = machineStock.LockQuantity;
                        storeSellStockLog.SellQuantity = machineStock.SellQuantity;
                        storeSellStockLog.ChangeType = Enumeration.MachineStockLogChangeTpye.Lock;
                        storeSellStockLog.ChangeQuantity = item.Quantity;
                        storeSellStockLog.Creator = operater;
                        storeSellStockLog.CreateTime = this.DateTime;
                        storeSellStockLog.RemarkByDev = string.Format("取消订单，恢复库存：{0}", item.Quantity);
                        CurrentDb.StoreSellStockLog.Add(storeSellStockLog);
                    }

                    CurrentDb.SaveChanges();
                    ts.Complete();

                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
                }
            }

            return result;
        }
    }
}
