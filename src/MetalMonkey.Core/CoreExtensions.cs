using Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MetalMonkey.Core
{
    public static class CoreExtensions
    {
        public static bool CaseInsensitiveEquals(this string? left, string? right) =>
            left?.Equals(right, StringComparison.OrdinalIgnoreCase) ?? false;

        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            Requires.NotNull(assembly, nameof(assembly));

            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }

        public static IEnumerable<Type> GetTypesWithInterface<T>(Assembly asm)
        {
            var iface = typeof(T);
            Requires.Argument(iface.IsInterface, nameof(T), "Type argument T must be an interface");
            return asm.GetLoadableTypes().Where(t => iface.IsAssignableFrom(t) && t.GetInterfaces().Contains(iface)).ToList();
        }

    }
}
