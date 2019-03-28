using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.AppTerm
{
    public enum PayWay
    {
        Unknow = 0,
        Wechat = 1,
        AliPay = 2
    }

    public class RopOrderPayQrCodeBuild
    {
        public string OrderId { get; set; }

        public PayWay PayWay { get; set; }
    }
}
