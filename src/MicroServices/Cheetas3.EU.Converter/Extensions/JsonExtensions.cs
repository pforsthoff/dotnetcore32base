using Cheetas3.EU.Converter.Entities.Base;
using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Cheetas3.EU.Converter.Extensions
{
    public static class JsonExtensions
    {

        public static object FromJson(this string json, Type type)
        {
            return FromJsonString(json, type);
        }

        public static object FromJsonString(string json, Type type)
        {
            using var ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            var ser = new DataContractJsonSerializer(type);
            var value = ser.ReadObject(ms);
            return value;
        }

        public static string ToJson(this object value)
        {
            return ToJsonString(value);
        }

        public static Byte[] ToMessage(this object entity)
        {
            var json = entity.ToJson();
            return json.ToByteArray();
        }

        //public static string ToMessage<T>(this T entity) where T : Entity
        //{
        //    return entity.ToByteArray();
        //}

        public static string ToJsonString(object value)
        {
            var ser = new DataContractJsonSerializer(value.GetType());
            using var ms = new MemoryStream();
            ser.WriteObject(ms, value);
            var json = Encoding.Default.GetString(ms.ToArray());
            return json;
        }
    }
}
