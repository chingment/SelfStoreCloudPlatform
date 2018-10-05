using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.App
{



    public class OrderService : BaseProvider
    {
        public CustomJsonResult Confrim(string pPperater, string pClientId, RopOrderConfirm rop)
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

                        var machineStock = CurrentDb.MachineStock.Where(m => m.StoreId == rop.StoreId && m.ProductSkuId == item.Id).FirstOrDefault();

                        if (skuModel != null)
                        {
                            item.ImgUrl = skuModel.ImgUrl;
                            item.Name = skuModel.Name;
                            item.SalePrice = machineStock.SalePrice;
                            item.SalePriceByVip = machineStock.SalePriceByVip;
                            item.ChannelType = Enumeration.ChannelType.SelfPick;
                            item.ChannelId = 1;
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
                    orderConfirmSkuModel.ChannelType = Enumeration.ChannelType.SelfPick;
                    orderConfirmSkuModel.ChannelId = 1;

                    skuAmountByOriginal += (item.SalePrice * item.Quantity);
                    skuAmountByMemebr += (item.SalePrice * item.Quantity);
                    skuAmountByVip += (item.SalePriceByVip * item.Quantity);
                    skus.Add(orderConfirmSkuModel);
                }
            }


            var clientUser = CurrentDb.SysClientUser.Where(m => m.Id == pClientId).FirstOrDefault();
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

            var skus_SelfExpress = skus.Where(m => m.ChannelType == Enumeration.ChannelType.Express).ToList();
            if (skus_SelfExpress.Count > 0)
            {
                var orderBlock_Express = new OrderBlockModel();
                orderBlock_Express.TagName = "快递商品";
                orderBlock_Express.Skus = skus_SelfExpress;
                var shippingAddressModel = new UserDeliveryAddressModel();
                var shippingAddress = CurrentDb.ClientDeliveryAddress.Where(m => m.ClientId == pClientId && m.IsDefault == true).FirstOrDefault();
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

            var skus_SelfPick = skus.Where(m => m.ChannelType == Enumeration.ChannelType.SelfPick).ToList();
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

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult<Lumos.Entity.Order> GoSettle(int operater, RopOrderConfirm model)
        {
            var result = new CustomJsonResult<Lumos.Entity.Order>();

            //Lumos.Entity.Order mod_Order = null;
            //List<Lumos.Entity.OrderChildDetails> mod_OrderChildDetails = null;
            //List<Lumos.Entity.OrderChildProductSkuDetails> mod_OrderChildProductSkuDetails = null;


            return result;
        }
    }
}
