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
        public CustomJsonResult Add(string operater, Store store)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var existObject = CurrentDb.Store.Where(m => m.UserId == store.UserId && m.Name == store.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }
                store.Id = GuidUtil.New();
                store.Status = Enumeration.StoreStatus.Closed;
                store.CreateTime = this.DateTime;
                store.Creator = operater;
                CurrentDb.Store.Add(store);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

            }
            return result;
        }

        public CustomJsonResult Edit(string operater, Store store)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var l_Store = CurrentDb.Store.Where(m => m.Id == store.Id).FirstOrDefault();

                var existObject = CurrentDb.Store.Where(m => m.UserId == l_Store.UserId && m.Id != store.Id && m.Name == store.Name).FirstOrDefault();
                if (existObject != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "名称已存在,请使用其它");
                }

                if (store.Status == Enumeration.StoreStatus.Opened)
                {
                    var storeMachineBindCount = CurrentDb.StoreMachine.Where(m => m.StoreId == store.Id && m.IsBind == true).Count();
                    if (storeMachineBindCount == 0)
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "设置为正常状态，必须先在机器管理里绑定一台机器");
                    }
                }

                l_Store.Name = store.Name;
                l_Store.Address = store.Address;
                l_Store.Status = store.Status;
                l_Store.LastUpdateTime = this.DateTime;
                l_Store.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }

        public CustomJsonResult BindOnMachine(string operater, string storeId, string[] machineIds)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                if (machineIds == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "没有机器人");
                }

                if (machineIds.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "没有机器人");
                }

                var store = CurrentDb.Store.Where(m => m.Id == storeId).FirstOrDefault();

                if (store == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "没有便利店");
                }

                foreach (var machineId in machineIds)
                {
                    var storeMachine = CurrentDb.StoreMachine.Where(m => m.StoreId == storeId && m.MachineId == machineId).FirstOrDefault();
                    if (storeMachine == null)
                    {
                        storeMachine = new StoreMachine();
                        storeMachine.Id = GuidUtil.New();
                        storeMachine.UserId = store.UserId;
                        storeMachine.MachineId = machineId;
                        storeMachine.StoreId = storeId;
                        storeMachine.IsBind = true;
                        storeMachine.CreateTime = this.DateTime;
                        storeMachine.Creator = operater;
                        CurrentDb.StoreMachine.Add(storeMachine);
                        CurrentDb.SaveChanges();
                    }
                    else
                    {
                        storeMachine.IsBind = true;
                        storeMachine.LastUpdateTime = this.DateTime;
                        storeMachine.Mender = operater;
                        CurrentDb.SaveChanges();
                    }
                }

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "绑定成功");
            }
            return result;
        }

        public CustomJsonResult BindOffMachine(string operater, string storeId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var storeMachines = CurrentDb.StoreMachine.Where(m => m.StoreId == storeId).ToList();

                foreach (var item in storeMachines)
                {
                    if (item.MachineId == machineId)
                    {
                        item.IsBind = false;
                        item.LastUpdateTime = this.DateTime;
                        item.Mender = operater;
                    }
                }

                var storeMachineBindCount = storeMachines.Where(m => m.IsBind == true).Count();
                if (storeMachineBindCount == 0)
                {
                    var store = CurrentDb.Store.Where(m => m.Id == storeId).FirstOrDefault();

                    store.Status = Enumeration.StoreStatus.Closed;
                    store.LastUpdateTime = this.DateTime;
                    store.Mender = operater;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "解绑成功");
            }
            return result;
        }
    }
}
