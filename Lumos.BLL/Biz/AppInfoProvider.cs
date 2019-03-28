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
        public string GetSecret(string id)
        {
            var appInfo = CurrentDb.SysAppInfo.Where(m => m.AppId == id).FirstOrDefault();
            if (appInfo == null)
                return null;

            return appInfo.AppSecret;
        }
    }
}
