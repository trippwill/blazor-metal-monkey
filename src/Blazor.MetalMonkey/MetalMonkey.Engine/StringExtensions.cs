using System;
using System.Collections.Generic;

namespace MetalMonkey.Engine
{
    public static class StringExtensions
    {
        private const char PathSeparator = '/';

        public static string JoinPath(this IEnumerable<string?> segments) => string.Join(PathSeparator, segments);

        public static IEnumerable<string> SplitPath(this string path) =>
            path.Split(PathSeparator, System.StringSplitOptions.TrimEntries | System.StringSplitOptions.RemoveEmptyEntries);

        public static bool EqualsIgnoreCase(this string? left, string? right) =>
            left is null ? right is null : left.Equals(right, StringComparison.OrdinalIgnoreCase);

        public static bool IsNotEmpty(this string? value) => !string.IsNullOrEmpty(value);
    }
}