using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Cheetas3.EU.Converter.Entities.Base
{
    [DataContract]
    public abstract class Entity
    {
        public byte[] ToMessage()
        {
            return Encoding.UTF8.GetBytes(ToJsonString(this));
        }

        public static string ToJsonString(object value)
        {
            var ser = new DataContractJsonSerializer(value.GetType());
            using var ms = new MemoryStream();
            ser.WriteObject(ms, value);
            var json = Encoding.UTF8.GetString(ms.ToArray());
            return json;
        }
    }
}
