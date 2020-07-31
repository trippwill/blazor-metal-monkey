using System;

namespace MetalMonkey.Engine.Routing
{
    public class MetalRouteContext
    {
        public MetalRouteContext(string path)
        {
            Segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < Segments.Length; i++)
            {
                Segments[i] = Uri.UnescapeDataString(Segments[i]);
            }
        }

        public string[] Segments { get; }
    }
}
