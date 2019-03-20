using Lumos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySDK
{

    public interface IPushService
    {
        CustomJsonResult Send(string registrationid, string cmd, object data);
    }
}
