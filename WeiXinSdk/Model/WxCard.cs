using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxCardExt
    {
        public string code { get; set; }
        public string openid { get; set; }
        public string timestamp { get; set; }
        public string signature { get; set; }
        public string nonce_str { get; set; }
    }

    public class WxCard
    {
        public string cardId { get; set; }

        public string cardExt { get; set; }

        public string code { get; set; }
    }
}
