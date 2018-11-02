using Lumos.BLL.Biz.RModels;
using Lumos.Entity;
using Lumos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL
{
    public class MachineProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, Machine pMachine)
        {
            CustomJsonResult result = new CustomJsonResult();

            var lPosMachine = CurrentDb.Machine.Where(m => m.DeviceId == pMachine.DeviceId).FirstOrDefault();
            if (lPosMachine != null)
                return new CustomJsonResult(ResultType.Failure, "该POS机设备ID已经登记");

            pMachine.Id = GuidUtil.New();
            pMachine.CreateTime = this.DateTime;
            pMachine.Creator = pOperater;


            CurrentDb.Machine.Add(pMachine);
            CurrentDb.SaveChanges();

            // machine.Sn = SnUtil.Build(machine.Id);
            CurrentDb.SaveChanges();


            return new CustomJsonResult(ResultType.Success, "登记成功");
        }

        public CustomJsonResult Edit(string pOperater, Machine pMachine)
        {
            CustomJsonResult result = new CustomJsonResult();

            var lMachine = CurrentDb.Machine.Where(m => m.Id == pMachine.Id).FirstOrDefault();
            if (lMachine == null)
                return new CustomJsonResult(ResultType.Failure, "不存在");

            lMachine.Name = pMachine.Name;
            lMachine.MacAddress = pMachine.MacAddress;
            lMachine.MendTime = this.DateTime;
            lMachine.Mender = pOperater;
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
            var key = string.Format("machineLoginResult:{0}", rop.Token.ToLower());

            var redis = new RedisClient<string>();
            var isFlag = redis.KSet(key, "true", new TimeSpan(0, 1, 0));

            if (!isFlag)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "登录失败");
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "登录成功");
        }

    }

}
