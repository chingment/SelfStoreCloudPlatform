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
        CustomJsonResult Send<T>(string registrationid, string cmd, T data);
    }
}
