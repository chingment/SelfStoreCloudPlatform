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
        [Remark("支付检查")]
        PayCheck = 1,
        [Remark("支付取消")]
        PayCancle = 2,
    }

    public class RupOperateGetResult
    {
        public string Id { get; set; }

        public RupOperateGetResultType Type { get; set; }

    }
}
