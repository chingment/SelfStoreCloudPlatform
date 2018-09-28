﻿using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.App
{
    public class CartService : BaseProvider
    {
        public CartPageModel GetPageData(string pOperater, string pClientId, string pStoreId)
        {
            var pageModel = new CartPageModel();


            var carts = CurrentDb.ClientCart.Where(m => m.ClientId == pClientId && m.StoreId == pStoreId && m.Status == Enumeration.CartStatus.WaitSettle).ToList();


            var skus = new List<CartSkuModel>();

            foreach (var item in carts)
            {
                var skuModel = BizFactory.ProductSku.GetModel(item.ProductSkuId);
                if (skuModel != null)
                {
                    var cartProcudtSkuModel = new CartSkuModel();
                    cartProcudtSkuModel.CartId = item.Id;
                    cartProcudtSkuModel.Id = skuModel.Id;
                    cartProcudtSkuModel.Name = skuModel.Name;
                    cartProcudtSkuModel.ImgUrl = ImgSet.GetMain(skuModel.DispalyImgUrls);
                    cartProcudtSkuModel.SalePrice = skuModel.SalePrice.ToF2Price();
                    cartProcudtSkuModel.Quantity = item.Quantity;
                    cartProcudtSkuModel.SumPrice = item.Quantity * skuModel.SalePrice;
                    cartProcudtSkuModel.Selected = item.Selected;
                    cartProcudtSkuModel.ChannelId = item.ChannelId;
                    cartProcudtSkuModel.ChannelType = item.ChannelType;
                    skus.Add(cartProcudtSkuModel);
                }
            }


            var channels = (from c in carts select new { c.ChannelId, c.ChannelType }).Distinct();


            foreach (var item in channels)
            {

                var carBlock = new CartBlock();
                carBlock.ChannelId = item.ChannelId;
                carBlock.ChannelType = item.ChannelType;
                carBlock.Skus = skus.Where(m => m.ChannelId == item.ChannelId && m.ChannelType == item.ChannelType).ToList();

                switch (item.ChannelType)
                {
                    case Enumeration.ChannelType.SelfPick:
                        carBlock.TagName = "自提商品";
                        break;
                    case Enumeration.ChannelType.Express:
                        carBlock.TagName = "快递商品";
                        break;
                }

                pageModel.Block.Add(carBlock);
            }



            pageModel.Count = skus.Sum(m => m.Quantity);
            pageModel.SumPrice = skus.Sum(m => m.SumPrice);
            pageModel.SumPriceBySelected = skus.Where(m => m.Selected == true).Sum(m => m.SumPrice);
            pageModel.CountBySelected = skus.Where(m => m.Selected == true).Count();

            return pageModel;
        }

        private static readonly object operatelock = new object();
        public CustomJsonResult Operate(string operater, string pClientId, RopCartOperate rop)
        {
            var result = new CustomJsonResult();

            lock (operatelock)
            {

                using (TransactionScope ts = new TransactionScope())
                {

                    foreach (var item in rop.Skus)
                    {
                        var mod_Cart = CurrentDb.ClientCart.Where(m => m.ClientId == pClientId && m.StoreId == rop.StoreId && m.ProductSkuId == item.SkuId && m.ChannelId == item.ChannelId && m.ChannelType == item.ChannelType && m.Status == Enumeration.CartStatus.WaitSettle).FirstOrDefault();

                        LogUtil.Info("购物车操作：" + rop.Operate);
                        switch (rop.Operate)
                        {
                            case Enumeration.CartOperateType.Selected:
                                LogUtil.Info("购物车操作：选择");
                                mod_Cart.Selected = item.Selected;
                                break;
                            case Enumeration.CartOperateType.Decrease:
                                LogUtil.Info("购物车操作：减少");
                                if (mod_Cart.Quantity >= 2)
                                {
                                    mod_Cart.Quantity -= 1;
                                    mod_Cart.MendTime = this.DateTime;
                                    mod_Cart.Mender = operater;
                                }
                                break;
                            case Enumeration.CartOperateType.Increase:
                                LogUtil.Info("购物车操作：增加");
                                var skuModel = BizFactory.ProductSku.GetModel(item.SkuId);

                                if (mod_Cart == null)
                                {
                                    mod_Cart = new ClientCart();
                                    mod_Cart.Id = GuidUtil.New();
                                    mod_Cart.ClientId = pClientId;
                                    mod_Cart.StoreId = rop.StoreId;
                                    mod_Cart.ProductSkuId = skuModel.Id;
                                    mod_Cart.ProductSkuName = skuModel.Name;
                                    mod_Cart.ProductSkuImgUrl = ImgSet.GetMain(skuModel.DispalyImgUrls);
                                    mod_Cart.CreateTime = this.DateTime;
                                    mod_Cart.Creator = operater;
                                    mod_Cart.Quantity = 1;
                                    mod_Cart.ChannelId = item.ChannelId;
                                    mod_Cart.ChannelType = item.ChannelType;
                                    mod_Cart.Status = Enumeration.CartStatus.WaitSettle;
                                    CurrentDb.ClientCart.Add(mod_Cart);
                                }
                                else
                                {
                                    mod_Cart.Quantity += 1;
                                    mod_Cart.MendTime = this.DateTime;
                                    mod_Cart.Mender = operater;
                                }
                                break;
                            case Enumeration.CartOperateType.Delete:
                                LogUtil.Info("购物车操作：删除");
                                mod_Cart.Status = Enumeration.CartStatus.Deleted;
                                mod_Cart.MendTime = this.DateTime;
                                mod_Cart.Mender = operater;
                                break;
                        }
                    }

                    CurrentDb.SaveChanges();

                    var cartModel = GetPageData(operater, pClientId, rop.StoreId);

                    ts.Complete();

                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", cartModel);
                }
            }


            return result;
        }
    }
}
