using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Engine.Core.Extensions
{
    internal static class StringExtensions
    {
        public static bool IsEmpty(this string value) => string.Empty == value;

        public static string AsRelativePath(this IEnumerable<string> segments) => string.Join('/', segments);

        public static bool StartsWithRelativePath(this IEnumerable<string> segments, string value) =>
            segments.AsRelativePath().StartsWith(value);
    }
}
