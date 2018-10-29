using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SysAppInfoProvider : BaseProvider
    {
        public string GetSecret(string pAppId)
        {
            var appInfo = CurrentDb.SysAppInfo.Where(m => m.AppId == pAppId).FirstOrDefault();
            if (appInfo == null)
                return null;

            return appInfo.AppSecret;
        }

        public SysAppInfo Get(string pAppId)
        {
            var appInfo = CurrentDb.SysAppInfo.Where(m => m.AppId == pAppId).FirstOrDefault();
            if (appInfo == null)
                return null;

            return appInfo;
        }
    }
}
