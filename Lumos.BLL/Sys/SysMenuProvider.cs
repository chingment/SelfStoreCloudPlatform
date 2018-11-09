using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Sys
{
    public class SysMenuProvider : BaseProvider
    {
        public CustomJsonResult GetPermissions(string pOperater)
        {
            var ret = new RetSysMenuGetPermissions();

            ret.Permissions = SysFactory.AuthorizeRelay.GetPermissionList(new PermissionCode());

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult GetDetails(string pOperater, string menuId)
        {
            var ret = new RetSysMenuGetDetails();

            var menu = CurrentDb.SysMenu.Where(m => m.Id == menuId).FirstOrDefault();

            ret.MenuId = menu.Id;
            ret.Name = menu.Name;
            ret.Url = menu.Url;
            ret.Description = menu.Description;

            var sysMenuPermission = CurrentDb.SysMenuPermission.Where(u => u.MenuId == menuId).ToList();
            var permissionIdIds = (from p in sysMenuPermission select p.PermissionId).ToArray();

            ret.PermissionIds = permissionIdIds;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult Add(string pOperater, RopSysMenuAdd rop)
        {
            var sysMenu = new SysMenu();
            sysMenu.Name = rop.Name;
            sysMenu.Url = rop.Url;
            sysMenu.Description = rop.Description;
            sysMenu.PId = rop.PMenuId;
            return SysFactory.AuthorizeRelay.CreateMenu(pOperater, sysMenu, rop.PermissionIds);
        }


        public CustomJsonResult Edit(string pOperater, RopSysMenuEdit rop)
        {
            var sysMenu = new SysMenu();
            sysMenu.Id = rop.MenuId;
            sysMenu.Name = rop.Name;
            sysMenu.Url = rop.Url;
            sysMenu.Description = rop.Description;

            return SysFactory.AuthorizeRelay.UpdateMenu(pOperater, sysMenu, rop.PermissionIds);

        }


        public CustomJsonResult Delete(string pOperater, string[] menuIds)
        {
            return SysFactory.AuthorizeRelay.DeleteMenu(pOperater, menuIds);
        }

        public CustomJsonResult EditSort(string pOperater, RopSysMenuEditSort rop)
        {
            if (rop != null)
            {
                if (rop.Dics != null)
                {
                    foreach (var item in rop.Dics)
                    {
                        string menuId = item.MenuId;
                        int priority = item.Priority;
                        SysMenu model = CurrentDb.SysMenu.Where(m => m.Id == menuId).FirstOrDefault();
                        if (model != null)
                        {
                            model.Priority = priority;
                            CurrentDb.SaveChanges();
                        }
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

        }
    }
}
