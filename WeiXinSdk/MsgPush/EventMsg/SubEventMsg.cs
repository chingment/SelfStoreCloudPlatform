using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk.MsgPush
{
    public class SubEventMsg : BaseEventMsg
    {
        private string _eventkey;
        /// <summary>
        /// 事件KEY值，qrscene_为前缀，后面为二维码的参数值（已去掉前缀，可以直接使用）
        /// </summary>
        public override string EventKey
        {
            get { return _eventkey; }
            set { _eventkey = value.Replace("qrscene_", ""); }
        }
        /// <summary>
        /// 二维码的ticket，可用来换取二维码图片
        /// </summary>
        public string Ticket { get; set; }
    }
}
