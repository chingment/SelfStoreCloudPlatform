using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SysAppKeySecretProvider : BaseProvider
    {
        public string GetSecret(string key)
        {
            var sysAppKeySecret = CurrentDb.SysAppKeySecret.Where(m => m.Key == key).FirstOrDefault();
            if (sysAppKeySecret == null)
                return null;

            return sysAppKeySecret.Secret;
        }
    }
}
