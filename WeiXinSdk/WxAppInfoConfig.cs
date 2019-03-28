using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class WxAppInfoConfig
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string PayMchId { get; set; }
        public string PayKey { get; set; }
        public string PayResultNotifyUrl { get; set; }
        public string NotifyEventUrlToken { get; set; }
        public string SslCert_Path { get; set; }
        public string SslCert_Password { get; set; }
        public string Oauth2RedirectUrl { get; set; }
    }
}
