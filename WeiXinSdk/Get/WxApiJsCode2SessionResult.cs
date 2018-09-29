using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiJsCode2SessionResult : WxApiBaseResult
    {
        public string openid { get; set; }
        public string session_key { get; set; }
    }
}
