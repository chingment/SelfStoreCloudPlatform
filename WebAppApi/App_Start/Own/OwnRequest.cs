using Lumos.BLL;
using Lumos.Entity;
using Lumos.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi
{
    public static class OwnRequest
    {
        public const string SESSION_NAME = "SessionName";


        public static string GetCurrentUserId()
        {
            var userInfo = GetUserInfo();
            if (userInfo == null)
                return "00000000000000000000000000000000";

            return userInfo.UserId;
        }

        public static UserInfo GetUserInfo()
        {
            UserInfo userInfo = null;

            //var context = HttpContext.Current;
            //var request = context.Request;
            //var response = context.Response;

            //var token = request.Cookies[OwnRequest.SESSION_NAME];
            //if (token == null)
            //    return null;

            //userInfo = SSOUtil.GetUserInfo(token.Value);

            userInfo = new UserInfo();
            userInfo.Token = "1";
            userInfo.UserId = "00000000000000000000000000000000";
            userInfo.UserName = "admin";

            return userInfo;
        }

    }
}