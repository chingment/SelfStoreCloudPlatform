using Lumos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebMobile
{
    public class OwnWebSettingUtils
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