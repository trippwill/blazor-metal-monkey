using System.Collections.Generic;
using System.Linq;
using MetalMonkey.Engine.Routing;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine
{
    public class Utilities
    {
        public static RenderFragment EmptyRenderFragment { get; } = _ => {};

        public static List<string> EmptyStringList { get; } = Enumerable.Empty<string>().ToList();

        public static IList<T> GetEmptyList<T>() => Enumerable.Empty<T>().ToList();
    }
}