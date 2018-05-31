using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBack
{

    public static class OwnBreadcrumb
    {
        public static readonly string WebName = OwnWebSettingUtils.GetWebName();
        public static readonly string HomeTite = OwnWebSettingUtils.GetHomeTitle();
        public static readonly string HomePage = OwnWebSettingUtils.GetHomePage();
        public static IHtmlString Render()
        {

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();
            //sb.Append("<ul class=\"breadcrumb\">");
            //if (currNode == null || currNode.ParentNode == null)
            //{
            //    sb.Append("<li><a href=\"" + OwnBreadcrumb.HomePage + "\" class=\"root\"> &nbsp; </a></li>");
            //    return new MvcHtmlString(sb.ToString());
            //}
            //else
            //{

            //    var parents = new List<string>();
            //    SiteMapNode parent = currNode.ParentNode;
            //    while (parent != null)
            //    {
            //        string calssName = "site";
            //        string title = parent.Title;
            //        if (parent.Title == OwnBreadcrumb.HomeTite)
            //        {
            //            calssName = "root";
            //            title = "&nbsp;";
            //        }
            //        string html = "<li><a href=\"" + parent.Url + "\" class=\"" + calssName + "\">" + title + "</a></li>";
            //        if (parent.Url.IndexOf("#") > -1)
            //        {
            //            html = "<li><span  class=\"" + calssName + "\">" + title + "</span></li>";
            //        }

            //        parents.Add(html);

            //        parent = parent.ParentNode;
            //    }

            //    parents.Reverse();
            //    parents.Add(String.Format("<li><span class=\"site\">{0}</span></li>", currNode.Title));

            //    parents.ForEach(node => sb.Append(node));
            //}

            //sb.Append(" </ul>");
            //return new MvcHtmlString(sb.ToString());

             return new MvcHtmlString("");

        }

        public static string GetTitle()
        {
            return "";
        }
    }

}