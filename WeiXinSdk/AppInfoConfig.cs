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
        public string MchId { get; set; }
        public string Key { get; set; }
        public string AppSecret { get; set; }
        public string Notify_Url { get; set; }
        public string SslCert_Path { get; set; }
        public string SslCert_Password { get; set; }
        public string Oauth2RedirectUrl { get; set; }
        public string NotifyEventUrlToken { get; set; }
    }
}
