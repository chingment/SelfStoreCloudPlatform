using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lumos.Entity
{
    // Summary:
    //     Provides a base class for converting a System.DateTime to and from JSON.
    public class EnumerationTypeConverter<T> : JsonConverter
    {
        public EnumerationTypeConverter()
        {

        }

        public string FieldName { get; set; }

        public EnumerationTypeConverter(string fieldName)
        {
            this.FieldName = fieldName;
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
            writer.WriteValue(Enum.GetName(typeof(T), value));
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
