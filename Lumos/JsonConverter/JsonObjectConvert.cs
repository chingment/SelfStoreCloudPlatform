using Newtonsoft.Json;
using System;


namespace Lumos
{
    public class JsonObjectConvert : JsonConverter
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {

            return true;
        }

        /// <summary>
        /// 判断是否为Bool类型
        /// </summary>
        /// <param name="objectType">类型</param>
        /// <returns>为bool类型则可以进行转换</returns>
        public override bool CanConvert(Type objectType)
        {
            return false;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string v = value.ToString();
            writer.WriteRawValue(value.ToString());

        }
    }
}
