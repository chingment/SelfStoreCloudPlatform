using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiJsApiTicketResult : WxApiBaseResult
    {
        public string ticket { get; set; }

        public int expires_in { get; set; }
    }
}
