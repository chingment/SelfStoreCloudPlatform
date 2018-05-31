using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
namespace Lumos.Entity
{
    public class EnumerationRemarkConverter<T> : JsonConverter
    {
        public EnumerationRemarkConverter()
        {

        }


        /// <summary>
        /// 是否允许转换
        /// </summary>
        public override bool CanConvert(Type objectType)
        {
            string a = objectType.Name;
            string a1 = typeof(T).Name;
            bool canConvert = false;
            if (a == a1)
            {
                canConvert = true;
            }
            return canConvert;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string a = typeof(T).Name;
            return "";
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {

            Type type = typeof(T);
            FieldInfo fd = type.GetField(value.ToString());

            object[] attrs = fd.GetCustomAttributes(typeof(RemarkAttribute), false);
            string name = string.Empty;
            foreach (RemarkAttribute attr in attrs)
            {
                name = attr.CnName;
            }


            writer.WriteValue(name);
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 是否允许转换JSON字符串时调用
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
    }
}
