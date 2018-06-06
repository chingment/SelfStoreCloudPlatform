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
    public class MachineProvider : BaseProvider
    {
        public CustomJsonResult Add(string operater, Machine machine)
        {
            CustomJsonResult result = new CustomJsonResult();

            var l_posMachine = CurrentDb.Machine.Where(m => m.DeviceId == machine.DeviceId).FirstOrDefault();
            if (l_posMachine != null)
                return new CustomJsonResult(ResultType.Failure, "该POS机设备ID已经登记");


            machine.CreateTime = this.DateTime;
            machine.Creator = operater;


            CurrentDb.Machine.Add(machine);
            CurrentDb.SaveChanges();

           // machine.Sn = SnUtil.Build(machine.Id);
            CurrentDb.SaveChanges();


            return new CustomJsonResult(ResultType.Success, "登记成功");
        }

        public CustomJsonResult Edit(string operater, Machine machine)
        {
            CustomJsonResult result = new CustomJsonResult();

            var l_machine = CurrentDb.Machine.Where(m => m.Id == machine.Id).FirstOrDefault();
            if (l_machine == null)
                return new CustomJsonResult(ResultType.Failure, "不存在");

            l_machine.Name = machine.Name;
            l_machine.MacAddress = machine.MacAddress;
            l_machine.LastUpdateTime = this.DateTime;
            l_machine.Mender = operater;
            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, "保存成功");
        }

    }

}
