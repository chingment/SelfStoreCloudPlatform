using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMerch
{
    public class OwnWebSettingUtils
    {
        /// <summary>
        /// 获取登录的页面
        /// </summary>
        /// <returns></returns>
        public static string GetLoginPage(string returnUrl = "")
        {
            string server = System.Configuration.ConfigurationManager.AppSettings["custom:LoginServerUrl"];
            // string loginUrl = string.Format("{0}?returnUrl={1}", server, HttpUtility.UrlEncode(returnUrl));

            string loginUrl = string.Format("{0}", server);
            return loginUrl;
        }

        /// <summary>
        /// 获取登录后的主界面
        /// </summary>
        /// <returns></returns>
        public static string GetHomePage()
        {
            return "/Home/Index";
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
            return "贩聚社团商家平台";
        }

    }
}