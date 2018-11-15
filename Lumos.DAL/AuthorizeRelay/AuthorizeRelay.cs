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


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "保存成功");
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
