using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace System.Web
{
    public static class HmlExtension
    {
        public static string Encrypt(string plaintext)
        {
            string cl1 = plaintext;
            string pwd = string.Empty;
            MD5 md5 = MD5.Create();
            byte[] s = md5.ComputeHash(Encoding.Unicode.GetBytes(cl1));
            for (int i = 0; i < s.Length; i++)
            {
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }

        public static HtmlString GenerateUniqueSubmitIdentifier(this HtmlHelper htmlhelper)
        {
            string sessionname = Guid.NewGuid().ToString();
            HttpContext.Current.Session[sessionname] = null;

            TagBuilder builder = new TagBuilder("input");
            builder.Attributes["type"] = "hidden";
            builder.Attributes["name"] = "_UniqueSubmitIdentifier";
            builder.Attributes["value"] = Guid.NewGuid().ToString();



            TagBuilder builderSessionName = new TagBuilder("input");
            builderSessionName.Attributes["type"] = "hidden";
            builderSessionName.Attributes["name"] = "_UniqueSubmitIdentifierSessionName";
            builderSessionName.Attributes["value"] = sessionname;

            StringBuilder sb = new StringBuilder();
            sb.Append(builder.ToString(TagRenderMode.SelfClosing));
            sb.Append(builderSessionName.ToString(TagRenderMode.SelfClosing));

            return new HtmlString(sb.ToString());
        }


        public static IHtmlString initBool(this HtmlHelper helper, string name, object htmlAttributes = null)
        {
            IDictionary<string, object> HtmlAttributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            StringBuilder sb = new StringBuilder();


            var classname = "";
            if (HtmlAttributes["class"] != null)
            {
                classname = "class=\"" + HtmlAttributes["class"].ToString() + "\"";
            }

            bool selectValue = false;
            if (HtmlAttributes["selectedvalue"] != null)
            {
                selectValue = bool.Parse(HtmlAttributes["selectedvalue"].ToString());
            }

            int i = 0;
            string id = name.Replace(".", "_");
            sb.Append("<select name=\"" + name + "\" id=\"" + id + "\"  " + classname + ">");

            string checkeds = "";
            string checkeds2 = "";
            if (selectValue)
            {
                checkeds = "selected";
            }
            else
            {
                checkeds2 = "selected";
            }



            sb.Append(" <option value=\"True\"" + checkeds + ">是</option>");
            sb.Append(" <option value=\"False\"" + checkeds2 + ">否</option>");

            sb.Append("/<select>");



            return new MvcHtmlString(sb.ToString());
        }
    }
}
