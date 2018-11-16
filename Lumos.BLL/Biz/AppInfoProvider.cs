using Lumos.Entity;
using Lumos.WeiXinSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Biz
{
    public class AppInfoProvider : BaseProvider
    {
        public string GetSecret(string pAppId)
        {
            var appInfo = CurrentDb.SysAppInfo.Where(m => m.AppId == pAppId).FirstOrDefault();
            if (appInfo == null)
                return null;

            return appInfo.AppSecret;
        }

        public AppInfoConfig Get(string pAppId)
        {
            var sysAppInfo = CurrentDb.SysAppInfo.Where(m => m.AppId == pAppId).FirstOrDefault();
            if (sysAppInfo == null)
                return null;

            var appInfo = new AppInfoConfig();
            appInfo.AppId = sysAppInfo.AppId;
            appInfo.AppSecret = sysAppInfo.AppSecret;
            appInfo.AppWxPayMchId = sysAppInfo.AppWxPayMchId;
            appInfo.AppWxPayKey = sysAppInfo.AppWxPayKey;
            appInfo.AppWxPayResultNotifyUrl = sysAppInfo.AppWxPayResultNotifyUrl;
            appInfo.AppWxOauth2RedirectUrl = sysAppInfo.AppWxOauth2RedirectUrl;
            appInfo.AppWxNotifyEventUrlToken = sysAppInfo.AppWxNotifyEventUrlToken;
            return appInfo;
        }


    }
}
