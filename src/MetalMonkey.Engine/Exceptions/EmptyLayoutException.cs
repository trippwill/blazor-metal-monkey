using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Core.Exceptions
{
    class EmptyLayoutException : InvalidOperationException
    {
        public EmptyLayoutException(IEnumerable<string> route)
            : base($"No layout was found for the route: {string.Join("/", route)}")
        {
            this.Route = route;
        }

        public IEnumerable<string> Route { get; }
    }
}
