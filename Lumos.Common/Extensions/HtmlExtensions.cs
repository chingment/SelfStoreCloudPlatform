using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace System
{

    public static class HtmlExtensions
    {
        public static string ToJsonString(this NameValueCollection form)
        {
            var sb = new StringBuilder();
            sb.Append('{');
            if (form.AllKeys != null && form.AllKeys.Length > 0)
            {
                for (int i = 0; i < form.AllKeys.Length; i++)
                {
                    sb.AppendFormat("\"{0}\":\"{1}\"", form.AllKeys[i], form[form.AllKeys[i]]);
                    if (i < form.AllKeys.Length - 1) sb.Append(',');
                }
            }
            sb.Append('}');
            return sb.ToString();
        }
    }
}