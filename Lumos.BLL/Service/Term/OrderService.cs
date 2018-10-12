using Lumos.BLL.Service.Term.Models;
using Lumos.BLL.Task;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Term
{

    public class OrderService : BaseProvider
    {
        public CustomJsonResult Reserve(string pOperater, RopOrderReserve rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            Biz.RModels.RopOrderReserve bizRop = new Biz.RModels.RopOrderReserve();
            bizRop.Source = Enumeration.OrderSource.Machine;
            bizRop.StoreId = rop.StoreId;
            bizRop.MerchantId = rop.MerchantId;
            bizRop.PayTimeout = rop.PayTimeout;
            bizRop.ReserveMode = Enumeration.ReserveMode.OffLine;
            bizRop.ChannelId = rop.MachineId;

            foreach (var item in rop.Details)
            {
                bizRop.Details.Add(new Biz.RModels.RopOrderReserve.Detail() { SkuId = item.SkuId, Quantity = item.Quantity, ReceptionMode = item.ReceptionMode });
            }

            result = BizFactory.Order.Reserve(pOperater, bizRop);

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

        public CustomJsonResult Cancle(string pOperater, RopOrderCancle rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            result = BizFactory.Order.Cancle(pOperater, rop.OrderSn, rop.Reason);

            return result;
        }

    }
}
