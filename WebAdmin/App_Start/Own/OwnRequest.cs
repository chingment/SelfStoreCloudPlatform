﻿using Lumos.BLL;
using Lumos.BLL.Service.Admin;
using Lumos.Entity;
using Lumos.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAdmin
{
    public static class OwnRequest
    {
        public const string SESSION_NAME = "Session_WebAdmin";

        public static string GetCurrentUserId()
        {
            var userInfo = GetUserInfo();
            if (userInfo == null)
                return "";

            return userInfo.UserId;
        }

        public static UserInfo GetUserInfo()
        {
            UserInfo userInfo = null;

            var context = HttpContext.Current;
            var request = context.Request;
            var response = context.Response;

            var token = request.Cookies[OwnRequest.SESSION_NAME];
            if (token == null)
                return null;

            userInfo = SSOUtil.GetUserInfo(token.Value);

            return userInfo;
        }

        public static string GetAccessToken()
        {
            var context = HttpContext.Current;
            var request = context.Request;
            var response = context.Response;

            var token = request.Cookies[OwnRequest.SESSION_NAME];
            if (token == null)
                return null;

            return token.Value;

        }

        public static bool IsLogin()
        {
            if (GetUserInfo() == null)
                return false;

            return true;
        }

        public static string GetUserNameWithSymbol()
        {
            var userInfo = GetUserInfo();
            if (userInfo == null)
                return "";

            string userName = userInfo.UserName;

            char[] c = userName.ToCharArray();
            if (userName.Length > 2 & userName.Length <= 5)
            {
                c[1] = '*';
            }
            else if (userName.Length > 5 && userName.Length <= 9)
            {
                c[3] = '*';
                c[4] = '*';
            }
            else if (userName.Length > 9)
            {
                c[3] = '*';
                c[4] = '*';
                c[5] = '*';
            }

            userName = new string(c);

            return userName;
        }

        public static bool IsInPermission(string[] permissions)
        {
            var userId = GetCurrentUserId();

            List<string> listPermissions = AdminServiceFactory.SysAdminUser.GetPermissions(userId, userId);
            if (listPermissions == null)
                return false;
            if (listPermissions.Count < 1)
                return false;

            bool isHas = false;
            foreach (var permission in listPermissions)
            {
                foreach (var m in permissions)
                {
                    if (permission.Trim() == m.Trim())
                    {
                        isHas = true;
                        break;
                    }
                }
                if (isHas)
                {
                    break;
                }
            }

            return isHas;
        }

        public static void Postpone()
        {

            SSOUtil.Postpone(GetAccessToken());


        }

        public static void Quit()
        {

            SSOUtil.Quit(GetAccessToken());

            var context = HttpContext.Current;
            var request = context.Request;
            var response = context.Response;
            HttpCookie cookie_session = request.Cookies[OwnRequest.SESSION_NAME];
            if (cookie_session != null)
            {
                TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                cookie_session.Expires = DateTime.Now.Add(ts);
                response.AppendCookie(cookie_session);
            }



        }
    }
}