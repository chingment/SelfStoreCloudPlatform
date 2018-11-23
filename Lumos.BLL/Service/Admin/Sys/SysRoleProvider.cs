using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Admin
{
    public class SysRoleProvider : BaseProvider
    {
        public CustomJsonResult Add(string operater, RopSysRoleAdd rop)
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
            sysRole.BelongSite = rop.BelongSite;
            sysRole.CreateTime = DateTime.Now;
            sysRole.Creator = operater;
            sysRole.IsCanDelete = true;
            CurrentDb.SysRole.Add(sysRole);
            CurrentDb.SaveChanges();


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult Edit(string operater, RopSysRoleEdit rop)
        {
            var isExistRoleName = CurrentDb.SysRole.Where(m => m.Name == rop.Name && m.Id != rop.Id).FirstOrDefault();
            if (isExistRoleName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("保存失败，名称({0})已被使用", rop.Name));
            }

            var sysRole = CurrentDb.SysRole.Where(m => m.Id == rop.Id).FirstOrDefault();

            sysRole.Name = rop.Name;
            sysRole.Description = rop.Description;
            sysRole.MendTime = DateTime.Now;
            sysRole.Mender = operater;

            CurrentDb.SaveChanges();
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult Delete(string operater, string[] ids)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (ids == null || ids.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }


                foreach (var id in ids)
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



                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;


        }


        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetSysRoleGetDetails();

            var role = CurrentDb.SysRole.Where(m => m.Id == id).FirstOrDefault();

            ret.Id = role.Id;
            ret.Name = role.Name;
            ret.Description = role.Description;

            var roleMenus = AdminServiceFactory.SysRole.GetMenus(operater, id);
            var menuIds = (from p in roleMenus select p.Id).ToArray();

            ret.MenuIds = menuIds;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public List<SysMenu> GetMenus(string operater, string pRoleId)
        {
            var model = from c in CurrentDb.SysMenu
                        where
                            (from o in CurrentDb.SysRoleMenu where o.RoleId == pRoleId select o.MenuId).Contains(c.Id)
                        select c;

            return model.ToList();
        }

        public CustomJsonResult AddUserToRole(string operater, string roleId, string[] userIds)
        {
            foreach (string userId in userIds)
            {
                CurrentDb.SysUserRole.Add(new SysUserRole { Id = GuidUtil.New(), UserId = userId, RoleId = roleId, Creator = operater, CreateTime = DateTime.Now, IsCanDelete = true });
                CurrentDb.SaveChanges();

            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult RemoveUserFromRole(string operater, string roleId, string[] userIds)
        {

            var role = CurrentDb.SysRole.Where(m => m.Id == roleId).FirstOrDefault();

            foreach (string userId in userIds)
            {
                var user = CurrentDb.SysUser.Where(m => m.Id == userId).FirstOrDefault();


                SysUserRole userRole = CurrentDb.SysUserRole.Find(roleId, userId);

                if (!userRole.IsCanDelete)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("用户（{0}）不能从角色（{1}）中删除", user.UserName, role.Name));
                }

                CurrentDb.SysUserRole.Remove(userRole);
                CurrentDb.SaveChanges();




            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult SaveMenus(string operater, string id, string[] menuIds)
        {

            List<SysRoleMenu> roleMenuList = CurrentDb.SysRoleMenu.Where(r => r.RoleId == id).ToList();

            foreach (var roleMenu in roleMenuList)
            {
                CurrentDb.SysRoleMenu.Remove(roleMenu);
            }


            if (menuIds != null)
            {
                foreach (var menuId in menuIds)
                {
                    CurrentDb.SysRoleMenu.Add(new SysRoleMenu { Id = GuidUtil.New(), RoleId = id, MenuId = menuId, Creator = operater, CreateTime = DateTime.Now });
                }
            }

            CurrentDb.SaveChanges();


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }
    }
}
