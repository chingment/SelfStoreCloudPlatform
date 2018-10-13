﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public enum RupOperateResultType
    {
        [Remark("未知")]
        Unknow = 0,
        [Remark("支付")]
        Pay = 1
    }

    public class RupOperateResult
    {
        public string Id { get; set; }

        public RupOperateResultType Type { get; set; }
    }
}
