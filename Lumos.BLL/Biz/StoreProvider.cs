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
                store.Id = GuidUtil.New();
                store.Status = Enumeration.StoreStatus.Setting;
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
                l_Store.Name = store.Name;
                l_Store.Address = store.Address;
                l_Store.LastUpdateTime = this.DateTime;
                l_Store.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }

        public CustomJsonResult BindMachine(string operater, string storeId, string[] machineIds)
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
                        storeMachine.MerchantId = store.MerchantId;
                        storeMachine.MachineId = machineId;
                        storeMachine.StoreId = storeId;
                        storeMachine.Status = Enumeration.StoreMachineStatus.Bind;
                        storeMachine.CreateTime = this.DateTime;
                        storeMachine.Creator = operater;
                        CurrentDb.StoreMachine.Add(storeMachine);
                        CurrentDb.SaveChanges();
                    }
                    else
                    {
                        storeMachine.Status = Enumeration.StoreMachineStatus.Bind;
                        storeMachine.LastUpdateTime = this.DateTime;
                        storeMachine.Mender = operater;
                        CurrentDb.SaveChanges();
                    }
                }

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }
            return result;
        }

    }
}
