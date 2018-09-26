using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class StoreProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, Store pStore)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var isExistStore = CurrentDb.Store.Where(m => m.MerchantId == pStore.MerchantId && m.Name == pStore.Name).FirstOrDefault();
                if (isExistStore != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                pStore.Id = GuidUtil.New();
                pStore.Status = Enumeration.StoreStatus.Closed;
                pStore.CreateTime = this.DateTime;
                pStore.Creator = pOperater;
                CurrentDb.Store.Add(pStore);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string pOperater, Store pStore)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var lStore = CurrentDb.Store.Where(m => m.Id == pStore.Id).FirstOrDefault();

                var isExistStore = CurrentDb.Store.Where(m => m.MerchantId == lStore.MerchantId && m.Id != pStore.Id && m.Name == pStore.Name).FirstOrDefault();
                if (isExistStore != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }

                if (lStore.Status == Enumeration.StoreStatus.Opened)
                {
                    var storeMachineBindCount = CurrentDb.StoreMachine.Where(m => m.StoreId == pStore.Id && m.IsBind == true).Count();
                    if (storeMachineBindCount == 0)
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "设置为正常状态，必须先在机器管理里绑定一台机器");
                    }
                }

                lStore.Name = pStore.Name;
                lStore.Address = pStore.Address;
                lStore.Status = pStore.Status;
                lStore.MendTime = this.DateTime;
                lStore.Mender = pOperater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }

        public CustomJsonResult BindOnMachine(string pOperater, string pStoreId, string[] pMachineIds)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                if (pMachineIds == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "没有机器人");
                }

                if (pMachineIds.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "没有机器人");
                }

                var store = CurrentDb.Store.Where(m => m.Id == pStoreId).FirstOrDefault();

                if (store == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "没有便利店");
                }

                foreach (var machineId in pMachineIds)
                {
                    var storeMachine = CurrentDb.StoreMachine.Where(m => m.StoreId == pStoreId && m.MachineId == machineId).FirstOrDefault();
                    if (storeMachine == null)
                    {
                        storeMachine = new StoreMachine();
                        storeMachine.Id = GuidUtil.New();
                        storeMachine.MerchantId = store.MerchantId;
                        storeMachine.MachineId = machineId;
                        storeMachine.StoreId = pStoreId;
                        storeMachine.IsBind = true;
                        storeMachine.CreateTime = this.DateTime;
                        storeMachine.Creator = pOperater;
                        CurrentDb.StoreMachine.Add(storeMachine);
                        CurrentDb.SaveChanges();
                    }
                    else
                    {
                        storeMachine.IsBind = true;
                        storeMachine.MendTime = this.DateTime;
                        storeMachine.Mender = pOperater;
                        CurrentDb.SaveChanges();
                    }
                }

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "绑定成功");
            }
            return result;
        }

        public CustomJsonResult BindOffMachine(string pOperater, string pStoreId, string pMachineId)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var lStoreMachines = CurrentDb.StoreMachine.Where(m => m.StoreId == pStoreId).ToList();

                foreach (var item in lStoreMachines)
                {
                    if (item.MachineId == pMachineId)
                    {
                        item.IsBind = false;
                        item.MendTime = this.DateTime;
                        item.Mender = pOperater;
                    }
                }

                var storeMachineBindCount = lStoreMachines.Where(m => m.IsBind == true).Count();
                if (storeMachineBindCount == 0)
                {
                    var store = CurrentDb.Store.Where(m => m.Id == pStoreId).FirstOrDefault();

                    store.Status = Enumeration.StoreStatus.Closed;
                    store.MendTime = this.DateTime;
                    store.Mender = pOperater;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "解绑成功");
            }
            return result;
        }


    }
}
