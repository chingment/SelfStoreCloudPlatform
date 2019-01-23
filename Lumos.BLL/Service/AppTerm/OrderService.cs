using Lumos.BLL.Biz;
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

    public class OrderService : BaseProvider
    {
        public CustomJsonResult Reserve(string operater, RopOrderReserve rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            Biz.RopOrderReserve bizRop = new Biz.RopOrderReserve();
            bizRop.Source = Enumeration.OrderSource.Machine;
            bizRop.StoreId = rop.StoreId;
            bizRop.PayTimeout = rop.PayTimeout;
            bizRop.ReserveMode = Enumeration.ReserveMode.OffLine;
            bizRop.ChannelId = rop.MachineId;
            bizRop.ChannelType = Enumeration.ChannelType.Machine;

            foreach (var item in rop.Skus)
            {
                bizRop.Skus.Add(new Biz.RopOrderReserve.Sku() { Id = item.Id, Quantity = item.Quantity, ReceptionMode = Enumeration.ReceptionMode.Machine });
            }

            var bizResult = BizFactory.Order.Reserve(operater, bizRop);

            if (bizResult.Result == ResultType.Success)
            {
                RetOrderReserve ret = new RetOrderReserve();
                ret.OrderSn = bizResult.Data.OrderSn;
                ret.PayUrl = string.Format("http://mobile.17fanju.com/Order/Confirm?soure=machine&orderId=" + bizResult.Data.OrderId);

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
            }
            else
            {
                result = new CustomJsonResult(ResultType.Failure, ResultCode.Failure, bizResult.Message);
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

        public CustomJsonResult Cancle(string operater, RopOrderCancle rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.Order.Cancle(operater, rop.OrderSn, rop.Reason);

            return result;
        }

        public CustomJsonResult GetPickupList(string operater, RupOrderGetPickupList rup)
        {
            CustomJsonResult result = new CustomJsonResult();

            //result = BizFactory.Order.Cancle(operater, rop.OrderSn, rop.Reason);

            return result;
        }

        public CustomJsonResult PickupStatusQuery(string operater, RupOrderPickupStatusQuery rup)
        {
            CustomJsonResult result = new CustomJsonResult();

            //result = BizFactory.Order.Cancle(operater, rop.OrderSn, rop.Reason);

            return result;
        }

        public CustomJsonResult PickupStatusNotify(string operater, RopOrderPickupStatusNotify rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            //result = BizFactory.Order.Cancle(operater, rop.OrderSn, rop.Reason);

            return result;
        }

    }
}
