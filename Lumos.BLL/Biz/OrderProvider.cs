using Lumos.BLL.Biz.RModels;
using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class OrderProvider : BaseProvider
    {
        private static readonly object lock_UnifiedOrder = new object();
        public CustomJsonResult<Order> UnifiedOrder(string pOperater, string pClientId, UnifiedOrderPms pPayPms)
        {
            CustomJsonResult<Order> result = new CustomJsonResult<Order>();
            lock (lock_UnifiedOrder)
            {
                var strOrderPms = pPayPms.OrderPms.ToJsonString();

            }
            return result;
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

        public CustomJsonResult PayCompleted(string pOperater, string pOrderSn, DateTime pCompletedTime)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var order = CurrentDb.Order.Where(m => m.Sn == pOrderSn).FirstOrDefault();

                if (order == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("找不到该订单号({0})", pOrderSn));
                }

                if (order.Status == Enumeration.OrderStatus.Payed || order.Status == Enumeration.OrderStatus.Completed)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("订单号({0})已经支付通知成功", pOrderSn));
                }

                if (order.Status != Enumeration.OrderStatus.WaitPay)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("找不到该订单号({0})", pOrderSn));
                }

                order.Status = Enumeration.OrderStatus.Payed;
                order.PayTime = this.DateTime;
                order.MendTime = this.DateTime;
                order.Mender = pOperater;

                CurrentDb.SaveChanges();
                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, string.Format("支付完成通知：订单号({0})通知成功", pOrderSn));
            }

            return result;
        }

        public CustomJsonResult<RetPayResultQuery> PayResultQuery(string pOperater, string orderSn)
        {
            var result = new CustomJsonResult<RetPayResultQuery>();

            var order = CurrentDb.Order.Where(m => m.Sn == orderSn).FirstOrDefault();

            if (order == null)
            {
                return new CustomJsonResult<RetPayResultQuery>(ResultType.Failure, ResultCode.Failure, "找不到订单", null);
            }

            var ret = new RetPayResultQuery();

            ret.OrderSn = order.Sn;
            ret.Status = order.Status;

            result = new CustomJsonResult<RetPayResultQuery>(ResultType.Success, ResultCode.Success, "获取成功", ret);

            return result;
        }


        public CustomJsonResult Cancle(string pOperater, string pOrderSn, string cancelReason)
        {
            var result = new CustomJsonResult();


            using (TransactionScope ts = new TransactionScope())
            {
                var order = CurrentDb.Order.Where(m => m.Sn == pOrderSn).FirstOrDefault();

                if (order.Status == Enumeration.OrderStatus.Cancled)
                {
                    return new CustomJsonResult(ResultType.Success, ResultCode.Success, "该订单已经取消");
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

                        var machineStock = CurrentDb.MachineStock.Where(m => m.MerchantId == order.MerchantId && m.StoreId == order.StoreId && m.ProductSkuId == item.ProductSkuId && m.SlotId == item.SlotId && m.MachineId == item.MachineId).FirstOrDefault();

                        machineStock.LockQuantity -= item.Quantity;
                        machineStock.SellQuantity += item.Quantity;
                        machineStock.Mender = pOperater;
                        machineStock.MendTime = this.DateTime;

                        var machineStockLog = new MachineStockLog();
                        machineStockLog.Id = GuidUtil.New();
                        machineStockLog.MerchantId = item.MerchantId;
                        machineStockLog.StoreId = item.StoreId;
                        machineStockLog.MachineId = item.MachineId;
                        machineStockLog.SlotId = item.SlotId;
                        machineStockLog.ProductSkuId = item.ProductSkuId;
                        machineStockLog.Quantity = machineStock.Quantity;
                        machineStockLog.LockQuantity = machineStock.LockQuantity;
                        machineStockLog.SellQuantity = machineStock.SellQuantity;
                        machineStockLog.ChangeType = Enumeration.MachineStockLogChangeTpye.Lock;
                        machineStockLog.ChangeQuantity = item.Quantity;
                        machineStockLog.Creator = pOperater;
                        machineStockLog.CreateTime = this.DateTime;
                        machineStockLog.RemarkByDev = string.Format("取消订单，恢复库存：{0}", item.Quantity);
                        CurrentDb.MachineStockLog.Add(machineStockLog);
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
