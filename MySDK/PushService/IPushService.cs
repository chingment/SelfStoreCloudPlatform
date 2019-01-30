using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{
    public enum PushDataType
    {

    }

    public interface IPushService
    {
        CustomJsonResult SendPush(PushDataType type,string registrationid, object data);
    }
}
