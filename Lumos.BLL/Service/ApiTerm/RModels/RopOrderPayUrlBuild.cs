using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.ApiTerm
{
    public enum PayWay
    {
        Unknow = 0,
        Wechat = 1,
        AliPay = 2
    }

    public class RopOrderPayUrlBuild
    {
        public string MachineId { get; set; }
        public string OrderId { get; set; }

        public PayWay PayWay { get; set; }
    }
}
