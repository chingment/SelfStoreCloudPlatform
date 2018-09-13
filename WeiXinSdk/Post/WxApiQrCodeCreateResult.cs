using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiQrCodeCreateResult : WxApiBaseResult
    {
        public string ticket { get; set; }
        public int expire_seconds { get; set; }
        public string url { get; set; }
    }
}
