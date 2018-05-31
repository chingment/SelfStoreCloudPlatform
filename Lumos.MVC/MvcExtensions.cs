using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace System.Web
{
    public static class StaticResourceServerScripts
    {
        public static IHtmlString Render(string path)
        {

            string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:StaticResourceServerUrl"];
            if (strUrl != null)
            {
                strUrl += "Scripts/" + path;
            }

            return new MvcHtmlString("<script src=\"" + strUrl + "\" type=\"text/javascript\"></script>");
        }
    }

    public static class StaticResourceServerStyles
    {
        public static IHtmlString Render(string path)
        {

            string strUrl = System.Configuration.ConfigurationManager.AppSettings["custom:StaticResourceServerUrl"];
            if (strUrl != null)
            {
                strUrl += "Content/" + path;
            }

            return new MvcHtmlString("<link href=\"" + strUrl + "\" rel=\"stylesheet\"/>");
        }
    }

}