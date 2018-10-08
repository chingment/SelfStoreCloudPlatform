using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class ObjectExtensions
    {
        public static string ToJsonString(this Object obj)
        {
            if (obj == null)
            {
                return null;
            }

            string rt = null;
            try
            {
                rt = Newtonsoft.Json.JsonConvert.SerializeObject(obj);
            }
            catch
            {

            }

            return rt;
        }

        public static T ToJsonObject<T>(this Object s)
        {
            if (s == null)
                return default(T);

            try
            {
                T t = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(Newtonsoft.Json.JsonConvert.SerializeObject(s));

                return t;
            }
            catch
            {
                return default(T);
            }
        }
    }
}
