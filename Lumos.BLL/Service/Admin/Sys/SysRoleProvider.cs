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

        private List<SysRole> GetFathers(Enumeration.BelongSite belongSite, string id)
        {
            var sysRoles = CurrentDb.SysRole.Where(m => m.BelongSite == belongSite).ToList();

            var list = new List<SysRole>();
            var list2 = list.Concat(GetFatherList(sysRoles, id));
            return list2.ToList();
        }

        private IEnumerable<SysRole> GetFatherList(IList<SysRole> list, string pId)
        {
            var query = list.Where(p => p.Id == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetFatherList(list, t.PId)));
        }

        private List<SysRole> GetSons(string id)
        {
            var sysRoles = CurrentDb.SysRole.ToList();
            var sysRole = sysRoles.Where(p => p.Id == id).ToList();
            var list2 = sysRole.Concat(GetSonList(sysRoles, id));
            return list2.ToList();
        }

        private IEnumerable<SysRole> GetSonList(IList<SysRole> list, string pId)
        {
            var query = list.Where(p => p.PId == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetSonList(list, t.Id)));
        }

        public CustomJsonResult Add(string operater, RopSysRoleAdd rop)
        {

            var fathter = GetFathers(rop.BelongSite, rop.PId);
            int dept = fathter.Count;
            var isExists = CurrentDb.SysRole.Where(m => m.PId == rop.PId && m.Name == rop.Name && m.Dept == dept).FirstOrDefault();
            if (isExists != null)
            {
                return new CustomJsonResult(ResultType.Failure, "该名称在同一级别已经存在");
            }

            var sysRole = new SysRole();
            sysRole.Id = GuidUtil.New();
            sysRole.Name = rop.Name;
            sysRole.Description = rop.Description;
            sysRole.PId = rop.PId;
            sysRole.BelongSite = rop.BelongSite;
            sysRole.Dept = dept;
            sysRole.CreateTime = DateTime.Now;
            sysRole.Creator = operater;
            CurrentDb.SysRole.Add(sysRole);
            CurrentDb.SaveChanges();


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult Edit(string operater, RopSysRoleEdit rop)
        {
            var sysRole = CurrentDb.SysRole.Where(m => m.Id == rop.Id).FirstOrDefault();
            if (sysRole == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
            }

            var fathter = GetFathers(sysRole.BelongSite, sysRole.PId);
            int dept = fathter.Count;
            var isExists = CurrentDb.SysRole.Where(m => m.PId == sysRole.PId && m.Name == rop.Name && m.Dept == dept && m.Id != rop.Id).FirstOrDefault();
            if (isExists != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("保存失败，该名称({0})已被同一级别使用", rop.Name));
            }

            sysRole.Name = rop.Name;
            sysRole.Description = rop.Description;
            sysRole.MendTime = DateTime.Now;
            sysRole.Mender = operater;

            CurrentDb.SaveChanges();
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public CustomJsonResult Delete(string operater, string id)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                var sysRole = CurrentDb.SysRole.Where(m => m.Id == id).FirstOrDefault();

                if (sysRole == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }

                if (sysRole.Dept == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选角色（{0}）不允许删除", sysRole.Name));
                }


                var sons = GetSons(id).ToList();

                foreach (var son in sons)
                {
                    CurrentDb.SysRole.Remove(son);

                    var sysRoleMenus = CurrentDb.SysRoleMenu.Where(r => r.RoleId == son.Id).ToList();

                    foreach (var sysRoleMenu in sysRoleMenus)
                    {
                        CurrentDb.SysRoleMenu.Remove(sysRoleMenu);
                    }

                    var sysUserRoles = CurrentDb.SysUserRole.Where(r => r.RoleId == son.Id).ToList();

                    foreach (var sysUserRole in sysUserRoles)
                    {
                        CurrentDb.SysUserRole.Remove(sysUserRole);
                    }
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

        public CustomJsonResult EditSort(string operater, RopSysRoleEditSort rop)
        {
            if (rop != null)
            {
                if (rop.Dics != null)
                {
                    foreach (var item in rop.Dics)
                    {
                        string id = item.Id;
                        int priority = item.Priority;
                        var sysRole = CurrentDb.SysRole.Where(m => m.Id == id).FirstOrDefault();
                        if (sysRole != null)
                        {
                            sysRole.Priority = priority;
                            CurrentDb.SaveChanges();
                        }
                    }
                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }
    }
}
