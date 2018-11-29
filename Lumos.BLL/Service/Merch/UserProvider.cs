using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Merch
{
    public class UserProvider : BaseProvider
    {
        public List<string> GetPermissions(string operater,string merchantId, string id)
        {
            List<string> list = new List<string>();


            var adminUser = CurrentDb.SysMerchantUser.Where(m => m.MerchantId == merchantId&&m.Id== id).FirstOrDefault();
            if (adminUser == null)
                return list;


            var model = (from sysMenuPermission in CurrentDb.SysMenuPermission
                         where
                             (from sysRoleMenu in CurrentDb.SysRoleMenu
                              where
                              (from sysPositionRole in CurrentDb.SysPositionRole
                               where sysPositionRole.PositionId == adminUser.PositionId
                               select sysPositionRole.RoleId)
                              .Contains(sysRoleMenu.RoleId)
                              select sysRoleMenu.MenuId).Contains(sysMenuPermission.MenuId)
                         select sysMenuPermission.PermissionId).Distinct();

            if (model != null)
            {
                list = model.ToList();
            }

            return list;
        }
    }
}
