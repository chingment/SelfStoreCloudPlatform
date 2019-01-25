﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public class RopOrderPickupEventNotify
    {
        public string MachineId { get; set; }
        public string OrderId { get; set; }
        public string SkuId { get; set; }
        public string SlotId { get; set; }
        public string Code { get; set; }
        public string Remark { get; set; }
    }
}
