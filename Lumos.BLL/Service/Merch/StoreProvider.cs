using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Merch
{
    public class StoreProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string merchantId, string storeId)
        {
            var ret = new RetStoreGetDetails();
            var store = CurrentDb.Store.Where(m => m.MerchantId == merchantId && m.Id == storeId).FirstOrDefault();
            if (store != null)
            {
                ret.Id = store.Id ?? "";
                ret.Name = store.Name ?? "";
                ret.Address = store.Address ?? "";
                ret.Description = store.Description ?? "";
                ret.Status = store.Status;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "�����ɹ�", ret);
        }

        public CustomJsonResult Add(string operater, string merchantId, RopStoreAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var isExistStore = CurrentDb.Store.Where(m => m.MerchantId == merchantId && m.Name == rop.Name).FirstOrDefault();
                if (isExistStore != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "�����Ѵ���,��ʹ������");
                }

                var store = new Store();
                store.Id = GuidUtil.New();
                store.MerchantId = merchantId;
                store.Name = rop.Name;
                store.Address = rop.Address;
                store.Description = rop.Description;
                store.Status = Enumeration.StoreStatus.Closed;
                store.CreateTime = this.DateTime;
                store.Creator = operater;
                CurrentDb.Store.Add(store);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "�����ɹ�");

            }
            return result;
        }

        public CustomJsonResult Edit(string operater, string merchantId, RopStoreEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {

                var isExistStore = CurrentDb.Store.Where(m => m.MerchantId == merchantId && m.Id != rop.Id && m.Name == rop.Name).FirstOrDefault();
                if (isExistStore != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "�����Ѵ���,��ʹ������");
                }

                var store = CurrentDb.Store.Where(m => m.Id == rop.Id).FirstOrDefault();

                if (rop.Status == Enumeration.StoreStatus.Opened)
                {
                    var storeMachineBindCount = CurrentDb.StoreMachine.Where(m => m.StoreId == rop.Id && m.IsBind == true).Count();
                    if (storeMachineBindCount == 0)
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "����Ϊ����״̬���������ڻ����������һ̨����");
                    }
                }

                store.Name = rop.Name;
                store.Address = rop.Address;
                store.Status = rop.Status;
                store.MendTime = this.DateTime;
                store.Mender = operater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "�����ɹ�");
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
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "û�л�����");
                }

                if (machineIds.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "û�л�����");
                }

                var store = CurrentDb.Store.Where(m => m.Id == storeId).FirstOrDefault();

                if (store == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "û�б�����");
                }

                foreach (var machineId in machineIds)
                {
                    var storeMachine = CurrentDb.StoreMachine.Where(m => m.StoreId == storeId && m.MachineId == machineId).FirstOrDefault();
                    if (storeMachine == null)
                    {
                        storeMachine = new StoreMachine();
                        storeMachine.Id = GuidUtil.New();
                        storeMachine.MerchantId = store.MerchantId;
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
                        var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();
                        storeMachine.MachineName = machine.Name;
                        storeMachine.IsBind = true;
                        storeMachine.MendTime = this.DateTime;
                        storeMachine.Mender = operater;
                        CurrentDb.SaveChanges();
                    }
                }

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "�����ɹ�");
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
                        item.MendTime = this.DateTime;
                        item.Mender = operater;
                    }
                }

                var storeMachineBindCount = storeMachines.Where(m => m.IsBind == true).Count();
                if (storeMachineBindCount == 0)
                {
                    var store = CurrentDb.Store.Where(m => m.Id == storeId).FirstOrDefault();

                    store.Status = Enumeration.StoreStatus.Closed;
                    store.MendTime = this.DateTime;
                    store.Mender = operater;
                }

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "�����ɹ�");
            }
            return result;
        }


    }
}
