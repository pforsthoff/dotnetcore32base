using System;
using System.Text;

namespace Cheetas3.EU.Application.Common.Extensions
{
    public static class StringExtensions
    {
        public static Byte[] ToByteArray(this string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        public static string ToMessageString(this byte[] value)
        {
            return Encoding.UTF8.GetString(value);
        }
    }
}