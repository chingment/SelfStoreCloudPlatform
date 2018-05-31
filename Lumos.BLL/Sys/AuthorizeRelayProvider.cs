using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using Lumos.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL
{
    public class AuthorizeRelayProvider : BaseProvider
    {
        private AuthorizeRelay _authorizeRelay = new AuthorizeRelay();

        public LoginResult SignIn(string username, string password, string loginIp, Enumeration.LoginType loginType)
        {
            DateTime timeNow = DateTime.Now;

            IpInfo ipInfo = new IpInfo();
            SysUserLoginHistory loginHis = new SysUserLoginHistory();
            loginHis.LoginTime = timeNow;
            loginHis.Ip = ipInfo.Ip;
            loginHis.Country = ipInfo.Country;
            loginHis.Province = ipInfo.Province;
            loginHis.City = ipInfo.City;
            loginHis.LoginType = loginType;

            var result = _authorizeRelay.SignIn(username, password, timeNow, loginIp);

            switch (result.ResultTip)
            {
                case Enumeration.LoginResultTip.UserNotExist:
                    loginHis.Description = "登录失败";
                    loginHis.Result = Enumeration.LoginResult.Failure;
                    break;
                case Enumeration.LoginResultTip.UserAccessFailedMaxCount:
                    loginHis.Description = "登录失败,连续输入错误密码3次,锁定帐号30分钟";
                    loginHis.Result = Enumeration.LoginResult.Failure;
                    break;
                case Enumeration.LoginResultTip.UserPasswordIncorrect:
                    loginHis.Description = "登录失败,密码错误";
                    loginHis.Result = Enumeration.LoginResult.Failure;
                    loginHis.UserId = result.User.Id;
                    break;
                case Enumeration.LoginResultTip.UserDisabled:
                    loginHis.Description = "登录失败,帐号被禁用";
                    loginHis.Result = Enumeration.LoginResult.Failure;
                    loginHis.UserId = result.User.Id;
                    break;
                case Enumeration.LoginResultTip.UserDeleted:
                    loginHis.Description = "登录失败,帐号已删除";
                    loginHis.Result = Enumeration.LoginResult.Failure;
                    loginHis.UserId = result.User.Id;
                    break;
                case Enumeration.LoginResultTip.VerifyPass:
                    loginHis.Description = "登录成功";
                    loginHis.Result = Enumeration.LoginResult.Success;
                    loginHis.UserId = result.User.Id;

                    break;
            }

            CurrentDb.SysUserLoginHistory.Add(loginHis);
            CurrentDb.SaveChanges();


            return result;
        }

        public bool UserNameIsExists(string userName)
        {
            return _authorizeRelay.UserNameIsExists(userName);
        }

        public List<SysMenu> GetUserMenus(int userId)
        {
            return _authorizeRelay.GetUserMenus(userId);
        }

        public List<string> GetUserPermissions(int userId)
        {
            return _authorizeRelay.GetUserPermissions(userId);
        }

        public CustomJsonResult CreateUser<T>(int operater, T user, params int[] roleIds) where T : SysUser
        {
            return _authorizeRelay.CreateUser<T>(operater, user, roleIds);
        }

        public CustomJsonResult UpdateUser<T>(int operater, T user, params int[] roleIds) where T : SysUser
        {
            return _authorizeRelay.UpdateUser<T>(operater, user, roleIds);
        }

        public CustomJsonResult DeleteUser(int operater, int[] userIds)
        {
            return _authorizeRelay.DeleteUser(operater, userIds);
        }

        public CustomJsonResult ChangePassword(int operater, int userId, string oldpassword, string newpassword)
        {
            return _authorizeRelay.ChangePassword(operater, userId, oldpassword, newpassword);
        }

        public CustomJsonResult CreateRole(int operater, SysRole role)
        {
            return _authorizeRelay.CreateRole(operater, role);
        }

        public CustomJsonResult UpdateRole(int operater, SysRole role)
        {
            return _authorizeRelay.UpdateRole(operater, role);
        }

        public CustomJsonResult DeleteRole(int operater, int[] ids)
        {
            return _authorizeRelay.DeleteRole(operater, ids);
        }

        public CustomJsonResult SaveRoleMenu(int operater, int roleId, int[] menuIds)
        {
            return _authorizeRelay.SaveRoleMenu(operater, roleId, menuIds);
        }

        public CustomJsonResult RemoveUserFromRole(int operater, int roleId, int[] userIds)
        {
            return _authorizeRelay.RemoveUserFromRole(operater, roleId, userIds);
        }

        public CustomJsonResult AddUserToRole(int operater, int roleId, int[] userIds)
        {
            return _authorizeRelay.AddUserToRole(operater, roleId, userIds);
        }

        public List<SysMenu> GetRoleMenus(int roleId)
        {
            return _authorizeRelay.GetRoleMenus(roleId);
        }

        public CustomJsonResult CreateMenu(int operater, SysMenu sysMenu, string[] perssionId)
        {
            return _authorizeRelay.CreateMenu(operater, sysMenu, perssionId);
        }

        public CustomJsonResult UpdateMenu(int operater, SysMenu sysMenu, string[] perssionIds)
        {
            return _authorizeRelay.UpdateMenu(operater, sysMenu, perssionIds);
        }

        public CustomJsonResult DeleteMenu(int operater, int[] ids)
        {
            return _authorizeRelay.DeleteMenu(operater, ids);
        }

        public List<SysPermission> GetPermissionList(PermissionCode permission)
        {
            return _authorizeRelay.GetPermissionList(permission);
        }
    }
}