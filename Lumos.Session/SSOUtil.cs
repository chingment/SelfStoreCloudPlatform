using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Session
{
    public static class SSOUtil
    {
        private static string sign = "token";
        public static void SetUserInfo(UserInfo userInfo)
        {
            var session = new Session();
            session.Set(string.Format("{0}:{1}", sign, userInfo.Token), userInfo);
        }

        public static UserInfo GetUserInfo(string key)
        {
            var session = new Session();
            return session.Get<UserInfo>(string.Format("{0}:{1}", sign, key));
        }

        public static void Postpone(string key)
        {
            var session = new Session();

            session.Postpone(string.Format("{0}:{1}", sign, key));
        }

        public static void Quit(string key)
        {
            var session = new Session();

            session.Quit(string.Format("{0}:{1}", sign, key));
        }
    }
}
