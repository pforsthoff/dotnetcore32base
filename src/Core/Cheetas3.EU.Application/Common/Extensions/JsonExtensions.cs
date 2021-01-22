using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace Cheetas3.EU.Application.Common.Extensions
{
    public static class JsonExtensions
    {

        public static object FromJson(this string json, Type type)
        {
            return FromJsonString(json, type);
        }

        public static object FromJsonString(string json, Type type)
        {
            using var ms = new MemoryStream(Encoding.UTF8.GetBytes(json));
            var ser = new DataContractJsonSerializer(type);
            var value = ser.ReadObject(ms);
            return value;
        }

        public static string ToJson(this object value)
        {
            return ToJsonString(value);
        }

        public static string ToJsonString(object value)
        {

            //var settings = new System.Runtime.Serialization.Json.DataContractJsonSerializerSettings
            //{
            //    IgnoreExtensionDataObject = true,
            //    MaxItemsInObjectGraph = 1,
            //    RootName = "SliceMessage",
            //    UseSimpleDictionaryFormat = true
            //};

            var ser = new DataContractJsonSerializer(value.GetType());
            using var ms = new MemoryStream();
            ser.WriteObject(ms, value);
            var json = Encoding.UTF8.GetString(ms.ToArray());
            return json;
        }
    }
}
