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
            ret.IsUse = machine.IsUse;

            var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == machine.Id && m.IsBind == true).FirstOrDefault();
            if (merchantMachine != null)
            {
                var merchantInfo = CurrentDb.MerchantInfo.Where(m => m.MerchantId == merchantMachine.MerchantId).FirstOrDefault();
                if (merchantInfo != null)
                {
                    ret.Merchant.Id = merchantInfo.MerchantId;
                    ret.Merchant.Name = merchantInfo.Name ?? "";
                    ret.Merchant.ContactName = merchantInfo.ContactName ?? "";
                    ret.Merchant.ContactPhone = merchantInfo.ContactPhone ?? "";
                    ret.Merchant.ContactAddress = merchantInfo.ContactAddress ?? "";
                }
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


        public CustomJsonResult BindOnMerchant(string operater, string merchantId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMachine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();

                if (lMachine.IsUse)
                {
                    return new CustomJsonResult(ResultType.Failure, "该设备已经被绑定，未被解绑");
                }

                lMachine.IsUse = true;
                lMachine.MendTime = this.DateTime;
                lMachine.Mender = operater;

                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId).FirstOrDefault();
                if (lMerchantMachine == null)
                {
                    lMerchantMachine = new MerchantMachine();
                    lMerchantMachine.Id = GuidUtil.New();
                    lMerchantMachine.MerchantId = merchantId;
                    lMerchantMachine.MachineId = machineId;
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.CreateTime = this.DateTime;
                    lMerchantMachine.Creator = operater;
                    CurrentDb.MerchantMachine.Add(lMerchantMachine);
                }
                else
                {
                    lMerchantMachine.IsBind = true;
                    lMerchantMachine.MendTime = this.DateTime;
                    lMerchantMachine.Mender = operater;
                }

                lMerchantMachine.LogoImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/LogoImg.png";
                lMerchantMachine.BtnBuyImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnBuyImg.png";
                lMerchantMachine.BtnPickImgUrl = "http://file.17fanju.com/Upload/machTmp/tmp/BtnPickImg.png";

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "绑定成功");
            }

            return result;
        }

        public CustomJsonResult BindOffMerchant(string operater, string merchantId, string machineId)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var lMerchantMachine = CurrentDb.MerchantMachine.Where(m => m.MerchantId == merchantId && m.MachineId == machineId).FirstOrDefault();

                if (lMerchantMachine.IsBind == false)
                {

                    return new CustomJsonResult(ResultType.Failure, "该设备已经被解绑");
                }

                lMerchantMachine.IsBind = false;
                lMerchantMachine.MendTime = this.DateTime;
                lMerchantMachine.Mender = operater;

                var machine = CurrentDb.Machine.Where(m => m.Id == machineId).FirstOrDefault();

                machine.IsUse = false;
                machine.MendTime = this.DateTime;
                machine.Mender = operater;

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, "解绑成功");
            }

            return result;
        }
    }

}
