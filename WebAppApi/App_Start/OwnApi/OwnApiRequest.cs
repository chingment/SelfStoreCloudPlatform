using Lumos;
using Lumos.Session;
using System.Web;

namespace WebAppApi
{
    public static class OwnApiRequest
    {
        public const string SESSION_NAME = "SessionName";


        public static string GetCurrentUserId()
        {
            var userInfo = GetUserInfo();
            if (userInfo == null)
                return null;


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

            if (userInfo == null)
                return null;

            LogUtil.Info("CurrentUserId：" + userInfo.UserId);

            return userInfo;
        }

    }
}