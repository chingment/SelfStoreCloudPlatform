﻿using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;
using Lumos.BLL.Task;
using Lumos.Entity;
using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.ApiApp
{

    public class OrderService : BaseProvider
    {

        public CustomJsonResult Reserve(string operater, string clientUserId, RopOrderReserve rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            Biz.RopOrderReserve bizRop = new Biz.RopOrderReserve();
            bizRop.Source = Enumeration.OrderSource.Machine;
            bizRop.StoreId = rop.StoreId;
            bizRop.ReserveMode = Enumeration.ReserveMode.Online;
            bizRop.ClientUserId = clientUserId;

            foreach (var item in rop.Skus)
            {
                bizRop.Skus.Add(new Biz.RopOrderReserve.Sku() { CartId = item.CartId, Id = item.Id, Quantity = item.Quantity, ReceptionMode = item.ReceptionMode });
            }

            var bizResult = BizFactory.Order.Reserve(operater, bizRop);

            if (bizResult.Result == ResultType.Success)
            {
                RetOrderReserve ret = new RetOrderReserve();
                ret.OrderId = bizResult.Data.OrderId;
                ret.OrderSn = bizResult.Data.OrderSn;
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
            }
            else
            {
                result = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, bizResult.Message);
            }


            return result;

        }

        public CustomJsonResult Confrim(string operater, string clientUserId, RopOrderConfirm rop)
        {
            var result = new CustomJsonResult();
            var ret = new RetOrderConfirm();
            var subtotalItem = new List<OrderConfirmSubtotalItemModel>();
            var skus = new List<OrderConfirmSkuModel>();

            decimal skuAmountByActual = 0;//实际总价
            decimal skuAmountByOriginal = 0;//原总价
            decimal skuAmountByMemebr = 0;//普通用户总价
            decimal skuAmountByVip = 0;//会员用户总价

            Store store;

            if (string.IsNullOrEmpty(rop.OrderId))
            {
                store = CurrentDb.Store.Where(m => m.Id == rop.StoreId).FirstOrDefault();

                if (rop.Skus != null)
                {
                    foreach (var item in rop.Skus)
                    {
                        var skuModel = BizFactory.ProductSku.GetModel(item.Id);

                        var machineStock = CurrentDb.StoreSellStock.Where(m => m.StoreId == rop.StoreId && m.ProductSkuId == item.Id).FirstOrDefault();

                        if (skuModel != null)
                        {
                            item.ImgUrl = skuModel.ImgUrl;
                            item.Name = skuModel.Name;
                            item.SalePrice = machineStock.SalePrice;
                            item.SalePriceByVip = machineStock.SalePriceByVip;

                            skuAmountByOriginal += (machineStock.SalePrice * item.Quantity);
                            skuAmountByMemebr += (machineStock.SalePrice * item.Quantity);
                            skuAmountByVip += (machineStock.SalePriceByVip * item.Quantity);

                            skus.Add(item);
                        }
                    }
                }
            }
            else
            {
                var order = CurrentDb.Order.Where(m => m.Id == rop.OrderId).FirstOrDefault();

                if (order == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到该订单");
                }

                if (order.Status != Enumeration.OrderStatus.WaitPay)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该订单不在就绪支付状态");
                }


                var orderDetailsChilds = CurrentDb.OrderDetailsChild.Where(m => m.OrderId == rop.OrderId).ToList();

                var storeId = orderDetailsChilds[0].StoreId;

                store = CurrentDb.Store.Where(m => m.Id == storeId).FirstOrDefault();

                foreach (var item in orderDetailsChilds)
                {
                    var orderConfirmSkuModel = new OrderConfirmSkuModel();
                    orderConfirmSkuModel.Id = item.ProductSkuId;
                    orderConfirmSkuModel.ImgUrl = item.ProductSkuImgUrl;
                    orderConfirmSkuModel.Name = item.ProductSkuName;
                    orderConfirmSkuModel.Quantity = item.Quantity;
                    orderConfirmSkuModel.SalePrice = item.SalePrice;
                    orderConfirmSkuModel.SalePriceByVip = item.SalePriceByVip;
                    orderConfirmSkuModel.ReceptionMode = item.ReceptionMode;
                    skuAmountByOriginal += (item.SalePrice * item.Quantity);
                    skuAmountByMemebr += (item.SalePrice * item.Quantity);
                    skuAmountByVip += (item.SalePriceByVip * item.Quantity);
                    skus.Add(orderConfirmSkuModel);
                }
            }


            var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == clientUserId).FirstOrDefault();
            bool isVip = false;

            if (clientUser != null)
            {
                if (clientUser.IsVip)
                {
                    isVip = true;
                }
            }


            if (isVip)
            {
                skuAmountByActual = skuAmountByVip;//会员用户总价 为 实际总价

                subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = "会员优惠", Amount = string.Format("-{0}", (skuAmountByOriginal - skuAmountByVip).ToF2Price()), IsDcrease = true });
            }
            else
            {
                skuAmountByActual = skuAmountByMemebr;
            }

            var orderBlock = new List<OrderBlockModel>();

            var skus_SelfExpress = skus.Where(m => m.ReceptionMode == Enumeration.ReceptionMode.Express).ToList();
            if (skus_SelfExpress.Count > 0)
            {
                var orderBlock_Express = new OrderBlockModel();
                orderBlock_Express.TagName = "快递商品";
                orderBlock_Express.Skus = skus_SelfExpress;
                var shippingAddressModel = new UserDeliveryAddressModel();
                var shippingAddress = CurrentDb.ClientDeliveryAddress.Where(m => m.ClientUserId == clientUserId && m.IsDefault == true).FirstOrDefault();
                if (shippingAddress != null)
                {
                    shippingAddressModel.Id = shippingAddress.Id;
                    shippingAddressModel.Consignee = shippingAddress.Consignee;
                    shippingAddressModel.PhoneNumber = shippingAddress.PhoneNumber;
                    shippingAddressModel.AreaName = shippingAddress.AreaName;
                    shippingAddressModel.Address = shippingAddress.Address;
                    shippingAddressModel.CanSelectElse = true;
                }
                orderBlock_Express.DeliveryAddress = shippingAddressModel;
                orderBlock.Add(orderBlock_Express);
            }

            var skus_SelfPick = skus.Where(m => m.ReceptionMode == Enumeration.ReceptionMode.Machine).ToList();
            if (skus_SelfPick.Count > 0)
            {
                var orderBlock_SelfPick = new OrderBlockModel();
                orderBlock_SelfPick.TagName = "自提商品";
                orderBlock_SelfPick.Skus = skus_SelfPick;
                var shippingAddressModel2 = new UserDeliveryAddressModel();
                shippingAddressModel2.Id = null;
                shippingAddressModel2.Consignee = "店铺名称";
                shippingAddressModel2.PhoneNumber = store.Name;
                shippingAddressModel2.AreaName = "";
                shippingAddressModel2.Address = store.Address;
                shippingAddressModel2.CanSelectElse = false;

                orderBlock_SelfPick.DeliveryAddress = shippingAddressModel2;

                orderBlock.Add(orderBlock_SelfPick);
            }

            ret.Block = orderBlock;

            #region 暂时不开通 Coupon

            //if (rop.CouponId == null || rop.CouponId.Count == 0)
            //{
            //    var couponsCount = CurrentDb.ClientCoupon.Where(m => m.ClientId == pClientId && m.Status == Entity.Enumeration.CouponStatus.WaitUse && m.EndTime > DateTime.Now).Count();

            //    if (couponsCount == 0)
            //    {
            //        ret.Coupon = new OrderConfirmCouponModel { TipMsg = "暂无可用优惠卷", TipType = TipType.NoCanUse };
            //    }
            //    else
            //    {
            //        ret.Coupon = new OrderConfirmCouponModel { TipMsg = string.Format("{0}个可用", couponsCount), TipType = TipType.CanUse };
            //    }
            //}
            //else
            //{

            //    var coupons = CurrentDb.ClientCoupon.Where(m => m.ClientId == pClientId && rop.CouponId.Contains(m.Id)).ToList();

            //    foreach (var item in coupons)
            //    {
            //        var amount = 0m;
            //        switch (item.Type)
            //        {
            //            case Enumeration.CouponType.FullCut:
            //            case Enumeration.CouponType.UnLimitedCut:
            //                if (skuAmountByActual >= item.LimitAmount)
            //                {
            //                    amount = -item.Discount;
            //                    skuAmountByActual = skuAmountByActual - item.Discount;

            //                    //subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = item.Name, Amount = string.Format("{0}", amount.ToF2Price()), IsDcrease = true });

            //                    ret.Coupon = new OrderConfirmCouponModel { TipMsg = string.Format("{0}", amount.ToF2Price()), TipType = TipType.InUse };

            //                }

            //                break;
            //            case Enumeration.CouponType.Discount:

            //                amount = skuAmountByActual - (skuAmountByActual * (item.Discount / 10));

            //                skuAmountByActual = skuAmountByActual * (item.Discount / 10);

            //                // subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = item.Name, Amount = string.Format("{0}", amount.ToF2Price()), IsDcrease = true });
            //                ret.Coupon = new OrderConfirmCouponModel { TipMsg = string.Format("{0}", amount.ToF2Price()), TipType = TipType.InUse };
            //                break;
            //        }
            //    }

            //}

            #endregion

            //subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = "满5减3元", Amount = "-9", IsDcrease = true });
            //subtotalItem.Add(new OrderConfirmSubtotalItemModel { ImgUrl = "", Name = "优惠卷", Amount = "-10", IsDcrease = true });

            ret.SubtotalItem = subtotalItem;


            ret.ActualAmount = skuAmountByActual.ToF2Price();

            ret.OriginalAmount = skuAmountByOriginal.ToF2Price();

            ret.OrderId = rop.OrderId;
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public List<RetOrderModel> List(string operater, string clientUserId, RupOrderList rup)
        {
            var query = (from o in CurrentDb.Order
                         where o.ClientUserId == clientUserId
                         select new { o.Id, o.Sn, o.StoreId, o.PickCode, o.StoreName, o.Status, o.SubmitTime, o.CompletedTime, o.ChargeAmount, o.CancledTime }
             );


            if (rup.Status != Enumeration.OrderStatus.Unknow)
            {
                query = query.Where(m => m.Status == rup.Status);
            }

            int pageSize = 10;

            query = query.OrderByDescending(r => r.SubmitTime).Skip(pageSize * (rup.PageIndex)).Take(pageSize);

            var list = query.ToList();

            List<RetOrderModel> models = new List<RetOrderModel>();

            foreach (var item in list)
            {
                var model = new RetOrderModel();

                model.Id = item.Id;
                model.Sn = item.Sn;
                model.Tag.Name = new FsText(item.StoreName, "");


                //model.StatusName = item.Status.GetCnName();
                model.ChargeAmount = item.ChargeAmount.ToF2Price();


                var orderDetails = CurrentDb.OrderDetails.Where(c => c.OrderId == item.Id).ToList();

                foreach (var orderDetail in orderDetails)
                {
                    var block = new FsBlock();

                    block.Tag.Name = new FsText(orderDetail.ChannelName, "");


                    if (item.Status == Enumeration.OrderStatus.Payed)
                    {
                        if (orderDetail.ChannelType == Enumeration.ChannelType.Machine)
                        {
                            block.Tag.Desc = new FsField("取货码", "", item.PickCode, "#f18d00");
                        }
                    }

                    var orderDetailsChilds = CurrentDb.OrderDetailsChild.Where(m => m.OrderDetailsId == orderDetail.Id).ToList();

                    foreach (var orderDetailsChild in orderDetailsChilds)
                    {

                        var field = new FsTemplateData();

                        field.Type = "SkuTmp";

                        var sku = new FsTemplateData.TmplOrderSku();

                        sku.Id = orderDetailsChild.Id;
                        sku.Name = orderDetailsChild.ProductSkuName;
                        sku.ImgUrl = orderDetailsChild.ProductSkuImgUrl;
                        sku.Quantity = orderDetailsChild.Quantity.ToString();
                        sku.ChargeAmount = orderDetailsChild.ChargeAmount.ToF2Price();


                        field.Value = sku;

                        block.Data.Add(field);
                    }


                    model.Blocks.Add(block);
                }

                switch (item.Status)
                {
                    case Enumeration.OrderStatus.WaitPay:
                        model.Buttons.Add(new FsButton() { Name = new FsText() { Content = "取消订单", Color = "red" }, OpType = "FUN", OpVal = "cancleOrder" });
                        model.Buttons.Add(new FsButton() { Name = new FsText() { Content = "继续支付", Color = "green" }, OpType = "URL", OpVal = OperateService.GetOrderDetailsUrl(rup.Caller, item.Id) });
                        break;
                    case Enumeration.OrderStatus.Payed:
                        model.Buttons.Add(new FsButton() { Name = new FsText() { Content = "查看详情", Color = "green" }, OpType = "URL", OpVal = OperateService.GetOrderDetailsUrl(rup.Caller, item.Id) });
                        break;
                    case Enumeration.OrderStatus.Completed:
                        model.Buttons.Add(new FsButton() { Name = new FsText() { Content = "查看详情", Color = "green" }, OpType = "URL", OpVal = OperateService.GetOrderDetailsUrl(rup.Caller, item.Id) });
                        break;
                    case Enumeration.OrderStatus.Cancled:
                        model.Buttons.Add(new FsButton() { Name = new FsText() { Content = "查看详情", Color = "green" }, OpType = "URL", OpVal = OperateService.GetOrderDetailsUrl(rup.Caller, item.Id) });
                        break;
                }

                models.Add(model);
            }


            return models;
        }


        public CustomJsonResult Details(string operater, string clientUserId, string orderId)
        {
            CustomJsonResult result = new CustomJsonResult();

            var ret = new RetOrderDetails();

            var order = CurrentDb.Order.Where(m => m.Id == orderId).FirstOrDefault();


            ret.Status = order.Status;


            ret.Tag.Name = new FsText(order.StoreName, "");
            ret.Tag.Desc = new FsField("状态", "", order.Status.GetCnName(), "");

            var fsBlockByField = new FsBlockByField();

            fsBlockByField.Tag.Name = new FsText("订单信息", "");

            fsBlockByField.Data.Add(new FsField("订单编号", "", order.Sn, ""));
            fsBlockByField.Data.Add(new FsField("创建时间", "", order.SubmitTime.ToUnifiedFormatDateTime(), ""));
            if (order.PayTime != null)
            {
                fsBlockByField.Data.Add(new FsField("付款时间", "", order.PayTime.ToUnifiedFormatDateTime(), ""));
            }
            if (order.CompletedTime != null)
            {
                fsBlockByField.Data.Add(new FsField("完成时间", "", order.CompletedTime.ToUnifiedFormatDateTime(), ""));
            }

            if (order.CancledTime != null)
            {
                fsBlockByField.Data.Add(new FsField("取消时间", "", order.CancledTime.ToUnifiedFormatDateTime(), ""));
                fsBlockByField.Data.Add(new FsField("取消原因", "", order.CancelReason, ""));
            }

            ret.FieldBlocks.Add(fsBlockByField);


            var orderDetails = CurrentDb.OrderDetails.Where(c => c.OrderId == order.Id).ToList();


            foreach (var orderDetail in orderDetails)
            {
                var block = new FsBlock();

                block.Tag.Name = new FsText(orderDetail.ChannelName, "");


                var orderDetailsChilds = CurrentDb.OrderDetailsChild.Where(m => m.OrderDetailsId == orderDetail.Id).ToList();

                foreach (var orderDetailsChild in orderDetailsChilds)
                {

                    var data = new FsTemplateData();

                    data.Type = "SkuTmp";

                    var sku = new FsTemplateData.TmplOrderSku();

                    sku.Id = orderDetailsChild.Id;
                    sku.Name = orderDetailsChild.ProductSkuName;
                    sku.ImgUrl = orderDetailsChild.ProductSkuImgUrl;
                    sku.Quantity = orderDetailsChild.Quantity.ToString();
                    sku.ChargeAmount = orderDetailsChild.ChargeAmount.ToF2Price();


                    data.Value = sku;

                    block.Data.Add(data);
                }


                ret.Blocks.Add(block);
            }




            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);

        }

        public CustomJsonResult Cancle(string operater, string clientUserId, RopOrderCancle rop)
        {
            return BizFactory.Order.Cancle(operater, rop.Id, "用户取消");
        }

        public CustomJsonResult GetJsApiPaymentPms(string operater, string clientUserId, WxAppInfoConfig appInfo, RupOrderGetJsApiPaymentPms rup)
        {
            var result = new CustomJsonResult();

            var wxUserInfo = CurrentDb.WxUserInfo.Where(m => m.ClientUserId == clientUserId).FirstOrDefault();

            if (wxUserInfo == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到该用户数据");
            }

            var order = CurrentDb.Order.Where(m => m.Id == rup.OrderId).FirstOrDefault();

            if (order == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到该订单数据");
            }

            order.AppId = appInfo.AppId;
            order.ClientUserId = wxUserInfo.ClientUserId;
            order.PayExpireTime = this.DateTime.AddMinutes(5);

            switch (rup.PayWay)
            {
                case PayWay.AliPay:
                    order.PayWay = Enumeration.OrderPayWay.AliPay;
                    break;
                case PayWay.Wechat:
                    order.PayWay = Enumeration.OrderPayWay.Wechat;


                    var ret_UnifiedOrder = SdkFactory.Wx.UnifiedOrderByJsApi(appInfo, wxUserInfo.OpenId, order.Sn, 0.01m, "", Common.CommonUtil.GetIP(), "自助商品", order.PayExpireTime.Value);

                    if (string.IsNullOrEmpty(ret_UnifiedOrder.PrepayId))
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "支付二维码生成失败");
                    }
                    order.PayPrepayId = ret_UnifiedOrder.PrepayId;
                    order.PayQrCodeUrl = ret_UnifiedOrder.CodeUrl;
                    CurrentDb.SaveChanges();

                    Task4Factory.Global.Enter(TimerTaskType.CheckOrderPay, order.PayExpireTime.Value, order);

                    var pms = SdkFactory.Wx.GetJsApiPayParams(appInfo, order.Id, order.Sn, ret_UnifiedOrder.PrepayId);

                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", pms);
                    break;
            }







            //JsApiPayParams parms = new JsApiPayParams("wxb01e0e16d57bd762", "37b016a9569e4f519702696e1274d63a", ret_UnifiedOrder.PrepayId, order.Id, order.Sn);

            //return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret.Data);

            return result;
        }

    }
}