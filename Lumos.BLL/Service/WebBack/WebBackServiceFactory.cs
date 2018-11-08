using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class WebBackServiceFactory
    {
        public static SysStaffUserService SysStaffUser
        {
            get
            {
                return new SysStaffUserService();
            }
        }

        public static SysUserService SysUser
        {
            get
            {
                return new SysUserService();
            }
        }

        public static SysRoleService SysRole
        {
            get
            {
                return new SysRoleService();
            }
        }

        public static SysMenuService SysMenu
        {
            get
            {
                return new SysMenuService();
            }
        }
    }
}
