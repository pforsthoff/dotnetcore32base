using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Cheetas3.EU.Converter.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescriptionString<TEnum>(this TEnum value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());

            return fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .Cast<DescriptionAttribute>()
                .Select(x => x.Description)
                .FirstOrDefault();
        }
    }
}
