using System;
using System.Collections.Generic;
using System.Linq;

namespace MetalMonkey.Engine.Routing
{
    public readonly struct MetalRouteContext
    {
        internal MetalRouteContext(IMetalRoute parentRoute, IEnumerable<string> parentSegments, IEnumerable<string> currentSegments)
        {
            ParentRoute = parentRoute;
            ParentSegments = parentSegments;
            CurrentSegments = currentSegments;
        }

        public IEnumerable<string> ParentSegments {get;}

        public IEnumerable<string> CurrentSegments {get;}

        public IMetalRoute ParentRoute { get; }

        internal static MetalRouteContext FromBaseRelativePath(MetalRouter router, string relativePath)
        {
            return new MetalRouteContext(
                router,
                Enumerable.Empty<string>(),
                relativePath.Split('/')
            );
        }

        internal MetalRouteContext MoveCapturedToParent(IMetalRoute parentRoute, IEnumerable<string> capturedSegments)
        {
            var currentCapturedSegments = CurrentSegments.TakeWhile((cseg, idx) => capturedSegments.ElementAtOrDefault(idx).EqualsIgnoreCase(cseg));
            if (currentCapturedSegments is null || !currentCapturedSegments.Any())
            {
                return this;
            }

            return new MetalRouteContext(
                parentRoute,
                ParentSegments.Concat(currentCapturedSegments),
                CurrentSegments.Skip(currentCapturedSegments.Count())
                );
        }

        public override string ToString()
        {
            return $"ParentSegments={ParentSegments.JoinPath()}, CurrentSegments={CurrentSegments.JoinPath()}";
        }
    }
}