using Lumos.DAL.AuthorizeRelay;
using Lumos.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.BLL.Service.Admin
{
    public class AuthorizeRelayProvider : BaseProvider
    {
        private AuthorizeRelay _authorizeRelay = new AuthorizeRelay();

        public LoginResult SignIn(string pUsername, string pPassword, string pLoginIp, Enumeration.LoginType pLoginType)
        {
            DateTime timeNow = DateTime.Now;

            IpInfo ipInfo = new IpInfo();
            SysUserLoginHistory loginHis = new SysUserLoginHistory();
            loginHis.Id = GuidUtil.New();
            loginHis.LoginTime = timeNow;
            loginHis.Ip = ipInfo.Ip;
            loginHis.Country = ipInfo.Country;
            loginHis.Province = ipInfo.Province;
            loginHis.City = ipInfo.City;
            loginHis.LoginType = pLoginType;
            loginHis.CreateTime = this.DateTime;
            var result = _authorizeRelay.SignIn(pUsername, pPassword, timeNow, pLoginIp);

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
                    loginHis.Creator = result.User.Id;
                    break;
            }

            if (loginHis.UserId == null)
            {
                loginHis.UserId = "-1";
            }

            CurrentDb.SysUserLoginHistory.Add(loginHis);
            CurrentDb.SaveChanges();


            return result;
        }

        public List<SysMenu> GetUserMenus(string pUserId)
        {
            return _authorizeRelay.GetUserMenus(pUserId);
        }

        public List<string> GetUserPermissions(string pUserId)
        {
            return _authorizeRelay.GetUserPermissions(pUserId);
        }

        public CustomJsonResult ChangePassword(string pOperater, string pUserId, string pOldpassword, string pNewpassword)
        {
            return _authorizeRelay.ChangePassword(pOperater, pUserId, pOldpassword, pNewpassword);
        }

        public List<SysPermission> GetPermissionList(PermissionCode pPermissionCode)
        {
            return _authorizeRelay.GetPermissionList(pPermissionCode);
        }
    }
}