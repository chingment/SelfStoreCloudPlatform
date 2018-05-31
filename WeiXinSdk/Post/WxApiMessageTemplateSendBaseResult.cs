using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxApiMessageTemplateSendBaseResult : WxApiBaseResult
    {
        public string touser { get; set; }

        public string template_id { get; set; }

        public string url { get; set; }

        public virtual object data { get; set; }
    }
}
