using Lumos.BLL.Service.Term.Models;
using Lumos.BLL.Service.Term.Models.Machine;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class MachineService : BaseProvider
    {
        public CustomJsonResult ApiConfig(string operater, string deviceId)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.DeviceId == deviceId).FirstOrDefault();

            if (machine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未入库登记");
            }

            if (!machine.IsUse)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未配置商户");
            }


            var merchantMachine = CurrentDb.MerchantMachine.Where(m => m.MachineId == machine.Id & m.Status == Entity.Enumeration.MerchantMachineStatus.Bind).FirstOrDefault();

            if (merchantMachine == null)
            {
                return new CustomJsonResult(ResultType.Failure, "设备未绑定商户");
            }

            var merchant = CurrentDb.Merchant.Where(m => m.Id == merchantMachine.MerchantId).FirstOrDefault();
            if (merchant == null)
            {
                return new CustomJsonResult(ResultType.Failure, "已绑定商户，却找不到商户信息");
            }

            var model = new ApiConfigModel();

            model.MerchantId = merchant.Id;
            model.MachineId = machine.Id;
            model.ApiHost = merchant.ApiHost;
            model.ApiKey = merchant.ApiKey;
            model.ApiSecret = merchant.ApiSecret;
            model.PayTimeout = merchant.PayTimeout;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }
    }
}
