using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class OpenIds
    {
        public List<string> openid { get; set; }
    }

    public class WxApiUserGetResult : WxApiBaseResult
    {
        public int total { get; set; }

        public int count { get; set; }

        public OpenIds data { get; set; }

        public string next_openid { get; set; }
    }
}
