using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.Tenpay
{
    public interface ITenpayRequest
    {
        SortedDictionary<string, string> DoPost(IWxConfig config, ITenpayPostApi obj, bool isUserCert = false);

        string ReturnContent { get; }

    }
}
