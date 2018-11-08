using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.WebBack
{
    public class SysRoleService : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, RopSysRoleAdd rop)
        {
            var sysRole = new SysRole();

            sysRole.Name = rop.Name;
            sysRole.Description = rop.Description;

            return SysFactory.AuthorizeRelay.CreateRole(pOperater, sysRole);
        }

        public CustomJsonResult Edit(string pOperater, RopSysRoleEdit rop)
        {
            var sysRole = new SysRole();
            sysRole.Id = rop.RoleId;
            sysRole.Name = rop.Name;
            sysRole.Description = rop.Description;

            return SysFactory.AuthorizeRelay.UpdateRole(pOperater, sysRole);
        }

        public CustomJsonResult Delete(string pOperater, string[] roleIds)
        {
            return SysFactory.AuthorizeRelay.DeleteRole(pOperater, roleIds);
        }


        public CustomJsonResult GetDetails(string pOperater, string roleId)
        {
            var ret = new RetSysRoleGetDetails();

            var role = CurrentDb.SysRole.Where(m => m.Id == roleId).FirstOrDefault();

            ret.RoleId = role.Id;
            ret.Name = role.Name;
            ret.Description = role.Description;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }


        public CustomJsonResult AddUserToRole(string pOperater, string roleId, string[] userIds)
        {
            return SysFactory.AuthorizeRelay.AddUserToRole(pOperater, roleId, userIds);
        }

        public CustomJsonResult RemoveUserFromRole(string pOperater, string roleId, string[] userIds)
        {
            return SysFactory.AuthorizeRelay.RemoveUserFromRole(pOperater, roleId, userIds);
        }

        public CustomJsonResult SaveRoleMenu(string pOperater, string roleId, string[] menuIds)
        {
            return SysFactory.AuthorizeRelay.SaveRoleMenu(pOperater, roleId, menuIds);
        }
    }
}
