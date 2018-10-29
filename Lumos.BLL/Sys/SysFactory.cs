using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SysFactory : BaseFactory
    {
        public static SysAppInfoProvider AppInfo
        {
            get
            {
                return new SysAppInfoProvider();
            }
        }

        public static SysUserProvider SysUser
        {
            get
            {
                return new SysUserProvider();
            }
        }

        public static SysItemCacheUpdateTimeProvider SysItemCacheUpdateTime
        {
            get
            {
                return new SysItemCacheUpdateTimeProvider();
            }
        }

        public static AuthorizeRelayProvider AuthorizeRelay
        {
            get
            {
                return new AuthorizeRelayProvider();
            }
        }
    }
}
