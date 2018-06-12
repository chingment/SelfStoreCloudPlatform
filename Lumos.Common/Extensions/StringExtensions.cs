using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class StringExtensions
    {
        public static string ToSearchString(this string s)
        {

            if (s == null)
                return "";

            s = s.Trim();
            if (s.Length > 1000)
            {
                s = s.Substring(0, 1000);
            }

            return s.ToString();
        }

        public static string NullToEmpty(this object s)
        {
            if (s == null)
            {
                return "";
            }
            else
            {
                return s.ToString().Trim();
            }
        }

        public static string NullStringToNullObject(this object s)
        {
            if (s == null)
            {
                return null;
            }
            else
            {
                if (s.ToString().Trim().ToUpper() == "NULL")
                    return null;

                return s.ToString();
            }
        }

        public static T ToJsonObject<T>(this string s)
        {
            if (s == null)
                return default(T);

            if (string.IsNullOrEmpty(s))
                return default(T);

            try
            {
                T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(s);

                return t;
            }
            catch
            {
                return default(T);
            }
        }

    }
}
