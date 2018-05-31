using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class DateTimeUtility
    {
        public static string ToUnifiedFormatDateTime(this DateTime instance)
        {
            if (instance == null)
                return "";
            return instance.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToUnifiedFormatDateTime(this DateTime instance, string nullhandler)
        {
            if (instance == null)
                return nullhandler;
            return instance.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToUnifiedFormatDateTime(this DateTime? instance)
        {
            if (instance == null)
                return "";
            return DateTime.Parse(instance.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToUnifiedFormatDateTime(this DateTime? instance, string nullhandler)
        {
            if (instance == null)
                return nullhandler;
            return DateTime.Parse(instance.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToUnifiedFormatDate(this DateTime instance)
        {
            if (instance == null)
                return "";
            return DateTime.Parse(instance.ToString()).ToString("yyyy-MM-dd");
        }

        public static string ToUnifiedFormatDate(this DateTime? instance)
        {
            if (instance == null)
                return "";
            return DateTime.Parse(instance.ToString()).ToString("yyyy-MM-dd");
        }

        public static string ToUnifiedFormatDate(this DateTime? instance, string nullhandler)
        {
            if (instance == null)
                return nullhandler;
            return DateTime.Parse(instance.ToString()).ToString("yyyy-MM-dd");
        }
    }
}
