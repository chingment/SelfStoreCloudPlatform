﻿using Lumos.BLL.Biz;
using Lumos.BLL.Service.Merch;
using Lumos.BLL.Task;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.AppTerm
{

    public class OrderService : BaseService
    {
        public CustomJsonResult Reserve(RopOrderReserve rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.Id == rop.MachineId).FirstOrDefault();

            Biz.RopOrderReserve bizRop = new Biz.RopOrderReserve();
            bizRop.Source = Enumeration.OrderSource.Machine;
            bizRop.StoreId = machine.StoreId;
            bizRop.PayTimeout = rop.PayTimeout;
            bizRop.ReserveMode = Enumeration.ReserveMode.OffLine;
            bizRop.ChannelId = machine.Id;
            bizRop.ChannelType = Enumeration.ChannelType.Machine;

            foreach (var item in rop.Skus)
            {
                bizRop.Skus.Add(new Biz.RopOrderReserve.Sku() { Id = item.Id, Quantity = item.Quantity, ReceptionMode = Enumeration.ReceptionMode.Machine });
            }

            var bizResult = BizFactory.Order.Reserve(machine.MerchantId, bizRop);

            if (bizResult.Result == ResultType.Success)
            {
                RetOrderReserve ret = new RetOrderReserve();
                ret.OrderId = bizResult.Data.OrderId;
                ret.PayUrl = string.Format("http://mobile.17fanju.com/Order/Confirm?soure=machine&orderId=" + bizResult.Data.OrderId);

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
            }
            else
            {
                result = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, bizResult.Message);
            }


            return result;

        }

        public CustomJsonResult<RetOrderPayResultQuery> PayResultQuery(RupOrderPayResultQuery rup)
        {
            CustomJsonResult<RetOrderPayResultQuery> ret = new CustomJsonResult<RetOrderPayResultQuery>();


            var ret_Biz = BizFactory.Order.PayResultQuery(rup.MachineId, rup.OrderId);

            ret.Result = ret_Biz.Result;
            ret.Code = ret_Biz.Code;
            ret.Message = ret_Biz.Message;

            if (ret_Biz.Data != null)
            {
                ret.Data = new RetOrderPayResultQuery();
                ret.Data.OrderId = ret_Biz.Data.OrderId;
                ret.Data.OrderSn = ret_Biz.Data.OrderSn;
                ret.Data.Status = ret_Biz.Data.Status;
            }

            return ret;
        }

        public CustomJsonResult Cancle(RopOrderCancle rop)
        {
            CustomJsonResult result = new CustomJsonResult();


            result = BizFactory.Order.Cancle(GuidUtil.Empty(), rop.OrderId, rop.Reason);

            return result;
        }

        public CustomJsonResult SkusPickupBill(RupOrderSkusPickupBill rup)
        {
            CustomJsonResult result = new CustomJsonResult();

            var ret = new RetOrderSkusPickupBill();

            var orderDetailsChilds = CurrentDb.OrderDetailsChild.Where(m => m.OrderId == rup.OrderId).ToList();

            foreach (var orderDetailsChild in orderDetailsChilds)
            {
                var sku = new RetOrderSkusPickupBill.Sku();
                sku.Id = orderDetailsChild.ProductSkuId;
                sku.Name = orderDetailsChild.ProductSkuName;
                sku.ImgUrl = orderDetailsChild.ProductSkuImgUrl;
                sku.Quantity = orderDetailsChild.Quantity;

                var orderDetailsChildSons = CurrentDb.OrderDetailsChildSon.Where(m => m.OrderId == orderDetailsChild.OrderId && m.ProductSkuId == orderDetailsChild.ProductSkuId).ToList();

                foreach(var orderDetailsChildSon in  orderDetailsChildSons)
                {
                    var slot = new RetOrderSkusPickupBill.Slot();
                    slot.Id = orderDetailsChildSon.SlotId;
                    slot.Status = orderDetailsChildSon.Status;

                    sku.Slots.Add(slot);
                }
            }

            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);

            return result;
        }

        public CustomJsonResult SkuPickupStatusQuery(RupOrderSkuPickupStatusQuery rup)
        {
            CustomJsonResult result = new CustomJsonResult();

            var ret = new RetOrderSkuPickupStatusQuery();

            var orderDetailsChildSon = CurrentDb.OrderDetailsChildSon.Where(m => m.OrderId == rup.OrderId && m.ProductSkuId == rup.SkuId && m.SlotId == rup.SlotId).FirstOrDefault();

            ret.Status = orderDetailsChildSon.Status;
            ret.Tips = orderDetailsChildSon.Status.GetCnName();


            result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);

            return result;
        }

        public CustomJsonResult SkuPickupEventNotify(RopOrderSkuPickupEventNotify rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            //result = BizFactory.Order.Cancle(operater, rop.OrderSn, rop.Reason);

            return result;
        }

    }
}
