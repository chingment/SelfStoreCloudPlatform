using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiApp
{
    public class OperateService : BaseProvider
    {
        public CustomJsonResult GetResult(string operater, string client, RupOperateGetResult rup)
        {
            var result = new CustomJsonResult();

            switch (rup.Type)
            {
                case RupOperateGetResultType.Pay:
                    result = PayResult(operater, rup);
                    break;
            }

            return result;
        }


        private CustomJsonResult PayResult(string operater, RupOperateGetResult rup)
        {
            var result = new CustomJsonResult();


            switch (rup.Type)
            {
                case RupOperateGetResultType.Pay:

                    break;
            }
            var ret = new RetOperateResult();


            var order = CurrentDb.Order.Where(m => m.Id == rup.Id).FirstOrDefault();

            if (order == null)
            {
                ret.Result = RetOperateResult.ResultType.Failure;
                ret.Message = "系统找不到该订单号";
                ret.IsComplete = true;
                return new CustomJsonResult(ResultType.Success, ResultCode.Success, "查询支付结果失败：找不到该订单", ret);
            }


            switch (order.Status)
            {
                case Enumeration.OrderStatus.Submitted:
                    ret.Result = RetOperateResult.ResultType.Success;
                    ret.Message = "该订单未支付";
                    ret.IsComplete = true;
                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "订单未支付", ret);
                    break;
                case Enumeration.OrderStatus.WaitPay:
                    ret.Result = RetOperateResult.ResultType.Success;
                    ret.IsComplete = false;
                    ret.Message = "该订单未支付";
                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "订单未支付", ret);
                    break;
                case Enumeration.OrderStatus.Payed:
                    ret.Result = RetOperateResult.ResultType.Success;
                    ret.Remarks = "";
                    ret.Message = "支付成功";
                    ret.IsComplete = true;
                    ret.Buttons.Add(new RetOperateResult.Button() { Name = "回到首页", Color = "red", Url = "" });
                    ret.Buttons.Add(new RetOperateResult.Button() { Name = "查看详情", Color = "green", Url = "" });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "订单号", Value = order.Sn });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "提交时间", Value = order.SubmitTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "支付时间", Value = order.PayTime.ToUnifiedFormatDateTime() });
                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
                    break;
                case Enumeration.OrderStatus.Completed:
                    ret.Result = RetOperateResult.ResultType.Success;
                    ret.Message = "该订单已经完成";
                    ret.IsComplete = true;
                    ret.Buttons.Add(new RetOperateResult.Button() { Name = "回到首页", Color = "red", Url = "" });
                    ret.Buttons.Add(new RetOperateResult.Button() { Name = "查看详情", Color = "green", Url = "" });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "订单号", Value = order.Sn });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "提交时间", Value = order.SubmitTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "支付时间", Value = order.PayTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "完成时间", Value = order.CompletedTime.ToUnifiedFormatDateTime() });
                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "订单已经完成", ret);
                    break;
                case Enumeration.OrderStatus.Cancled:
                    ret.Result = RetOperateResult.ResultType.Success;
                    ret.Message = "该订单已经取消";
                    ret.IsComplete = true;
                    ret.Buttons.Add(new RetOperateResult.Button() { Name = "回到首页", Color = "red", Url = "" });
                    ret.Buttons.Add(new RetOperateResult.Button() { Name = "查看详情", Color = "green", Url = "" });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "订单号", Value = order.Sn });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "提交时间", Value = order.SubmitTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "取消时间", Value = order.CancledTime.ToUnifiedFormatDateTime() });
                    ret.Fields.Add(new RetOperateResult.Field() { Name = "取消原因", Value = order.CancelReason });
                    result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "订单已经取消", ret);
                    break;
                default:
                    break;
            }



            return result;
        }
    }
}
