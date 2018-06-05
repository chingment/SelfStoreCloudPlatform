using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebMerch
{

    public static class OwnBreadcrumb
    {
        public static readonly string WebName = OwnWebSettingUtils.GetWebName();
        public static readonly string HomeTite = OwnWebSettingUtils.GetHomeTitle();
        public static readonly string HomePage = OwnWebSettingUtils.GetHomePage();
        public static IHtmlString Render()
        {
            return new MvcHtmlString("");
        }

        public static string GetTitle()
        {
            return "";
        }
    }

}