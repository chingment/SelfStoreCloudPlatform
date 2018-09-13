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
        public CustomJsonResult<Order> UnifiedOrder(string pOperater, string pUserId, UnifiedOrderPms pPayPms)
        {
            CustomJsonResult<Order> result = new CustomJsonResult<Order>();
            lock (lock_UnifiedOrder)
            {
                var strOrderPms = Newtonsoft.Json.JsonConvert.SerializeObject(pPayPms.OrderPms);
          
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
                    mod_OrderNotifyLog.UserId = order.UserId;
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


                if (order.IsPromoteProfit)
                {
                    //var promoteUserRelations = CurrentDb.PromoteUserRelation.Where(m => m.CUserId == order.UserId && m.PromoteId == order.PromoteId).ToList();
                    //var promoteProfitRates = CurrentDb.PromoteProfitRate.Where(m => m.PromoteId == order.PromoteId).ToList();
                    //foreach (var item in promoteUserRelations)
                    //{
                    //    var promoteProfitRate = promoteProfitRates.Where(m => m.Dept == item.Dept).FirstOrDefault();

                    //    var fund = CurrentDb.Fund.Where(m => m.UserId == item.UserId).FirstOrDefault();

                    //    var profit = order.ChargeAmount * (promoteProfitRate.Rate / 100);
                    //    fund.Balance += profit;
                    //    fund.Mender = pOperater;
                    //    fund.MendTime = this.DateTime;

                    //    var fundTrans = new FundTrans();
                    //    fundTrans.Id = GuidUtil.New();
                    //    fundTrans.Sn = SnUtil.Build(Enumeration.BizSnType.Order);
                    //    fundTrans.UserId = fund.UserId;
                    //    fundTrans.ChangeType = Enumeration.FundTransChangeType.PromoteProfit;
                    //    fundTrans.ChangeAmount = profit;
                    //    fundTrans.Balance = fund.Balance;
                    //    fundTrans.CreateTime = this.DateTime;
                    //    fundTrans.Creator = pOperater;
                    //    fundTrans.Description = string.Format("下级支付{0}元购买优惠卷，作为上级获得收益{1}元", order.ChargeAmount, profit);
                    //    CurrentDb.FundTrans.Add(fundTrans);
                    //    CurrentDb.SaveChanges();

                    //}

       
                }

                CurrentDb.SaveChanges();
                ts.Complete();


                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, string.Format("支付完成通知：订单号({0})通知成功", pOrderSn));
            }

            return result;
        }


    }
}
