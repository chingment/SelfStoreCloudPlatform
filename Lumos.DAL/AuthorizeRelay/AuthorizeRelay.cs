using System;
using System.Collections.Generic;
using System.Linq;
using Lumos.Entity;
using System.Reflection;
using System.Transactions;

namespace Lumos.DAL.AuthorizeRelay
{
    public class LoginResult
    {

        private Enumeration.LoginResult _ResultType;
        private Enumeration.LoginResultTip _ResultTip;
        private SysUser _User;

        public Enumeration.LoginResult ResultType
        {
            get
            {
                return _ResultType;
            }
        }


        public Enumeration.LoginResultTip ResultTip
        {
            get
            {
                return _ResultTip;
            }
        }


        public SysUser User
        {
            get
            {
                return _User;
            }
        }

        public LoginResult()
        {

        }

        public LoginResult(Enumeration.LoginResult pType, Enumeration.LoginResultTip pTip)
        {
            this._ResultType = pType;
            this._ResultTip = pTip;
        }

        public LoginResult(Enumeration.LoginResult pType, Enumeration.LoginResultTip pTip, SysUser pUser)
        {
            this._ResultType = pType;
            this._ResultTip = pTip;
            this._User = pUser;
        }

    }

    public class AuthorizeRelay
    {
        private AuthorizeRelayDbContext _db;

        public AuthorizeRelay()
        {
            _db = new AuthorizeRelayDbContext();
        }

        private void AddOperateHistory(string pOperater, Enumeration.OperateType pOperateType, string pReferenceId, string pContent)
        {
            SysOperateHistory operateHistory = new SysOperateHistory();
            operateHistory.Id = GuidUtil.New();
            operateHistory.UserId = pOperater;
            operateHistory.ReferenceId = pReferenceId;
            operateHistory.Ip = "";
            operateHistory.Type = pOperateType;
            operateHistory.Content = pContent;
            operateHistory.CreateTime = DateTime.Now;
            operateHistory.Creator = pOperater;
            _db.SysOperateHistory.Add(operateHistory);
            _db.SaveChanges();
        }

        private object CloneObject(object pObject)
        {
            Type t = pObject.GetType();
            PropertyInfo[] properties = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, pObject, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(pObject, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        }

        public LoginResult SignIn(string pUserName, string pPassword, DateTime pLoginTime, string pLoginIp)
        {
            LoginResult result = new LoginResult();

            pUserName = pUserName.Trim();
            var user = _db.SysUser.Where(m => m.UserName == pUserName).FirstOrDefault();

            if (user == null)
            {
                result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserNotExist);
            }

