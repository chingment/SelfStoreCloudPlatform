using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lumos.WeiXinSdk;

namespace Lumos.BLL
{
    public class WxConfigByTest : IWxConfig
    {
        private string _appId = "";
        private string _appSecret = "fee895c9923da26a4d42d9c435202b37";
        private string _mchId = "";
        private string _key = "37b016a9569e4f519702696e1274d63a";
        private string _notify_Url = "";
        private string _sslCert_Path = "";
        private string _sslCert_Password = "1486589902";
        private string _oauth2RedirectUrl = "";
        private string _notifyEventUrlToken = "";

        public WxConfigByTest()
        {
            _appId = System.Configuration.ConfigurationManager.AppSettings["custom:WxAppId"];
            _mchId = System.Configuration.ConfigurationManager.AppSettings["custom:WxMchId"];
            _notify_Url = System.Configuration.ConfigurationManager.AppSettings["custom:WxNotifyUrl"];
            _oauth2RedirectUrl = System.Configuration.ConfigurationManager.AppSettings["custom:WxOauth2RedirectUrl"];
            _sslCert_Path = System.Configuration.ConfigurationManager.AppSettings["custom:WxSslCertPath"];
        }


        public string AppId
        {
            get
            {
                return _appId;
            }
            set
            {
                _appId = value;
            }
        }
        public string MchId
        {
            get
            {
                return _mchId;
            }
            set
            {
                _mchId = value;
            }
        }
        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value;
            }
        }
        public string AppSecret
        {
            get
            {
                return _appSecret;
            }
            set
            {
                _appSecret = value;
            }
        }

        public string Notify_Url
        {
            get
            {
                return _notify_Url;
            }
            set
            {
                _notify_Url = value;
            }
        }
        public string SslCert_Path
        {
            get
            {
                return _sslCert_Path;
            }
            set
            {
                _sslCert_Path = value;
            }
        }

        public string SslCert_Password
        {
            get
            {
                return _sslCert_Password;
            }
            set
            {
                _sslCert_Password = value;
            }
        }

        public string Oauth2RedirectUrl
        {
            get
            {
                return _oauth2RedirectUrl;
            }
            set
            {
                _oauth2RedirectUrl = value;
            }
        }

        public string NotifyEventUrlToken
        {
            get
            {
                return _notifyEventUrlToken;
            }
            set
            {
                _notifyEventUrlToken = value;
            }
        }
    }
}
