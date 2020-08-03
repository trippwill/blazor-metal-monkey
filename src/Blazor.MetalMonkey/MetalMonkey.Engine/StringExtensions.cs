using System;
using System.Collections.Generic;

namespace MetalMonkey.Engine
{
    public static class StringExtensions
    {
        public static string JoinPath(this IEnumerable<string> segments) => string.Join('/', segments);

        public static bool EqualsIgnoreCase(this string? left, string? right) =>
            left is null ? right is null : left.Equals(right, StringComparison.OrdinalIgnoreCase); 
    }
}