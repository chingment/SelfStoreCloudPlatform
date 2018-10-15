using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public class OperateService : BaseProvider
    {
        public CustomJsonResult GetResult(string pOperater, string pClient, RupOperateGetResult rup)
        {
            var result = new CustomJsonResult();

            switch (rup.Type)
            {
                case RupOperateGetResultType.Pay:
                    result = PayResult(pOperater, rup);
                    break;
            }

            return result;
        }


        private CustomJsonResult PayResult(string pOperater, RupOperateGetResult rup)
        {
            var result = new CustomJsonResult();


            switch (rup.Type)
            {
                case RupOperateGetResultType.Pay:

                    break;
            }
            var ret = new RetPayResult();


            var order = CurrentDb.Order.Where(m => m.Id == rup.Id).FirstOrDefault();

            if (order == null)
            {
                ret.Result = RetPayResult.ResultType.Failure;
                ret.Message = "系统找不到该订单号";

                return new CustomJsonResult(ResultType.Success, ResultCode.StopTask, "查询支付结果失败：找不到该订单", ret);
            }


            switch (order.Status)
            {
                case Enumeration.OrderStatus.Submitted:
                    ret.Result = RetPayResult.ResultType.Failure;
                    ret.Message = "该订单未支付";
                    result = new CustomJsonResult(ResultType.Success, ResultCode.StopTask, "订单未支付", ret);
                    break;
                case Enumeration.OrderStatus.WaitPay:
                    ret.Result = RetPayResult.ResultType.Failure;
                    ret.Message = "该订单未支付";
                    result = new CustomJsonResult(ResultType.Success, ResultCode.ContinueTask, "订单未支付", ret);
                    break;
                case Enumeration.OrderStatus.Payed:
                    ret.Remarks = "";
                    ret.Message = "支付成功";
                    ret.Buttons.Add(new RetPayResult.Button() { Name = "回到首页", Color = "red", Url = "" });
                    ret.Buttons.Add(new RetPayResult.Button() { Name = "查看详情", Color = "green", Url = "" });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "订单号", Value = order.Sn });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "提交时间", Value = order.SubmitTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "支付时间", Value = order.PayTime.ToUnifiedFormatDateTime() });
                    result = new CustomJsonResult(ResultType.Success, ResultCode.StopTask, "操作成功", ret);
                    break;
                case Enumeration.OrderStatus.Completed:
                    ret.Result = RetPayResult.ResultType.Success;
                    ret.Message = "该订单已经完成";
                    ret.Buttons.Add(new RetPayResult.Button() { Name = "回到首页", Color = "red", Url = "" });
                    ret.Buttons.Add(new RetPayResult.Button() { Name = "查看详情", Color = "green", Url = "" });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "订单号", Value = order.Sn });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "提交时间", Value = order.SubmitTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "支付时间", Value = order.PayTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "完成时间", Value = order.CompletedTime.ToUnifiedFormatDateTime() });
                    result = new CustomJsonResult(ResultType.Success, ResultCode.StopTask, "订单已经完成", ret);
                    break;
                case Enumeration.OrderStatus.Cancled:
                    ret.Result = RetPayResult.ResultType.Success;
                    ret.Message = "该订单已经取消";
                    ret.Buttons.Add(new RetPayResult.Button() { Name = "回到首页", Color = "red", Url = "" });
                    ret.Buttons.Add(new RetPayResult.Button() { Name = "查看详情", Color = "green", Url = "" });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "订单号", Value = order.Sn });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "提交时间", Value = order.SubmitTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "取消时间", Value = order.CancledTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetPayResult.Field() { Name = "取消原因", Value = order.CancelReason });
                    result = new CustomJsonResult(ResultType.Success, ResultCode.StopTask, "订单已经取消", ret);
                    break;
                default:
                    break;
            }



            return result;
        }
    }
}
