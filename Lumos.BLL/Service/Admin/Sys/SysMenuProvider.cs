using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Lumos.BLL.Service.Admin
{
    public class SysMenuProvider : BaseProvider
    {
        public CustomJsonResult GetPermissions(string pOperater, Enumeration.BelongSite belongSite)
        {
            var ret = new RetSysMenuGetPermissions();

            if (belongSite == Enumeration.BelongSite.Admin)
            {
                ret.Permissions = AdminServiceFactory.AuthorizeRelay.GetPermissionList(typeof(AdminPermissionCode));
            }
            else if (belongSite == Enumeration.BelongSite.Merchant)
            {
                ret.Permissions = AdminServiceFactory.AuthorizeRelay.GetPermissionList(typeof(MchPermissionCode));
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public CustomJsonResult GetDetails(string pOperater, string pMenuId)
        {
            var ret = new RetSysMenuGetDetails();

            var menu = CurrentDb.SysMenu.Where(m => m.Id == pMenuId).FirstOrDefault();

            ret.MenuId = menu.Id;
            ret.Name = menu.Name;
            ret.Url = menu.Url;
            ret.Description = menu.Description;

            var sysMenuPermission = CurrentDb.SysMenuPermission.Where(u => u.MenuId == pMenuId).ToList();
            var permissionIdIds = (from p in sysMenuPermission select p.PermissionId).ToArray();

            ret.PermissionIds = permissionIdIds;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "获取成功", ret);
        }

        public static IEnumerable<SysMenu> GetFatherList(IList<SysMenu> list, string pId)
        {
            var query = list.Where(p => p.Id == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetFatherList(list, t.PId)));
        }

        public CustomJsonResult Add(string pOperater, RopSysMenuAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var allMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == rop.BelongSite).ToList();
                var fatheMenus = GetFatherList(allMenus, rop.PMenuId).ToList();
                int dept = fatheMenus.Count;
                var isExists = allMenus.Where(m => m.Name == rop.Name && m.Dept == dept).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "该名称在同一级别已经存在");
                }

                var sysMenu = new SysMenu();
                sysMenu.Id = GuidUtil.New();
                sysMenu.Name = rop.Name;
                sysMenu.Url = rop.Url;
                sysMenu.Description = rop.Description;
                sysMenu.PId = rop.PMenuId;
                sysMenu.IsCanDelete = true;
                sysMenu.Creator = pOperater;
                sysMenu.CreateTime = DateTime.Now;
                sysMenu.BelongSite = rop.BelongSite;
                sysMenu.Dept = dept;

                CurrentDb.SysMenu.Add(sysMenu);
                CurrentDb.SaveChanges();


                if (rop.PermissionIds != null)
                {
                    foreach (var id in rop.PermissionIds)
                    {
                        CurrentDb.SysMenuPermission.Add(new SysMenuPermission { Id = GuidUtil.New(), MenuId = sysMenu.Id, PermissionId = id, Creator = pOperater, CreateTime = DateTime.Now });
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "新建成功");
            }

            return result;
        }


        public CustomJsonResult Edit(string pOperater, RopSysMenuEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == rop.MenuId).FirstOrDefault();
                if (sysMenu == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("找不到选择的数据（{0}）", rop.Name));
                }

                var allMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == sysMenu.BelongSite).ToList();
                var fatheMenus = GetFatherList(allMenus, sysMenu.PId).ToList();
                int dept = fatheMenus.Count;
                var isExists = allMenus.Where(m => m.Name == rop.Name && m.Id != rop.MenuId).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("保存失败，该名称({0})已被同一级别使用", rop.Name));
                }

                sysMenu.Name = rop.Name;
                sysMenu.Url = rop.Url;
                sysMenu.Description = rop.Description;
                sysMenu.Mender = pOperater;
                sysMenu.MendTime = DateTime.Now;
                sysMenu.Dept = dept;

                var sysMenuPermission = CurrentDb.SysMenuPermission.Where(r => r.MenuId == rop.MenuId).ToList();
                foreach (var m in sysMenuPermission)
                {
                    CurrentDb.SysMenuPermission.Remove(m);
                }


                if (rop.PermissionIds != null)
                {
                    foreach (var id in rop.PermissionIds)
                    {
                        CurrentDb.SysMenuPermission.Add(new SysMenuPermission { Id = GuidUtil.New(), MenuId = sysMenu.Id, PermissionId = id, Creator = pOperater, CreateTime = DateTime.Now });
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
            }

            return result;
        }


        public CustomJsonResult Delete(string pOperater, string[] pMenuIds)
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (pMenuIds == null || pMenuIds.Length == 0)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "请选择要删除的数据");
                }

                foreach (var id in pMenuIds)
                {
                    var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == id).FirstOrDefault();

                    if (!sysMenu.IsCanDelete)
                    {
                        return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("所选菜单（{0}）不允许删除", sysMenu.Name));
                    }

                    CurrentDb.SysMenu.Remove(sysMenu);

                    var sysRoleMenus = CurrentDb.SysRoleMenu.Where(r => r.MenuId == id).ToList();
                    foreach (var sysRoleMenu in sysRoleMenus)
                    {
                        CurrentDb.SysRoleMenu.Remove(sysRoleMenu);
                    }

                    CurrentDb.SaveChanges();

                }


                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
            }

            return result;
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
