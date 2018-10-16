using Lumos.BLL;
using Lumos.Entity;
using Lumos.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppApi
{
    public static class OwnApiRequest
    {
        public const string SESSION_NAME = "SessionName";


        public static string GetCurrentUserId()
        {
            var userInfo = GetUserInfo();
            return userInfo.UserId;
        }

        public static UserInfo GetUserInfo()
        {
            UserInfo userInfo = null;

            var context = HttpContext.Current;
            var request = context.Request;
            var response = context.Response;

            var token = request.QueryString["accessToken"];
            if (token == null)
                return null;

            userInfo = SSOUtil.GetUserInfo(token);
            return userInfo;
        }

    }
}