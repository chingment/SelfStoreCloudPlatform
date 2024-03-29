﻿using Lumos.BLL.Biz;
using Lumos.BLL.Task;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{

    public class OrderProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {
            var ret = new RetOrderGetDetails();

            var order = CurrentDb.Order.Where(m => m.Id == id).FirstOrDefault();

            ret.Sn = order.Sn;
            ret.SourceName = order.Source.GetCnName();
            ret.Status = (int)order.Status;
            ret.StatusName = order.Status.GetCnName();
            ret.StoreName = order.StoreName;
            ret.ClientUserName = order.ClientUserName;
            ret.ClientUserId = order.ClientUserId;
            ret.ChargeAmount = order.ChargeAmount.ToF2Price();
            ret.DiscountAmount = order.DiscountAmount.ToF2Price();
            ret.OriginalAmount = order.OriginalAmount.ToF2Price();
            ret.SubmitTime = order.SubmitTime.ToUnifiedFormatDateTime();

            var orderDetails = CurrentDb.OrderDetails.Where(m => m.OrderId == order.Id).ToList();

            foreach (var item in orderDetails)
            {
                var block = new RetOrderGetDetails.Block();
                block.Name = item.ChannelName;


                var orderDetailsChild = CurrentDb.OrderDetailsChild.Where(m => m.OrderDetailsId == item.Id).ToList();

                foreach (var item2 in orderDetailsChild)
                {
                    var blockSku = new RetOrderGetDetails.BlockSku();

                    blockSku.Name = item2.ProductSkuName;
                    blockSku.Quantity = item2.Quantity;
                    blockSku.ChargeAmount = item2.ChargeAmount.ToF2Price();
                    blockSku.DiscountAmount = item2.DiscountAmount.ToF2Price();
                    blockSku.OriginalAmount = item2.OriginalAmount.ToF2Price();
                    blockSku.StatusName = item2.Status.GetCnName();

                    var orderDetailsChildSon = CurrentDb.OrderDetailsChildSon.Where(m => m.OrderDetailsChildId == item2.Id).ToList();

                    foreach (var item3 in orderDetailsChildSon)
                    {

                        blockSku.BlockSubSkus.Add(new RetOrderGetDetails.BlockSku.BlockSubSku { StatusName = item3.Status.GetCnName(), Quantity = item3.Quantity });
                    }


                    block.Skus.Add(blockSku);
                }

                ret.Blocks.Add(block);

            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }
    }
}
