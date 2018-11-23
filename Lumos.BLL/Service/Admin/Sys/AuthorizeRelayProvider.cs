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

        public LoginResult SignIn(string username, string password, string loginIp, Enumeration.LoginType loginType)
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
            loginHis.LoginType = loginType;
            loginHis.CreateTime = this.DateTime;
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

        public List<SysMenu> GetUserMenus(string userId,Enumeration.BelongSite belongSite)
        {
            return _authorizeRelay.GetUserMenus(userId, belongSite);
        }

        public List<string> GetUserPermissions(string userId)
        {
            return _authorizeRelay.GetUserPermissions(userId);
        }

        public CustomJsonResult ChangePassword(string operater, string userId, string oldpassword, string newpassword)
        {
            return _authorizeRelay.ChangePassword(operater, userId, oldpassword, newpassword);
        }

        public List<SysPermission> GetPermissionList(Type type)
        {
            return _authorizeRelay.GetPermissionList(type);
        }
    }
}