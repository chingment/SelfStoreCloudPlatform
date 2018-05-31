using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBack
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

}