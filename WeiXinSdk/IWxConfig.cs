using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.WeiXinSdk
{
    public interface IWxConfig
    {
        string AppId { get; set; }
        string MchId { get; set; }
        string Key { get; set; }
        string AppSecret { get; set; }
        string Notify_Url { get; set; }
        string SslCert_Path { get; set; }
        string SslCert_Password { get; set; }
        string Oauth2RedirectUrl { get; set; }

        string NotifyEventUrlToken { get; set; }
    }
}
