using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.App
{
    public enum Caller
    {
        Unknow = 0,
        NativeWebMoblie = 1,
        NativeMiniProgram = 2
    }

    public enum PayWay
    {
        Unknow = 0,
        Wechat = 1,
        AliPay = 2
    }

    public class RupOrderGetJsApiPaymentPms
    {
        public string OrderId { get; set; }

        public PayWay PayWay { get; set; }

    }
}
