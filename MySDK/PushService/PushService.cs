using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{
    public static class PushService
    {
        private static IPushService pushService = new JgPushService();

        public static CustomJsonResult Send<T>(string regId,string cmd, T data)
        {
            var result = new CustomJsonResult();
            pushService.Send(regId, cmd, data);
            return result;
        }
    }
}
