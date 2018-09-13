using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.MsgPush
{
    public abstract class BaseEventMsg
    {
        public string ToUserName { get; set; }
        public string FromUserName { get; set; }
        public int CreateTime { get; set; }
        public MsgType MsgType { get; set; }
        public long MsgId { get; set; }
        public EventType Event { get; set; }
        public virtual string EventKey { get; set; }
    }
}
