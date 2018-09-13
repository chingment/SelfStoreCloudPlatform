using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
   public class WxApiCustomMessagePostData
    {
        public string touser { get; set; }

        public string msgtype { get; set; }

        public WxApiCustomTextMessage text { get; set; }
    }

    public class WxApiCustomTextMessage
    {
        public string content { get; set; }
    }

}