            else
            {
                var lastUserInfo = CloneObject(user) as SysUser;

                bool isFlag = PassWordHelper.VerifyHashedPassword(user.PasswordHash, pPassword);

                if (!isFlag)
                {
                    result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserPasswordIncorrect, lastUserInfo);
                }
                else
                {

                    if (user.Status == Enumeration.UserStatus.Disable)
                    {
                        result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserDisabled, lastUserInfo);
                    }
                    else
                    {
                        if (user.IsDelete)
                        {
                            result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserDeleted, lastUserInfo);
                        }
                        else
                        {
                            user.LastLoginTime = pLoginTime;
                            user.LastLoginIp = pLoginIp;
                            _db.SaveChanges();

                            result = new LoginResult(Enumeration.LoginResult.Success, Enumeration.LoginResultTip.VerifyPass, lastUserInfo);

                        }
                    }
                }
            }


            return result;
        }

        public bool UserNameIsExists(string pUserName)
        {
            var sysUser = _db.SysUser.Where(m => m.UserName == pUserName).FirstOrDefault();
            if (sysUser == null)
                return false;

            return true;
        }

        public List<SysMenu> GetUserMenus(string pUserId)
        {
            List<SysMenu> listMenu = new List<SysMenu>();
            var model =
                from menu in _db.SysMenu
                where
                (from rolemenu in _db.SysRoleMenu
                 where
                 (from userrole in _db.SysUserRole where rolemenu.RoleId == userrole.RoleId && userrole.UserId == pUserId select userrole.RoleId)
                 .Contains(rolemenu.RoleId)
                 select rolemenu.MenuId).Contains(menu.Id)
                select menu;


            if (model != null)
            {
                listMenu = model.OrderByDescending(m => m.Priority).ToList();
            }
            return listMenu;
        }

        public List<string> GetUserPermissions(string pUserId)
        {
            List<string> list = new List<string>();

            var model = (from sysMenuPermission in _db.SysMenuPermission
                         where
                             (from sysRoleMenu in _db.SysRoleMenu
                              where
                              (from userrole in _db.SysUserRole where sysRoleMenu.RoleId == userrole.RoleId && userrole.UserId == pUserId select userrole.RoleId)
                              .Contains(sysRoleMenu.RoleId)
                              select sysRoleMenu.MenuId).Contains(sysMenuPermission.MenuId)
                         select sysMenuPermission.PermissionId).Distinct();




            if (model != null)
            {
                list = model.ToList();
            }
            return list;
        }

        public CustomJsonResult CreateUser<T>(string pOperater, T pUser, params string[] pRoleIds) where T : SysUser
        {
            CustomJsonResult result = new CustomJsonResult();

            var isExistUserName = _db.SysUser.Where(m => m.UserName == pUser.UserName).FirstOrDefault();

            if (isExistUserName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该用户名已经存在");
            }

            using (TransactionScope ts = new TransactionScope())
            {
                pUser.Creator = pOperater;
                pUser.CreateTime = DateTime.Now;
                pUser.RegisterTime = DateTime.Now;
                pUser.Status = Enumeration.UserStatus.Normal;
                pUser.SecurityStamp = Guid.NewGuid().ToString().Replace("-", "");


                if (typeof(T) == typeof(SysStaffUser))
                {
                    _db.SysStaffUser.Add(pUser as SysStaffUser);
                }
                else if (typeof(T) == typeof(SysMerchantUser))
                {
                    _db.SysMerchantUser.Add(pUser as SysMerchantUser);
                }

                _db.SaveChanges();

                List<SysUserRole> userRoleList = _db.SysUserRole.Where(m => m.UserId == pUser.Id).ToList();
                foreach (var userRole in userRoleList)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                if (pRoleIds != null)
                {
                    if (pRoleIds.Length > 0)
                    {
                        foreach (string roleId in pRoleIds)
                        {

                            _db.SysUserRole.Add(new SysUserRole { Id = GuidUtil.New(), UserId = pUser.Id, RoleId = roleId, Creator = pOperater, CreateTime = DateTime.Now });

                        }
                    }
                }

                AddOperateHistory(pOperater, Enumeration.OperateType.New, pUser.Id, string.Format("新建用户{0}(ID:{1})", pUser.UserName, pUser.Id));

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

                _db.SaveChanges();
                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult UpdateUser<T>(string pOperater, T pUser, string[] pRoleIds) where T : SysUser
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {

                if (typeof(T) == typeof(SysStaffUser))
                {
                    var sysStaffUser = _db.SysStaffUser.Where(m => m.Id == pUser.Id).FirstOrDefault();

                    if (sysStaffUser != null)
                    {
                        if (!string.IsNullOrEmpty(pUser.PasswordHash))
                        {
                            sysStaffUser.PasswordHash = pUser.PasswordHash;
                        }

                        sysStaffUser.FullName = pUser.FullName;
                        sysStaffUser.Email = pUser.Email;
                        sysStaffUser.PhoneNumber = pUser.PhoneNumber;
                        sysStaffUser.MendTime = DateTime.Now;
                        sysStaffUser.Mender = pOperater;
                        _db.SaveChanges();
                    }

                }

                List<SysUserRole> userRoleList = _db.SysUserRole.Where(m => m.UserId == pUser.Id).ToList();

                foreach (var userRole in userRoleList)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                if (pRoleIds != null)
                {
                    if (pRoleIds.Length > 0)
                    {
                        foreach (string roleId in pRoleIds)
                        {
                            if (roleId != "0")
                            {
                                _db.SysUserRole.Add(new SysUserRole { Id = GuidUtil.New(), UserId = pUser.Id, RoleId = roleId, Creator = pOperater, CreateTime = DateTime.Now });
                            }
                        }
                    }
                }


                AddOperateHistory(pOperater, Enumeration.OperateType.Update, pUser.Id, string.Format("修改用户{0}(ID:{1})", pUser.UserName, pUser.Id));

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");

                _db.SaveChanges();
                ts.Complete();
            }
            return result;
        }

        public CustomJsonResult DeleteUser(string pOperater, string[] pUserIds)
        {
            if (pUserIds == null)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到用户");

            if (pUserIds.Length <= 0)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到用户");


            foreach (string userId in pUserIds)
            {
                SysUser user = _db.SysUser.Find(userId);
                user.IsDelete = true;
                user.Mender = pOperater;
                user.MendTime = DateTime.Now;

                var userRoles = _db.SysUserRole.Where(m => m.UserId == userId).ToList();
                foreach (var userRole in userRoles)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                _db.SaveChanges();

                AddOperateHistory(pOperater, Enumeration.OperateType.Delete, user.Id, string.Format("删除用户{0}(ID:{1})", user.UserName, user.Id));
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public CustomJsonResult ChangePassword(string pOperater, string pUserId, string pOldpassword, string pNewpassword)
        {

            var sysUser = _db.SysUser.Where(m => m.Id == pUserId).FirstOrDefault();
            if (sysUser != null)
            {

                if (!PassWordHelper.VerifyHashedPassword(sysUser.PasswordHash, pOldpassword))
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "旧密码不正确");
                }

                sysUser.PasswordHash = PassWordHelper.HashPassword(pNewpassword);
                sysUser.Mender = pOperater;
                sysUser.MendTime = DateTime.Now;

                _db.SaveChanges();
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");
        }

        public bool ResetPassword(string pOperater, string pUserId, string pPassword)
        {
            //SysUser user = GetUser(userId);
            //user.PasswordHash = null;
            //user.Mender = operater;
            //user.LastUpdateTime = DateTime.Now;
            //_db.SaveChanges();

            //IdentityResult result = _userManager.AddPassword(userId, password);
            //return result.Succeeded;

            return true;

        }

        public CustomJsonResult CreateRole(string pOperater, SysRole pRole)
        {
            var isExistName = _db.SysRole.Where(m => m.Name == pRole.Name).FirstOrDefault();

            if (isExistName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该用户名已经存在");
            }

            pRole.PId = "0";
            pRole.CreateTime = DateTime.Now;
            pRole.Creator = pOperater;
            _db.SysRole.Add(pRole);
            _db.SaveChanges();
            AddOperateHistory(pOperater, Enumeration.OperateType.New, pRole.Id, string.Format("新建角色{0}(ID:{1})", pRole.Name, pRole.Id));

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult UpdateRole(string pOperater, SysRole pSysRole)
        {
            var isExistRoleName = _db.SysRole.Where(m => m.Name == pSysRole.Name && m.Id != pSysRole.Id).FirstOrDefault();
            if (isExistRoleName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "角色名字已经存在");
            }

            var _sysRole = _db.SysRole.Where(m => m.Id == pSysRole.Id).FirstOrDefault();

            _sysRole.Name = pSysRole.Name;
            _sysRole.Description = pSysRole.Description;
            _sysRole.MendTime = DateTime.Now;
            _sysRole.Mender = pOperater;

            AddOperateHistory(pOperater, Enumeration.OperateType.Update, _sysRole.Id, string.Format("修改角色{0}(ID:{1})", _sysRole.Name, _sysRole.Id));


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");
        }

        public CustomJsonResult DeleteRole(string pOperater, string[] pRoldeIds)
        {

            if (pRoldeIds != null)
            {
                foreach (var id in pRoldeIds)
                {
                    var roleUsers = _db.SysUserRole.Where(u => u.RoleId == id).Distinct();

                    var roleMenus = _db.SysRoleMenu.Where(u => u.RoleId == id).Distinct();


                    var role = _db.SysRole.Find(id);

                    foreach (var user in roleUsers)
                    {
                        _db.SysUserRole.Remove(user);
                    }

                    foreach (var menu in roleMenus)
                    {
                        _db.SysRoleMenu.Remove(menu);
                    }


                    _db.SysRole.Remove(role);
                    _db.SaveChanges();

                    AddOperateHistory(pOperater, Enumeration.OperateType.Delete, role.Id, string.Format("删除角色{0}(ID:{1})", role.Name, role.Id));
                }

            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public CustomJsonResult AddUserToRole(string pOperater, string pRoleId, string[] pUserIds)
        {
            foreach (string userId in pUserIds)
            {
                _db.SysUserRole.Add(new SysUserRole { Id = GuidUtil.New(), UserId = userId, RoleId = pRoleId, Creator = pOperater, CreateTime = DateTime.Now });
                _db.SaveChanges();

                AddOperateHistory(pOperater, Enumeration.OperateType.Update, pRoleId, string.Format("添加用户(ID：{0})到角色(ID:{1})", userId, pRoleId));
            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult RemoveUserFromRole(string pOperater, string pRoleId, string[] pUserIds)
        {
            foreach (string userId in pUserIds)
            {
                SysUserRole userRole = _db.SysUserRole.Find(pRoleId, userId);
                _db.SysUserRole.Remove(userRole);
                _db.SaveChanges();


                AddOperateHistory(pOperater, Enumeration.OperateType.Update, pRoleId, string.Format("移除用户(ID：{0})所在的角色(ID:{1})", userId, pRoleId));

            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "移除成功");
        }

        public List<SysMenu> GetRoleMenus(string pRoleId)
        {
            var model = from c in _db.SysMenu
                        where
                            (from o in _db.SysRoleMenu where o.RoleId == pRoleId select o.MenuId).Contains(c.Id)
                        select c;

            return model.ToList();
        }

        public CustomJsonResult SaveRoleMenu(string pOperater, string pRoleId, string[] pMenuIds)
        {

            List<SysRoleMenu> roleMenuList = _db.SysRoleMenu.Where(r => r.RoleId == pRoleId).ToList();

            foreach (var roleMenu in roleMenuList)
            {
                _db.SysRoleMenu.Remove(roleMenu);
            }


            if (pMenuIds != null)
            {
                foreach (var menuId in pMenuIds)
                {
                    _db.SysRoleMenu.Add(new SysRoleMenu { Id = GuidUtil.New(), RoleId = pRoleId, MenuId = menuId, Creator = pOperater, CreateTime = DateTime.Now });
                }
            }

            _db.SaveChanges();

            AddOperateHistory(pOperater, Enumeration.OperateType.Update, pRoleId, string.Format("保存角色(ID:{0})菜单", pRoleId));

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
        }

        public CustomJsonResult CreateMenu(string pOperater, SysMenu pSysMenu, string[] pPerssionIds)
        {
            pSysMenu.Id = GuidUtil.New();
            pSysMenu.Creator = pOperater;
            pSysMenu.CreateTime = DateTime.Now;
            _db.SysMenu.Add(pSysMenu);
            _db.SaveChanges();

            if (pPerssionIds != null)
            {
                foreach (var id in pPerssionIds)
                {
                    _db.SysMenuPermission.Add(new SysMenuPermission { Id = GuidUtil.New(), MenuId = pSysMenu.Id, PermissionId = id, Creator = pOperater, CreateTime = DateTime.Now });
                }
            }
            _db.SaveChanges();

            AddOperateHistory(pOperater, Enumeration.OperateType.New, pSysMenu.Id, string.Format("新建菜单(ID:{0})", pSysMenu.Id));

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult UpdateMenu(string pOperater, SysMenu pSysMenu, string[] pPerssionIds)
        {

            var _sysMenu = _db.SysMenu.Where(m => m.Id == pSysMenu.Id).FirstOrDefault();

            _sysMenu.Name = pSysMenu.Name;
            _sysMenu.Url = pSysMenu.Url;
            _sysMenu.Description = pSysMenu.Description;
            _sysMenu.Mender = pOperater;
            _sysMenu.MendTime = DateTime.Now;

            var sysMenuPermission = _db.SysMenuPermission.Where(r => r.MenuId == pSysMenu.Id).ToList();
            foreach (var m in sysMenuPermission)
            {
                _db.SysMenuPermission.Remove(m);
            }


            if (pPerssionIds != null)
            {
                foreach (var id in pPerssionIds)
                {
                    _db.SysMenuPermission.Add(new SysMenuPermission { Id = GuidUtil.New(), MenuId = pSysMenu.Id, PermissionId = id, Creator = pOperater, CreateTime = DateTime.Now });
                }
            }

            AddOperateHistory(pOperater, Enumeration.OperateType.Update, pSysMenu.Id, string.Format("修改菜单(ID:{0})", pSysMenu.Id));

            _db.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");

        }

        public CustomJsonResult DeleteMenu(string pOperater, string[] pMenuIds)
        {
            if (pMenuIds != null)
            {
                foreach (var id in pMenuIds)
                {
                    var sysMenu = _db.SysMenu.Where(m => m.Id == id).FirstOrDefault();

                    _db.SysMenu.Remove(sysMenu);

                    var sysRoleMenus = _db.SysRoleMenu.Where(r => r.MenuId == id).ToList();
                    foreach (var sysRoleMenu in sysRoleMenus)
                    {
                        _db.SysRoleMenu.Remove(sysRoleMenu);
                    }

                    _db.SaveChanges();

                    AddOperateHistory(pOperater, Enumeration.OperateType.Delete, id, string.Format("删除菜单(ID:{0})", id));

                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public List<SysPermission> GetPermissionList(PermissionCode pPermissionCode)
        {
            Type t = pPermissionCode.GetType();
            List<SysPermission> list = new List<SysPermission>();
            //SysPermission p = new SysPermission();
            //p.Id = "0";
            //p.Name = "权限集合";
            //p.PId = "";
            //list.Add(p);
            list = GetBasePermissionList(t, list);
            return list;
        }

        private List<SysPermission> GetBasePermissionList(Type pType, List<SysPermission> pSysPermissionList)
        {
            if (pType.Name != "Object")
            {
                System.Reflection.FieldInfo[] properties = pType.GetFields();
                foreach (System.Reflection.FieldInfo property in properties)
                {
                    string pId = "0";
                    object[] typeAttributes = property.GetCustomAttributes(false);
                    foreach (PermissionCodeAttribute attribute in typeAttributes)
                    {
                        pId = attribute.PId;
                    }
                    object id = property.GetValue(null);
                    string name = property.Name;
                    SysPermission model = new SysPermission();
                    model.Id = id.ToString();
                    model.Name = name;
                    pSysPermissionList.Add(model);
                }
                pSysPermissionList = GetBasePermissionList(pType.BaseType, pSysPermissionList);
            }
            return pSysPermissionList;
        }

    }
}
