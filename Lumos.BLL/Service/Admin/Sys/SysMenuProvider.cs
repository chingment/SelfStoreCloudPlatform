﻿using Lumos.Entity;
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
        public CustomJsonResult GetPermissions(string operater, Enumeration.BelongSite belongSite)
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

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public CustomJsonResult GetDetails(string operater, string id)
        {
            var ret = new RetSysMenuGetDetails();

            var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == id).FirstOrDefault();
            if (sysMenu == null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空", ret);
            }

            ret.Id = sysMenu.Id;
            ret.Name = sysMenu.Name;
            ret.Url = sysMenu.Url;
            ret.Description = sysMenu.Description;

            var sysMenuPermission = CurrentDb.SysMenuPermission.Where(u => u.MenuId == id).ToList();
            var sysPermissionIds = (from p in sysMenuPermission select p.PermissionId).ToArray();

            ret.PermissionIds = sysPermissionIds;

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功", ret);
        }

        public static IEnumerable<SysMenu> GetFatherList(IList<SysMenu> list, string pId)
        {
            var query = list.Where(p => p.Id == pId).ToList();
            return query.ToList().Concat(query.ToList().SelectMany(t => GetFatherList(list, t.PId)));
        }

        public CustomJsonResult Add(string operater, RopSysMenuAdd rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var sysMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == rop.BelongSite).ToList();
                var sysMenusGetFather = GetFatherList(sysMenus, rop.PId).ToList();
                int dept = sysMenusGetFather.Count;
                var isExists = sysMenus.Where(m => m.PId == rop.PId && m.Name == rop.Name && m.Dept == dept).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, "该名称在同一级别已经存在");
                }

                var sysMenu = new SysMenu();
                sysMenu.Id = GuidUtil.New();
                sysMenu.Name = rop.Name;
                sysMenu.Url = rop.Url;
                sysMenu.Description = rop.Description;
                sysMenu.PId = rop.PId;
                sysMenu.IsCanDelete = true;
                sysMenu.Creator = operater;
                sysMenu.CreateTime = DateTime.Now;
                sysMenu.BelongSite = rop.BelongSite;
                sysMenu.Dept = dept;

                CurrentDb.SysMenu.Add(sysMenu);
                CurrentDb.SaveChanges();


                if (rop.PermissionIds != null)
                {
                    foreach (var permissionId in rop.PermissionIds)
                    {
                        CurrentDb.SysMenuPermission.Add(new SysMenuPermission { Id = GuidUtil.New(), MenuId = sysMenu.Id, PermissionId = permissionId, Creator = operater, CreateTime = DateTime.Now });
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();
                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
        }


        public CustomJsonResult Edit(string operater, RopSysMenuEdit rop)
        {
            CustomJsonResult result = new CustomJsonResult();
            using (TransactionScope ts = new TransactionScope())
            {
                var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == rop.Id).FirstOrDefault();
                if (sysMenu == null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "数据为空");
                }

                var sysMenus = CurrentDb.SysMenu.Where(m => m.BelongSite == sysMenu.BelongSite).ToList();
                var sysMenusGetFather = GetFatherList(sysMenus, sysMenu.PId).ToList();
                int dept = sysMenusGetFather.Count;
                var isExists = sysMenus.Where(m => m.Name == rop.Name && m.Id != rop.Id).FirstOrDefault();
                if (isExists != null)
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, string.Format("保存失败，该名称({0})已被同一级别使用", rop.Name));
                }

                sysMenu.Name = rop.Name;
                sysMenu.Url = rop.Url;
                sysMenu.Description = rop.Description;
                sysMenu.Mender = operater;
                sysMenu.MendTime = DateTime.Now;
                sysMenu.Dept = dept;

                var sysMenuPermissions = CurrentDb.SysMenuPermission.Where(r => r.MenuId == rop.Id).ToList();

                foreach (var sysMenuPermission in sysMenuPermissions)
                {
                    CurrentDb.SysMenuPermission.Remove(sysMenuPermission);
                }


                if (rop.PermissionIds != null)
                {
                    foreach (var permissionId in rop.PermissionIds)
                    {
                        CurrentDb.SysMenuPermission.Add(new SysMenuPermission { Id = GuidUtil.New(), MenuId = sysMenu.Id, PermissionId = permissionId, Creator = operater, CreateTime = DateTime.Now });
                    }
                }

                CurrentDb.SaveChanges();
                ts.Complete();

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
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

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
            }

            return result;
        }

        public CustomJsonResult EditSort(string operater, RopSysMenuEditSort rop)
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

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");

        }
    }
}
