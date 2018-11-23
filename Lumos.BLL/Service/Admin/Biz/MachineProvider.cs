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
            if (machine != null)
            {
                ret.Id = machine.Id ?? ""; ;
                ret.Name = machine.Name ?? ""; ;
                ret.DeviceId = machine.DeviceId ?? ""; ;
                ret.MacAddress = machine.MacAddress ?? "";
                ret.IsUse = machine.IsUse;

                var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == machine.Id && m.IsBind == true).FirstOrDefault();
                if (merchantMachine != null)
                {
                    var sysMerchantUser = CurrentDb.SysMerchantUser.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
                    if (sysMerchantUser != null)
                    {
                        ret.Merchant.Id = sysMerchantUser.Id;
                        ret.Merchant.Name = sysMerchantUser.MerchantName ?? "";
                        ret.Merchant.ContactName = sysMerchantUser.ContactName ?? "";
                        ret.Merchant.ContactPhone = sysMerchantUser.ContactPhone ?? "";
                        ret.Merchant.ContactAddress = sysMerchantUser.ContactAddress ?? "";
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string operater, RopMachineAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var lPosMachine = CurrentDb.Machine.Where(m => m.DeviceId == rop.DeviceId).FirstOrDefault();
            if (lPosMachine != null)
                return new CustomJsonResult(ResultType.Failure, "该POS机设备ID已经登记");

            var machine = new Machine();
            machine.Id = GuidUtil.New();
            machine.Name = rop.Name;
            machine.DeviceId = rop.DeviceId;
            machine.MacAddress = rop.MacAddress;
            machine.CreateTime = this.DateTime;
            machine.Creator = operater;
            CurrentDb.Machine.Add(machine);
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "登记成功");
        }

        public CustomJsonResult Edit(string operater, RopMachineEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.Id == rop.Id).FirstOrDefault();
            if (machine == null)
                return new CustomJsonResult(ResultType.Failure, "不存在");

            machine.Name = rop.Name;
            machine.MacAddress = rop.MacAddress;
            machine.MendTime = this.DateTime;
            machine.Mender = operater;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "保存成功");
        }


    }

}
