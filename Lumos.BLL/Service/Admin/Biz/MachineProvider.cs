using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Admin
{
    public class MachineProvider : BaseProvider
    {
        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetMachineGetDetails();
            var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            ret.Id = machine.Id ?? ""; ;
            ret.Name = machine.Name ?? ""; ;
            ret.DeviceId = machine.DeviceId ?? ""; ;
            ret.MacAddress = machine.MacAddress ?? "";

            var merchantInfo = CurrentDb.Merchant.Where(m => m.Id == machine.MerchantId).FirstOrDefault();
            if (merchantInfo != null)
            {
                ret.IsBindMerchant = true;

                ret.Merchant.Id = merchantInfo.Id;
                ret.Merchant.Name = merchantInfo.Name ?? "";
                ret.Merchant.ContactName = merchantInfo.ContactName ?? "";
                ret.Merchant.ContactPhone = merchantInfo.ContactPhone ?? "";
                ret.Merchant.ContactAddress = merchantInfo.ContactAddress ?? "";
            }



            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string operater, RopMachineAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.DeviceId == rop.DeviceId).FirstOrDefault();
            if (machine != null)
                return new CustomJsonResult(ResultType.Failure, "该设备ID已经登记");

            machine = new Machine();
            machine.Id = GuidUtil.New();
            machine.Name = rop.Name;
            machine.DeviceId = rop.DeviceId;
            machine.MacAddress = rop.MacAddress;
            machine.CreateTime = this.DateTime;
            machine.Creator = operater;
            CurrentDb.Machine.Add(machine);
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "操作成功");
        }

        public CustomJsonResult Edit(string operater, RopMachineEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.Id == rop.Id).FirstOrDefault();
            if (machine == null)
                return new CustomJsonResult(ResultType.Failure, "数据为空");

            machine.Name = rop.Name;
            machine.MacAddress = rop.MacAddress;
            machine.MendTime = this.DateTime;
            machine.Mender = operater;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "操作成功");
        }


        public CustomJsonResult BindOnMerchant(string operater, string id, string merchantId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();

                if (!string.IsNullOrEmpty(machine.MerchantId))
                {
                    return new CustomJsonResult(ResultType.Failure, "该设备已经被绑定");
                }

                machine.MerchantId = merchantId;
                machine.LogoImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/LogoImg.png";
                machine.BtnBuyImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnBuyImg.png";
                machine.BtnPickImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnPickImg.png";
                machine.MendTime = this.DateTime;
                machine.Mender = operater;


                var machineBindLog = new MachineBindLog();
                machineBindLog.Id = GuidUtil.New();
                machineBindLog.MerchantId = merchantId;
                machineBindLog.MachineId = id;
                machineBindLog.StoreId = null;
                machineBindLog.BindType = Enumeration.MachineBindType.On;
                machineBindLog.CreateTime = this.DateTime;
                machineBindLog.Creator = operater;
                CurrentDb.MachineBindLog.Add(machineBindLog);
                CurrentDb.SaveChanges();


                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "绑定成功");
            }

            return result;
        }

        public CustomJsonResult BindOffMerchant(string operater, string id, string merchantId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var machine = CurrentDb.Machine.Where(m => m.Id == id).FirstOrDefault();

                if (string.IsNullOrEmpty(machine.MerchantId))
                {
                    return new CustomJsonResult(ResultType.Failure, "该设备已经被解绑");
                }

                machine.MerchantId = null;
                machine.StoreId = null;
                machine.MendTime = this.DateTime;
                machine.Mender = operater;


                var machineBindLog = new MachineBindLog();
                machineBindLog.Id = GuidUtil.New();
                machineBindLog.MerchantId = merchantId;
                machineBindLog.MachineId = id;
                machineBindLog.BindType = Enumeration.MachineBindType.Off;
                machineBindLog.CreateTime = this.DateTime;
                machineBindLog.Creator = operater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "解绑成功");
            }

            return result;
        }
    }

}
