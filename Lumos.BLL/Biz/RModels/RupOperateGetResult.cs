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
        [Remark("发起支付成功检查结果")]
        SendPaySuccessCheck = 1,
        [Remark("发起支付取消检查结果")]
        SendPayCancleCheck = 2
    }

    public enum AppCaller
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("小程序")]
        MinProgram = 1
    }

    public class RupOperateGetResult
    {
        public string Id { get; set; }
        public RupOperateGetResultType Type { get; set; }
        public AppCaller Caller { get;set;}
    }
}
