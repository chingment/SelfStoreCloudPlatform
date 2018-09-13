using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{

    public class WxConfigByQyj : IWxConfig
    {
        private string _appId = "wx65f1ec3c268439c8";
        private string _appSecret = "9c827d75ccd716040ad7674d1cecd575";
        private string _mchId = "1503331631";
        private string _key = "153hcmut5t11v5h2fmnyzlzdreocqwfz";
        private string _notify_Url = "http://qyj.17fanju.com/Home/PayResult";
        private string _sslCert_Path = "E:\\Web\\cereson\\WebMobile\\cert\\apiclient_cert.p12";
        private string _sslCert_Password = "1503331631";
        private string _oauth2RedirectUrl = "http://qyj.17fanju.com/Home/Oauth2?returnUrl={0}";
        private string _notifyEventUrlToken = "q92s52no875Kg8S91n574122SS570ssn";

        public WxConfigByQyj()
        {
            //_appId = System.Configuration.ConfigurationManager.AppSettings["custom:WxAppId"];
            //_mchId = System.Configuration.ConfigurationManager.AppSettings["custom:WxMchId"];
            //_notify_Url = System.Configuration.ConfigurationManager.AppSettings["custom:WxNotifyUrl"];
            //_oauth2RedirectUrl = System.Configuration.ConfigurationManager.AppSettings["custom:WxOauth2RedirectUrl"];
            //_sslCert_Path = System.Configuration.ConfigurationManager.AppSettings["custom:WxSslCertPath"];
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
