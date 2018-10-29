using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public class AppInfoConfig
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string AppWxPayMchId { get; set; }
        public string AppWxPayKey { get; set; }
        public string AppWxPayResultNotifyUrl { get; set; }
        public string AppWxNotifyEventUrlToken { get; set; }
        public string SslCert_Path { get; set; }
        public string SslCert_Password { get; set; }
        public string AppWxOauth2RedirectUrl { get; set; }
    }
}
