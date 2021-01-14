using System;

namespace Cheetas3.EU.Mappings
{
    public static class MappingTransformations
    {
        public static string BigInt2TimeSpan(Int64 ticks)
        {
            return TimeSpan.FromTicks(ticks).ToString();
        }
    }
}
