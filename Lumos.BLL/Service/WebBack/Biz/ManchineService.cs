using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class ManchineService : BaseProvider
    {
        public CustomJsonResult GetDetails(string pOperater, string manchineId)
        {
            var ret = new RetMachineGetDetails();
            var machine = CurrentDb.Machine.Where(m => m.Id == manchineId).FirstOrDefault();
            if (machine != null)
            {
                ret.MachineId = machine.Id ?? ""; ;
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
                        ret.Merchant.MerchantId = sysMerchantUser.Id;
                        ret.Merchant.Name = sysMerchantUser.MerchantName ?? "";
                        ret.Merchant.ContactName = sysMerchantUser.ContactName ?? "";
                        ret.Merchant.ContactPhone = sysMerchantUser.ContactPhone ?? "";
                        ret.Merchant.ContactAddress = sysMerchantUser.ContactAddress ?? "";
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, RopMachineAdd rop)
        {
            var machine = new Machine();
            machine.Name = rop.Name;
            machine.DeviceId = rop.DeviceId;
            machine.MacAddress = rop.MacAddress;
            return BizFactory.Machine.Add(pOperater, machine);
        }

        public CustomJsonResult Edit(string pOperater, RopMachineEdit rop)
        {
            var machine = new Machine();
            machine.Id = rop.MachineId;
            machine.Name = rop.Name;
            machine.DeviceId = rop.DeviceId;
            machine.MacAddress = rop.MacAddress;

            return BizFactory.Machine.Edit(pOperater, machine);
        }
    }
}
