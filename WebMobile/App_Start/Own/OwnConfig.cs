using Lumos.Common;
using System.Linq;

namespace WebMobile
{
    public class OwnConfig
    {
        /// <summary>
        /// 获取登录的页面
        /// </summary>
        /// <returns></returns>
        public static string GetLoginPage(string returnUrl)
        {
            //if (string.IsNullOrEmpty(returnUrl))
            //    return "/Account/Login";

            return string.Format("/Account/Login?returnUrl={0}", returnUrl);
        }

        public static string WxOauth2(string returnUrl)
        {


            //if (string.IsNullOrEmpty(returnUrl))
            //    return "/Home/Oauth2";

            return string.Format("/Home/Oauth2?returnUrl={0}", returnUrl);
        }

        /// <summary>
        /// 获取登录后的主界面
        /// </summary>
        /// <returns></returns>
        public static string GetHomePage()
        {
            return "/Home/Index";
        }

        public static string GerServicePhone()
        {
            return "4000124508";
        }

        /// <summary>
        /// 获取网站主页的名称
        /// </summary>
        /// <returns></returns>
        public static string GetHomeTitle()
        {
            return "主页";
        }

        /// <summary>
        /// 获取网站的名称
        /// </summary>
        /// <returns></returns>
        public static string GetWebName()
        {
            return "贩聚社团";
        }

        /// <summary>
        /// 是否能查看错误日志的堆栈
        /// </summary>
        /// <returns></returns>
        public static bool CanViewErrorStackTrace()
        {
            string[] canViewIp = new string[] { "127.0.0.1", "::1" };


            string ip = CommonUtils.GetIP();

            if (canViewIp.Contains(ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetDoMain()
        {
            return System.Configuration.ConfigurationManager.AppSettings["custom:WxAppDomain"];
        }
    }
}