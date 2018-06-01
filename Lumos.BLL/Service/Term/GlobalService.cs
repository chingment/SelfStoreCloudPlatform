using Lumos.BLL.Service.Term.Models.Global;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Term
{
    public class GlobalService : BaseProvider
    {
        public CustomJsonResult DataSet(int operater, int merchantId, int machineId, DateTime? datetime)
        {
            CustomJsonResult result = new CustomJsonResult();
            var model = new DataSetModel();


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", model);
        }
    }
}
