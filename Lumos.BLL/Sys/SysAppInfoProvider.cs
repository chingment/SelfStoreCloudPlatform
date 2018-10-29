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
            var sysAppKeySecret = CurrentDb.SysAppInfo.Where(m => m.AppId == pAppId).FirstOrDefault();
            if (sysAppKeySecret == null)
                return null;

            return sysAppKeySecret.AppSecret;
        }
    }
}
