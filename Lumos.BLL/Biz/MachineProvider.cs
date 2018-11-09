using Lumos.BLL.Biz.RModels;
using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Biz
{
    public class MachineProvider : BaseProvider
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
            machine.Creator = pOperater;
            CurrentDb.Machine.Add(machine);
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "登记成功");
        }

        public CustomJsonResult Edit(string pOperater, RopMachineEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();

            var machine = CurrentDb.Machine.Where(m => m.Id == rop.MachineId).FirstOrDefault();
            if (machine == null)
                return new CustomJsonResult(ResultType.Failure, "不存在");

            machine.Name = rop.Name;
            machine.MacAddress = rop.MacAddress;
            machine.MendTime = this.DateTime;
            machine.Mender = pOperater;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "保存成功");
        }


        public CustomJsonResult LoginResultQuery(string pOperater, RupMachineLoginResultQuery rup)
        {
            var key = string.Format("machineLoginResult:{0}", rup.Token);

            var redis = new RedisClient<string>();
            var token = redis.KGetString(key);

            if (token == null)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败");


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功");
        }

        public CustomJsonResult LoginByQrCode(string pOperater, RopMachineLoginByQrCode rop)
        {

            var ret = new RetOperateResult();

            var key = string.Format("machineLoginResult:{0}", rop.Token.ToLower());
            var redis = new RedisClient<string>();
            var isFlag = redis.KSet(key, "true", new TimeSpan(0, 1, 0));
            if (!isFlag)
            {
                ret.Result = RetOperateResult.ResultType.Success;
                ret.Remarks = "";
                ret.Message = "登录失败";
                ret.IsComplete = true;
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败", ret);
            }
            else
            {
                ret.Result = RetOperateResult.ResultType.Success;
                ret.Remarks = "";
                ret.Message = "登录成功";
                ret.IsComplete = true;
                return new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功", ret);
            }
        }
    }

}
