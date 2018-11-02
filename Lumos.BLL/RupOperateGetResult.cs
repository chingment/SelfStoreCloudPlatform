using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public enum RupOperateGetResultType
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("支付")]
        Pay = 1
    }

    public class RupOperateGetResult
    {
        public string Id { get; set; }

        public RupOperateGetResultType Type { get; set; }
    }
}
