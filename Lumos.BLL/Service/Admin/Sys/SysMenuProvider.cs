using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class SysMenuProvider : BaseProvider
    {
        public CustomJsonResult GetPermissions(string pOperater)
        {
            var ret = new RetSysMenuGetPermissions();

            ret.Permissions = AdminServiceFactory.AuthorizeRelay.GetPermissionList(new PermissionCode());

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

        public CustomJsonResult Add(string pOperater, RopSysMenuAdd rop)
        {
            var sysMenu = new SysMenu();
            sysMenu.Id = GuidUtil.New();
            sysMenu.Name = rop.Name;
            sysMenu.Url = rop.Url;
            sysMenu.Description = rop.Description;
            sysMenu.PId = rop.PMenuId;
            sysMenu.IsCanDelete = true;
            sysMenu.Creator = pOperater;
            sysMenu.CreateTime = DateTime.Now;
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

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

        }


        public CustomJsonResult Edit(string pOperater, RopSysMenuEdit rop)
        {

            var sysMenu = CurrentDb.SysMenu.Where(m => m.Id == rop.MenuId).FirstOrDefault();

            sysMenu.Name = rop.Name;
            sysMenu.Url = rop.Url;
            sysMenu.Description = rop.Description;
            sysMenu.Mender = pOperater;
            sysMenu.MendTime = DateTime.Now;

            var sysMenuPermission = CurrentDb.SysMenuPermission.Where(r => r.MenuId == rop.MenuId).ToList();
            foreach (var m in sysMenuPermission)
            {
                CurrentDb.SysMenuPermission.Remove(m);
            }


            if (rop.PermissionIds != null)
            {
                foreach (var id in rop.PermissionIds)
                {
                    CurrentDb.SysMenuPermission.Add(new SysMenuPermission { Id = GuidUtil.New(), MenuId =sysMenu.Id, PermissionId = id, Creator = pOperater, CreateTime = DateTime.Now });
                }
            }

            CurrentDb.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");

  
        }


        public CustomJsonResult Delete(string pOperater, string[] pMenuIds)
        {
            if (pMenuIds != null)
            {
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
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
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
