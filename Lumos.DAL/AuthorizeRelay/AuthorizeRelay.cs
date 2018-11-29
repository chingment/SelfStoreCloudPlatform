using System;
using System.Collections.Generic;
using System.Linq;
using Lumos.Entity;
using System.Reflection;
using System.Transactions;


namespace Lumos.DAL.AuthorizeRelay
{

    public class LoginUserInfo
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public Enumeration.BelongSite BelongSite { get; set; }

        public string MerchantId { get; set; }
    }

    public class LoginResult
    {

        private Enumeration.LoginResult _ResultType;
        private Enumeration.LoginResultTip _ResultTip;
        private LoginUserInfo _User;

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


        public LoginUserInfo User
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

        public LoginResult(Enumeration.LoginResult pType, Enumeration.LoginResultTip pTip, LoginUserInfo pUser)
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

        private void AddOperateHistory(string operater, Enumeration.OperateType operateType, string referenceId, string content)
        {
            SysOperateHistory operateHistory = new SysOperateHistory();
            operateHistory.Id = GuidUtil.New();
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


                var loginUserInfo = new LoginUserInfo();
                loginUserInfo.UserId = user.Id;
                loginUserInfo.UserName = user.UserName;
                loginUserInfo.BelongSite = user.BelongSite;

                switch (user.BelongSite)
                {
                    case Enumeration.BelongSite.Merchant:

                        var sysMerchantUser = _db.SysMerchantUser.Where(m => m.Id == user.Id).FirstOrDefault();
                        if (sysMerchantUser != null)
                        {
                            loginUserInfo.MerchantId = sysMerchantUser.MerchantId;
                        }

                        break;
                }

                bool isFlag = PassWordHelper.VerifyHashedPassword(user.PasswordHash, pPassword);

                if (!isFlag)
                {
                    result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserPasswordIncorrect, loginUserInfo);
                }
                else
                {

                    if (user.Status == Enumeration.UserStatus.Disable)
                    {
                        result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserDisabled, loginUserInfo);
                    }
                    else
                    {
                        if (user.IsDelete)
                        {
                            result = new LoginResult(Enumeration.LoginResult.Failure, Enumeration.LoginResultTip.UserDeleted, loginUserInfo);
                        }
                        else
                        {
                            user.LastLoginTime = pLoginTime;
                            user.LastLoginIp = pLoginIp;
                            _db.SaveChanges();

                            result = new LoginResult(Enumeration.LoginResult.Success, Enumeration.LoginResultTip.VerifyPass, loginUserInfo);

                        }
                    }
                }
            }


            return result;
        }

        public CustomJsonResult ChangePassword(string operater, string userId, string oldpassword, string newpassword)
        {

            var sysUser = _db.SysUser.Where(m => m.Id == userId).FirstOrDefault();
            if (sysUser != null)
            {

                if (!PassWordHelper.VerifyHashedPassword(sysUser.PasswordHash, oldpassword))
                {
                    return new CustomJsonResult(ResultType.Failure, ResultCode.Failure, "旧密码不正确");
                }

                sysUser.PasswordHash = PassWordHelper.HashPassword(newpassword);
                sysUser.Mender = operater;
                sysUser.MendTime = DateTime.Now;

                _db.SaveChanges();
            }


            return new CustomJsonResult(ResultType.Success, ResultCode.Success, "操作成功");
        }

        public List<SysPermission> GetPermissionList(Type type)
        {
            List<SysPermission> list = new List<SysPermission>();
            list = GetBasePermissionList(type, list);
            return list;
        }

        private List<SysPermission> GetBasePermissionList(Type type, List<SysPermission> sysPermission)
        {
            if (type.Name != "Object")
            {
                System.Reflection.FieldInfo[] properties = type.GetFields();
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
                    sysPermission.Add(model);
                }
                sysPermission = GetBasePermissionList(type.BaseType, sysPermission);
            }
            return sysPermission;
        }

    }
}
