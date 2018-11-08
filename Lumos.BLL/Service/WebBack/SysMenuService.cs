using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class SysMenuService : BaseProvider
    {
        public CustomJsonResult GetInitDataByAddView(string pOperater)
        {
            var ret = new RetSysMenuGetInitDataByAddView();

            ret.Permissions = SysFactory.AuthorizeRelay.GetPermissionList(new PermissionCode());

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
    }
}
