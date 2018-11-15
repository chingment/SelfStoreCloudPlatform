using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class SysRoleProvider : BaseProvider
    {
        public CustomJsonResult Add(string pOperater, RopSysRoleAdd rop)
        {


            var isExistName = CurrentDb.SysRole.Where(m => m.Name == rop.Name).FirstOrDefault();

            if (isExistName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("添加失败，名称({0})已被使用", rop.Name));
            }
            var sysRole = new SysRole();
            sysRole.Id = GuidUtil.New();
            sysRole.Name = rop.Name;
            sysRole.Description = rop.Description;
            sysRole.PId = GuidUtil.Empty();
            sysRole.CreateTime = DateTime.Now;
            sysRole.Creator = pOperater;
            sysRole.IsCanDelete = true;
            CurrentDb.SysRole.Add(sysRole);
            CurrentDb.SaveChanges();
          

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult Edit(string pOperater, RopSysRoleEdit rop)
        {
            var isExistRoleName = CurrentDb.SysRole.Where(m => m.Name == rop.Name && m.Id != rop.RoleId).FirstOrDefault();
            if (isExistRoleName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("保存失败，名称({0})已被使用", rop.Name));
            }

            var sysRole = CurrentDb.SysRole.Where(m => m.Id == rop.RoleId).FirstOrDefault();

            sysRole.Name = rop.Name;
            sysRole.Description = rop.Description;
            sysRole.MendTime = DateTime.Now;
            sysRole.Mender = pOperater;

            CurrentDb.SaveChanges();
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
        }

        public CustomJsonResult Delete(string pOperater, string[] pRoleIds)
        {

            if (pRoleIds != null)
            {

                foreach (var id in pRoleIds)
                {
                    var roleUsers = CurrentDb.SysUserRole.Where(u => u.RoleId == id).Distinct();

                    var roleMenus = CurrentDb.SysRoleMenu.Where(u => u.RoleId == id).Distinct();


                    var role = CurrentDb.SysRole.Find(id);

                    if (!role.IsCanDelete)
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选角色（{0}）不允许删除", role.Name));
                    }


                    foreach (var user in roleUsers)
                    {
                        CurrentDb.SysUserRole.Remove(user);
                    }

                    foreach (var menu in roleMenus)
                    {
                        CurrentDb.SysRoleMenu.Remove(menu);
                    }


                    CurrentDb.SysRole.Remove(role);
                    CurrentDb.SaveChanges();

                }

            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");


        }


        public CustomJsonResult GetDetails(string pOperater, string roleId)
        {
            var ret = new RetSysRoleGetDetails();

            var role = CurrentDb.SysRole.Where(m => m.Id == roleId).FirstOrDefault();

            ret.RoleId = role.Id;
            ret.Name = role.Name;
            ret.Description = role.Description;

            var roleMenus = AdminServiceFactory.SysRole.GetRoleMenus(pOperater,roleId);
            var menuIds = (from p in roleMenus select p.Id).ToArray();

            ret.MenuIds = menuIds;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public List<SysMenu> GetRoleMenus(string pOperater, string pRoleId)
        {
            var model = from c in CurrentDb.SysMenu
                        where
                            (from o in CurrentDb.SysRoleMenu where o.RoleId == pRoleId select o.MenuId).Contains(c.Id)
                        select c;

            return model.ToList();
        }

        public CustomJsonResult AddUserToRole(string pOperater, string pRoleId, string[] pUserIds)
        {
            foreach (string userId in pUserIds)
            {
                CurrentDb.SysUserRole.Add(new SysUserRole { Id = GuidUtil.New(), UserId = userId, RoleId = pRoleId, Creator = pOperater, CreateTime = DateTime.Now, IsCanDelete = true });
                CurrentDb.SaveChanges();

            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult RemoveUserFromRole(string pOperater, string pRoleId, string[] pUserIds)
        {

            var role = CurrentDb.SysRole.Where(m => m.Id == pRoleId).FirstOrDefault();

            foreach (string userId in pUserIds)
            {
                var user = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();


                SysUserRole userRole = CurrentDb.SysUserRole.Find(pRoleId, userId);

                if (!userRole.IsCanDelete)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("用户（{0}）不能从角色（{1}）中删除", user.UserName, role.Name));
                }

                CurrentDb.SysUserRole.Remove(userRole);
                CurrentDb.SaveChanges();


           

            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "移除成功");
        }

        public CustomJsonResult SaveRoleMenu(string pOperater, string pRoleId, string[] pMenuIds)
        {

            List<SysRoleMenu> roleMenuList = CurrentDb.SysRoleMenu.Where(r => r.RoleId == pRoleId).ToList();

            foreach (var roleMenu in roleMenuList)
            {
                CurrentDb.SysRoleMenu.Remove(roleMenu);
            }


            if (pMenuIds != null)
            {
                foreach (var menuId in pMenuIds)
                {
                    CurrentDb.SysRoleMenu.Add(new SysRoleMenu { Id = GuidUtil.New(), RoleId = pRoleId, MenuId = menuId, Creator = pOperater, CreateTime = DateTime.Now });
                }
            }

            CurrentDb.SaveChanges();


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
        }
    }
}
