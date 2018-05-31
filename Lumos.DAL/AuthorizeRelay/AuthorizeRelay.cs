using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Runtime.Remoting.Messaging;
using Lumos.Entity;
using System.Reflection;
using System.Security.Cryptography;
using Lumos.Mvc;
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

        public LoginResult(Enumeration.LoginResult type, Enumeration.LoginResultTip tip)
        {
            this._ResultType = type;
            this._ResultTip = tip;
        }

        public LoginResult(Enumeration.LoginResult type, Enumeration.LoginResultTip tip, SysUser user)
        {
            this._ResultType = type;
            this._ResultTip = tip;
            this._User = user;
        }

    }

    public class AuthorizeRelay
    {
        private AuthorizeRelayDbContext _db;

        public AuthorizeRelay()
        {
            _db = new AuthorizeRelayDbContext();
        }

        private void AddOperateHistory(int operater, Enumeration.OperateType operateType, int referenceId, string content)
        {
            SysOperateHistory operateHistory = new SysOperateHistory();
            operateHistory.UserId = operater;
            operateHistory.ReferenceId = referenceId;
            operateHistory.Ip = "";
            operateHistory.Type = operateType;
            operateHistory.Content = content;
            operateHistory.CreateTime = DateTime.Now;
            operateHistory.Creator = operater;
            _db.SysOperateHistory.Add(operateHistory);
            _db.SaveChanges();
        }

        private object CloneObject(object o)
        {
            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties();
            Object p = t.InvokeMember("", System.Reflection.BindingFlags.CreateInstance, null, o, null);
            foreach (PropertyInfo pi in properties)
            {
                if (pi.CanWrite)
                {
                    object value = pi.GetValue(o, null);
                    pi.SetValue(p, value, null);
                }
            }
            return p;
        }

        public LoginResult SignIn(string userName, string password, DateTime loginTime, string loginIp)
        {
            LoginResult result = new LoginResult();

            userName = userName.Trim();
            var user = _db.SysUser.Where(m => m.UserName == userName).FirstOrDefault();

            if (user == null)
            {
                result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserNotExist);
            }

            else
            {
                var lastUserInfo = CloneObject(user) as SysUser;

                bool isFlag = PassWordHelper.VerifyHashedPassword(user.PasswordHash, password);

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
                            user.LastLoginTime = loginTime;
                            user.LastLoginIp = loginIp;
                            _db.SaveChanges();

                            result = new LoginResult(Enumeration.LoginResult.Success, Enumeration.LoginResultTip.VerifyPass, lastUserInfo);

                        }
                    }
                }
            }


            return result;
        }

        public bool UserNameIsExists(string username)
        {
            var sysUser = _db.SysUser.Where(m => m.UserName == username).FirstOrDefault();
            if (sysUser == null)
                return false;

            return true;
        }

        public List<SysMenu> GetUserMenus(int userId)
        {
            List<SysMenu> listMenu = new List<SysMenu>();
            var model =
                from menu in _db.SysMenu
                where
                (from rolemenu in _db.SysRoleMenu
                 where
                 (from userrole in _db.SysUserRole where rolemenu.RoleId == userrole.RoleId && userrole.UserId == userId select userrole.RoleId)
                 .Contains(rolemenu.RoleId)
                 select rolemenu.MenuId).Contains(menu.Id)
                select menu;


            if (model != null)
            {
                listMenu = model.OrderByDescending(m => m.Priority).ToList();
            }
            return listMenu;
        }

        public List<string> GetUserPermissions(int userId)
        {
            List<string> list = new List<string>();

            var model = (from sysMenuPermission in _db.SysMenuPermission
                         where
                             (from sysRoleMenu in _db.SysRoleMenu
                              where
                              (from userrole in _db.SysUserRole where sysRoleMenu.RoleId == userrole.RoleId && userrole.UserId == userId select userrole.RoleId)
                              .Contains(sysRoleMenu.RoleId)
                              select sysRoleMenu.MenuId).Contains(sysMenuPermission.MenuId)
                         select sysMenuPermission.PermissionId).Distinct();




            if (model != null)
            {
                list = model.ToList();
            }
            return list;
        }

        public CustomJsonResult CreateUser<T>(int operater, T user, params int[] roleIds) where T : SysUser
        {
            CustomJsonResult result = new CustomJsonResult();

            var isExistUserName = _db.SysUser.Where(m => m.UserName == user.UserName).FirstOrDefault();

            if (isExistUserName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该用户名已经存在");
            }

            using (TransactionScope ts = new TransactionScope())
            {
                user.Creator = operater;
                user.CreateTime = DateTime.Now;
                user.RegisterTime = DateTime.Now;
                user.Status = Enumeration.UserStatus.Normal;
                user.SecurityStamp = Guid.NewGuid().ToString().Replace("-", "");



                if (typeof(T) == typeof(SysSalesmanUser))
                {
                    _db.SysSalesmanUser.Add(user as SysSalesmanUser);
                }
                else if (typeof(T) == typeof(SysAgentUser))
                {
                    _db.SysAgentUser.Add(user as SysAgentUser);
                }
                else if (typeof(T) == typeof(SysStaffUser))
                {
                    _db.SysStaffUser.Add(user as SysStaffUser);
                }

                _db.SaveChanges();

                List<SysUserRole> userRoleList = _db.SysUserRole.Where(m => m.UserId == user.Id).ToList();
                foreach (var userRole in userRoleList)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                if (roleIds != null)
                {
                    if (roleIds.Length > 0)
                    {
                        foreach (int roleId in roleIds)
                        {
                            if (roleId != 0)
                            {
                                _db.SysUserRole.Add(new SysUserRole { UserId = user.Id, RoleId = roleId });
                            }
                        }
                    }
                }

                AddOperateHistory(operater, Enumeration.OperateType.New, user.Id, string.Format("新建用户{0}(ID:{1})", user.UserName, user.Id));

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");

                _db.SaveChanges();
                ts.Complete();
            }

            return result;
        }

        public CustomJsonResult UpdateUser<T>(int operater, T user, int[] roleIds) where T : SysUser
        {
            CustomJsonResult result = new CustomJsonResult();

            using (TransactionScope ts = new TransactionScope())
            {
                if (typeof(T) == typeof(SysSalesmanUser))
                {

                    var sysSalesmanUser = _db.SysSalesmanUser.Where(m => m.Id == user.Id).FirstOrDefault();

                    if (sysSalesmanUser != null)
                    {
                        if (!string.IsNullOrEmpty(user.Password))
                        {
                            sysSalesmanUser.PasswordHash = PassWordHelper.HashPassword(user.Password);
                        }
                        sysSalesmanUser.FullName = user.FullName;
                        sysSalesmanUser.Email = user.Email;
                        sysSalesmanUser.PhoneNumber = user.PhoneNumber;
                        sysSalesmanUser.LastUpdateTime = DateTime.Now;
                        sysSalesmanUser.Mender = operater;
                        _db.SaveChanges();
                    }

                }
                else if (typeof(T) == typeof(SysAgentUser))
                {
                    var sysAgentUser = _db.SysAgentUser.Where(m => m.Id == user.Id).FirstOrDefault();

                    if (sysAgentUser != null)
                    {
                        if (!string.IsNullOrEmpty(user.Password))
                        {
                            sysAgentUser.PasswordHash = PassWordHelper.HashPassword(user.Password);
                        }
                        sysAgentUser.FullName = user.FullName;
                        sysAgentUser.Email = user.Email;
                        sysAgentUser.PhoneNumber = user.PhoneNumber;
                        sysAgentUser.LastUpdateTime = DateTime.Now;
                        sysAgentUser.Mender = operater;
                        sysAgentUser.WechatNumber = user.WechatNumber;
                        _db.SaveChanges();
                    }

                }
                else if (typeof(T) == typeof(SysStaffUser))
                {
                    var sysStaffUser = _db.SysStaffUser.Where(m => m.Id == user.Id).FirstOrDefault();

                    if (sysStaffUser != null)
                    {
                        if (!string.IsNullOrEmpty(user.Password))
                        {
                            sysStaffUser.PasswordHash = PassWordHelper.HashPassword(user.Password);
                        }
                        sysStaffUser.FullName = user.FullName;
                        sysStaffUser.Email = user.Email;
                        sysStaffUser.PhoneNumber = user.PhoneNumber;
                        sysStaffUser.LastUpdateTime = DateTime.Now;
                        sysStaffUser.Mender = operater;
                        _db.SaveChanges();
                    }

                }

                List<SysUserRole> userRoleList = _db.SysUserRole.Where(m => m.UserId == user.Id).ToList();

                foreach (var userRole in userRoleList)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                if (roleIds != null)
                {
                    if (roleIds.Length > 0)
                    {
                        foreach (int roleId in roleIds)
                        {
                            if (roleId != 0)
                            {
                                _db.SysUserRole.Add(new SysUserRole { UserId = user.Id, RoleId = roleId });
                            }
                        }
                    }
                }


                AddOperateHistory(operater, Enumeration.OperateType.Update, user.Id, string.Format("修改用户{0}(ID:{1})", user.UserName, user.Id));

                result = new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");

                _db.SaveChanges();
                ts.Complete();
            }
            return result;
        }

        public CustomJsonResult DeleteUser(int operater, int[] userIds)
        {
            if (userIds == null)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到用户");

            if (userIds.Length <= 0)
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "找不到用户");


            foreach (int userId in userIds)
            {
                SysUser user = _db.SysUser.Find(userId);
                user.IsDelete = true;
                user.Mender = operater;
                user.LastUpdateTime = DateTime.Now;

                var userRoles = _db.SysUserRole.Where(m => m.UserId == userId).ToList();
                foreach (var userRole in userRoles)
                {
                    _db.SysUserRole.Remove(userRole);
                }

                _db.SaveChanges();

                AddOperateHistory(operater, Enumeration.OperateType.Delete, user.Id, string.Format("删除用户{0}(ID:{1})", user.UserName, user.Id));
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public CustomJsonResult ChangePassword(int operater, int userId, string oldpassword, string newpassword)
        {

            var sysUser = _db.SysUser.Where(m => m.Id == userId).FirstOrDefault();
            if (sysUser != null)
            {

                if(!PassWordHelper.VerifyHashedPassword(sysUser.PasswordHash,oldpassword))
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "旧密码不正确");
                }

                sysUser.PasswordHash = PassWordHelper.HashPassword(newpassword);
                sysUser.Mender = operater;
                sysUser.LastUpdateTime = DateTime.Now;

                _db.SaveChanges();
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");
        }

        public bool ResetPassword(int operater, int userId, string password)
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

        public CustomJsonResult CreateRole(int operater, SysRole role)
        {
            var isExistName = _db.SysRole.Where(m => m.Name == role.Name).FirstOrDefault();

            if (isExistName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "该用户名已经存在");
            }

            role.PId = 0;
            role.CreateTime = DateTime.Now;
            role.Creator = operater;
            _db.SysRole.Add(role);
            _db.SaveChanges();
            AddOperateHistory(operater, Enumeration.OperateType.New, role.Id, string.Format("新建角色{0}(ID:{1})", role.Name, role.Id));

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult UpdateRole(int operater, SysRole sysRole)
        {
            var isExistRoleName = _db.SysRole.Where(m => m.Name == sysRole.Name && m.Id != sysRole.Id).FirstOrDefault();
            if (isExistRoleName != null)
            {
                return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "角色名字已经存在");
            }

            var _sysRole = _db.SysRole.Where(m => m.Id == sysRole.Id).FirstOrDefault();

            _sysRole.Name = sysRole.Name;
            _sysRole.Description = sysRole.Description;
            _sysRole.LastUpdateTime = DateTime.Now;
            _sysRole.Mender = operater;

            AddOperateHistory(operater, Enumeration.OperateType.Update, _sysRole.Id, string.Format("修改角色{0}(ID:{1})", _sysRole.Name, _sysRole.Id));


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");
        }

        public CustomJsonResult DeleteRole(int operater, int[] ids)
        {

            if (ids != null)
            {
                foreach (var id in ids)
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

                    AddOperateHistory(operater, Enumeration.OperateType.Delete, role.Id, string.Format("删除角色{0}(ID:{1})", role.Name, role.Id));
                }

            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public CustomJsonResult AddUserToRole(int operater, int roleId, int[] userIds)
        {
            foreach (int userId in userIds)
            {
                _db.SysUserRole.Add(new SysUserRole { UserId = userId, RoleId = roleId });
                _db.SaveChanges();

                AddOperateHistory(operater, Enumeration.OperateType.Update, roleId, string.Format("添加用户(ID：{0})到角色(ID:{1})", userId, roleId));
            }
            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult RemoveUserFromRole(int operater, int roleId, int[] userIds)
        {
            foreach (int userId in userIds)
            {
                SysUserRole userRole = _db.SysUserRole.Find(roleId, userId);
                _db.SysUserRole.Remove(userRole);
                _db.SaveChanges();


                AddOperateHistory(operater, Enumeration.OperateType.Update, roleId, string.Format("移除用户(ID：{0})所在的角色(ID:{1})", userId, roleId));

            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "移除成功");
        }

        public List<SysMenu> GetRoleMenus(int roleId)
        {
            var model = from c in _db.SysMenu
                        where
                            (from o in _db.SysRoleMenu where o.RoleId == roleId select o.MenuId).Contains(c.Id)
                        select c;

            return model.ToList();
        }

        public CustomJsonResult SaveRoleMenu(int operater, int roleId, int[] menuIds)
        {

            List<SysRoleMenu> roleMenuList = _db.SysRoleMenu.Where(r => r.RoleId == roleId).ToList();

            foreach (var roleMenu in roleMenuList)
            {
                _db.SysRoleMenu.Remove(roleMenu);
            }


            if (menuIds != null)
            {
                foreach (var menuId in menuIds)
                {
                    _db.SysRoleMenu.Add(new SysRoleMenu { RoleId = roleId, MenuId = menuId });
                }
            }

            _db.SaveChanges();

            AddOperateHistory(operater, Enumeration.OperateType.Update, roleId, string.Format("保存角色(ID:{0})菜单", roleId));

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
        }

        public CustomJsonResult CreateMenu(int operater, SysMenu sysMenu, string[] perssionId)
        {
            sysMenu.Creator = operater;
            sysMenu.CreateTime = DateTime.Now;
            _db.SysMenu.Add(sysMenu);
            _db.SaveChanges();

            if (perssionId != null)
            {
                foreach (var id in perssionId)
                {
                    _db.SysMenuPermission.Add(new SysMenuPermission { MenuId = sysMenu.Id, PermissionId = id });
                }
            }
            _db.SaveChanges();

            AddOperateHistory(operater, Enumeration.OperateType.New, sysMenu.Id, string.Format("新建菜单(ID:{0})", sysMenu.Id));

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "添加成功");
        }

        public CustomJsonResult UpdateMenu(int operater, SysMenu sysMenu, string[] perssions)
        {

            var _sysMenu = _db.SysMenu.Where(m => m.Id == sysMenu.Id).FirstOrDefault();

            _sysMenu.Name = sysMenu.Name;
            _sysMenu.Url = sysMenu.Url;
            _sysMenu.Description = sysMenu.Description;
            _sysMenu.Mender = operater;
            _sysMenu.LastUpdateTime = DateTime.Now;

            var sysMenuPermission = _db.SysMenuPermission.Where(r => r.MenuId == sysMenu.Id).ToList();
            foreach (var m in sysMenuPermission)
            {
                _db.SysMenuPermission.Remove(m);
            }


            if (perssions != null)
            {
                foreach (var m in perssions)
                {
                    _db.SysMenuPermission.Add(new SysMenuPermission { MenuId = sysMenu.Id, PermissionId = m });
                }
            }

            AddOperateHistory(operater, Enumeration.OperateType.Update, sysMenu.Id, string.Format("修改菜单(ID:{0})", sysMenu.Id));

            _db.SaveChanges();

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "修改成功");

        }

        public CustomJsonResult DeleteMenu(int operater, int[] ids)
        {
            if (ids != null)
            {
                foreach (var id in ids)
                {
                    var sysMenu = _db.SysMenu.Where(m => m.Id == id).FirstOrDefault();

                    _db.SysMenu.Remove(sysMenu);

                    var sysRoleMenus = _db.SysRoleMenu.Where(r => r.MenuId == id).ToList();
                    foreach (var sysRoleMenu in sysRoleMenus)
                    {
                        _db.SysRoleMenu.Remove(sysRoleMenu);
                    }

                    _db.SaveChanges();

                    AddOperateHistory(operater, Enumeration.OperateType.Delete, id, string.Format("删除菜单(ID:{0})", id));

                }
            }

            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "删除成功");
        }

        public List<SysPermission> GetPermissionList(PermissionCode permission)
        {
            Type t = permission.GetType();
            List<SysPermission> list = new List<SysPermission>();
            //SysPermission p = new SysPermission();
            //p.Id = "0";
            //p.Name = "权限集合";
            //p.PId = "";
            //list.Add(p);
            list = GetBasePermissionList(t, list);
            return list;
        }

        private List<SysPermission> GetBasePermissionList(Type t, List<SysPermission> list)
        {
            if (t.Name != "Object")
            {
                System.Reflection.FieldInfo[] properties = t.GetFields();
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
                    list.Add(model);
                }
                list = GetBasePermissionList(t.BaseType, list);
            }
            return list;
        }

    }
}
