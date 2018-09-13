using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebMobile
{
    public static class OwnStaticScriptsResource
    {
        public static IHtmlString Render(string path)
        {
            string strPath = "/Scripts/" + path;
            string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:StaticResourceServerUrl"];
            if (strUrl != null)
            {
                strPath = strUrl + strPath;
            }


            return new MvcHtmlString("<script src=\"" + strPath + "\" type=\"text/javascript\"></script>");
        }
    }

    public static class OwnStaticStylesResource
    {
        public static IHtmlString Render(string path)
        {
            string strPath = "/Content/" + path;
            string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:StaticResourceServerUrl"];
            if (strUrl != null)
            {
                strPath = strUrl + strPath;
            }

            return new MvcHtmlString("<link href=\"" + strPath + "\" rel=\"stylesheet\"/>");
        }
    }

    public static class OwnStaticImagesResource
    {
        public static string Render(string path)
        {
            string strPath = "/Content/base/images/" + path;

            string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:StaticResourceServerUrl"];
            if (strUrl != null)
            {
                strPath = strUrl + strPath;
            }



            return strPath;
        }

        public static string TabbarIcon(string name)
        {
            string img = "";
            string url = HttpContext.Current.Request.Url.AbsolutePath;
            switch (name)
            {
                case "首页":
                    img = "home";
                    if (url.IndexOf("/Home/Index") > -1)
                    {
                        img += "_active";
                    }
                    break;
                case "购物车":
                    img = "catalog";
                    break;
                case "个人":
                    img = "cart";
                    break;
            }
            img = "footer_tab_icon_" + img + ".png";

            StringBuilder sb = new StringBuilder();
            sb.Append("< span class=\"bar-icon\">");
            sb.Append("<img src = \"" + Render(img) + "\" />");
            sb.Append(" </span>");
            sb.Append(" < span class=\"bar-txt seleced\">"+ name + "</span>");

            return Render(img);

        }
    }

}