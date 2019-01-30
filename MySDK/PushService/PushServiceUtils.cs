using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{
    public static class PushServiceUtils
    {
        public static IPushService service = new JPushService();

        public static CustomJsonResult SendPush(PushDataType type, string registrationid, object data)
        {
            var result = new CustomJsonResult();
            service.SendPush(type, registrationid, data);
            return result;
        }
    }
}
