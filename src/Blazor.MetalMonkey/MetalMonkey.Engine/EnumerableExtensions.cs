using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalMonkey.Engine
{
    public static class EnumerableExtensions
    {
        public static bool SequenceStartsWith<T>(this IEnumerable<T> sequence, IEnumerable<T> value, int elementCount, IEqualityComparer<T>? comparer = null) =>
            sequence
            .Take(elementCount)
            .SequenceEqual(value, comparer);

        public static bool SequenceStartsWith<T>(this IEnumerable<T> sequence, IList<T> value, IEqualityComparer<T>? comparer = null) =>
            sequence.SequenceStartsWith(value, value.Count, comparer);
    }
}
