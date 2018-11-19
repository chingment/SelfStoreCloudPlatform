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
        public CustomJsonResult GetDetails(string pOperater, string pMerchantId, string pStoreId)
        {
            var ret = new RetStoreGetDetails();
            var store = CurrentDb.Store.Where(m => m.MerchantId == pMerchantId && m.Id == pStoreId).FirstOrDefault();
            if (store != null)
            {
                ret.StoreId = store.Id ?? "";
                ret.Name = store.Name ?? "";
                ret.Address = store.Address ?? "";
                ret.Description = store.Description ?? "";
                ret.Status = store.Status;
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "��ȡ�ɹ�", ret);
        }

        public CustomJsonResult Add(string pOperater, string pMerchantId, RopStoreAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var isExistStore = CurrentDb.Store.Where(m => m.MerchantId == pMerchantId && m.Name == rop.Name).FirstOrDefault();
                if (isExistStore != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "�����Ѵ���,��ʹ������");
                }

                var store = new Store();
                store.Id = GuidUtil.New();
                store.MerchantId = pMerchantId;
                store.Name = rop.Name;
                store.Address = rop.Address;
                store.Description = rop.Description;
                store.Status = Enumeration.StoreStatus.Closed;
                store.CreateTime = this.DateTime;
                store.Creator = pOperater;
                CurrentDb.Store.Add(store);
                CurrentDb.SaveChanges();

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "��ӳɹ�");

            }
            return result;
        }

        public CustomJsonResult Edit(string pOperater, string pMerchantId, RopStoreEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {

                var isExistStore = CurrentDb.Store.Where(m => m.MerchantId == pMerchantId && m.Id != rop.StoreId && m.Name == rop.Name).FirstOrDefault();
                if (isExistStore != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "�����Ѵ���,��ʹ������");
                }

                var store = CurrentDb.Store.Where(m => m.Id == rop.StoreId).FirstOrDefault();

                if (rop.Status == Enumeration.StoreStatus.Opened)
                {
                    var storeMachineBindCount = CurrentDb.StoreMachine.Where(m => m.StoreId == rop.StoreId && m.IsBind == true).Count();
                    if (storeMachineBindCount == 0)
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "����Ϊ����״̬���������ڻ����������һ̨����");
                    }
                }

                store.Name = rop.Name;
                store.Address = rop.Address;
                store.Status = rop.Status;
                store.MendTime = this.DateTime;
                store.Mender = pOperater;
                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "����ɹ�");
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
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "û�л�����");
                }

                if (pMachineIds.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "û�л�����");
                }

                var store = CurrentDb.Store.Where(m => m.Id == pStoreId).FirstOrDefault();

                if (store == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "û�б�����");
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
                        var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();
                        storeMachine.MachineName = machine.Name;
                        storeMachine.IsBind = true;
                        storeMachine.MendTime = this.DateTime;
                        storeMachine.Mender = pOperater;
                        CurrentDb.SaveChanges();
                    }
                }

                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "�󶨳ɹ�");
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
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "���ɹ�");
            }
            return result;
        }


    }
}
