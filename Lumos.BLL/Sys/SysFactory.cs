using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class SysFactory : BaseFactory
    {
        public static SysAppKeySecretProvider AppKeySecret
        {
            get
            {
                return new SysAppKeySecretProvider();
            }
        }

        public static SysUserProvider SysUser
        {
            get
            {
                return new SysUserProvider();
            }
        }

        public static SysBannerProvider Banner
        {
            get
            {
                return new SysBannerProvider();
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
