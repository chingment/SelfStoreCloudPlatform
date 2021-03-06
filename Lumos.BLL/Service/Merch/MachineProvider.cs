﻿using Lumos.BLL.Biz;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class MachineProvider : BaseProvider
    {
        public CustomJsonResult Edit(string operater, string merchantId, RopMachineEdit rop)
        {
            var machine = CurrentDb.Machine.Where(m => m.MerchantId == merchantId && m.Id == rop.Id).FirstOrDefault();
            if (machine != null)
            {
                //l_MerchantMachine.ma = rop.Name;
                machine.Mender = operater;
                machine.MendTime = DateTime.Now;
                CurrentDb.SaveChanges();
            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult GetStock(string operater, string merchantId, string id)
        {

            var ret = new RetMachineGetStock();


            var machine = CurrentDb.Machine.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (machine != null)
            {
                ret.Id = id;
                ret.Name = machine.Name;

                if (machine.StoreId == null)
                {
                    ret.IsBindStore = false;
                }
                else
                {
                    ret.IsBindStore = true;

                    var store = CurrentDb.Store.Where(m => m.Id == machine.StoreId).FirstOrDefault();
                    if (store != null)
                    {
                        ret.Store.Id = store.Id;
                        ret.Store.Name = store.Name;
                        ret.Store.Address = store.Address;

                        var skus = from u in CurrentDb.StoreSellStock
                                   where
                                   u.MerchantId == merchantId &&
                                   u.StoreId == store.Id &&
                                   u.ChannelId == id &&
                                   u.ChannelType == Enumeration.ChannelType.Machine
                                   select new { u.Id, u.SlotId, u.ProductSkuId, u.ProductSkuName, u.Quantity, u.LockQuantity, u.SellQuantity, u.SalePrice, u.SalePriceByVip };

                        List<object> olist = new List<object>();
                        foreach (var item in skus)
                        {
                            var skuModel = BizFactory.ProductSku.GetModel(item.ProductSkuId);
                            if (skuModel != null)
                            {
                                ret.Skus.Add(new RetMachineGetStock.SkuModel
                                {
                                    Id = skuModel.Id,
                                    Name = skuModel.Name,
                                    ImgUrl = skuModel.ImgUrl,
                                    SlotId = item.SlotId,
                                    Quantity = item.Quantity,
                                    LockQuantity = item.LockQuantity,
                                    SellQuantity = item.SellQuantity,
                                    SalePrice = item.SalePrice,
                                    SalePriceByVip = item.SalePriceByVip
                                });
                            }
                        }
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult GetDetails(string operater, string merchantId, string id)
        {

            var ret = new RetMachineGetDetails();


            var machine = CurrentDb.Machine.Where(m => m.MerchantId == merchantId && m.Id == id).FirstOrDefault();
            if (machine != null)
            {
                ret.Id = id;
                ret.Name = machine.Name;

                if (machine.StoreId == null)
                {
                    ret.IsBindStore = false;
                }
                else
                {
                    ret.IsBindStore = true;

                    var store = CurrentDb.Store.Where(m => m.Id == machine.StoreId).FirstOrDefault();
                    if (store != null)
                    {
                        ret.Store.Id = store.Id;
                        ret.Store.Name = store.Name;
                        ret.Store.Address = store.Address;
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult BindOnStore(string operater, string merchantId, string storeId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var machine = CurrentDb.Machine.Where(m => m.MerchantId == merchantId && m.Id == machineId).FirstOrDefault();

                if (machine == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                var store = CurrentDb.Store.Where(m => m.MerchantId == merchantId && m.Id == storeId).FirstOrDefault();

                if (store == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "没有便利店");
                }

                machine.StoreId = storeId;
                machine.MendTime = this.DateTime;
                machine.Mender = operater;

                var machineBindLog = new MachineBindLog();
                machineBindLog.Id = GuidUtil.New();
                machineBindLog.MerchantId = merchantId;
                machineBindLog.MachineId = machineId;
                machineBindLog.StoreId = storeId;
                machineBindLog.BindType = Enumeration.MachineBindType.On;
                machineBindLog.CreateTime = this.DateTime;
                machineBindLog.Creator = operater;
                machineBindLog.Description = string.Format("机器[{0}]绑定店铺[{1}]", machine.Id, store.Name);
                CurrentDb.MachineBindLog.Add(machineBindLog);
                CurrentDb.SaveChanges();


                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }
            return result;
        }

        public CustomJsonResult BindOffStore(string operater, string merchantId, string storeId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var machine = CurrentDb.Machine.Where(m => m.MerchantId == merchantId && m.StoreId == storeId && m.Id == machineId).FirstOrDefault();

                if (machine == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                var store = CurrentDb.Store.Where(m => m.MerchantId == merchantId && m.Id == storeId).FirstOrDefault();

                if (store == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                machine.StoreId = null;
                machine.MendTime = this.DateTime;
                machine.Mender = operater;


                var machineBindLog = new MachineBindLog();
                machineBindLog.Id = GuidUtil.New();
                machineBindLog.MerchantId = merchantId;
                machineBindLog.MachineId = machineId;
                machineBindLog.StoreId = storeId;
                machineBindLog.BindType = Enumeration.MachineBindType.Off;
                machineBindLog.CreateTime = this.DateTime;
                machineBindLog.Creator = operater;
                machineBindLog.Description = string.Format("机器[{0}]解绑店铺[{1}]", machine.Id, store.Name);
                CurrentDb.MachineBindLog.Add(machineBindLog);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }
            return result;
        }
    }
}
