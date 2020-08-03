using System;
using System.Collections.Generic;
using System.Linq;

namespace MetalMonkey.Engine.Routing
{
    public readonly struct MetalRouteContext
    {
        internal MetalRouteContext(MetalRouter router, IEnumerable<string> parentSegments, IEnumerable<string> currentSegments)
        {
            MetalRouter = router;
            ParentSegments = parentSegments;
            CurrentSegments = currentSegments;
        }

        public IEnumerable<string> ParentSegments {get;}

        public IEnumerable<string> CurrentSegments {get;}

        public MetalRouter MetalRouter { get; }

        internal static MetalRouteContext FromBaseRelativePath(MetalRouter router, string relativePath)
        {
            return new MetalRouteContext(
                router,
                Enumerable.Empty<string>(),
                relativePath.Split('/')
            );
        }

        internal MetalRouteContext MoveCapturedToParent(IEnumerable<string> capturedSegments)
        {
            var currentCapturedSegments = CurrentSegments.TakeWhile((cseg, idx) => capturedSegments.ElementAtOrDefault(idx).EqualsIgnoreCase(cseg));
            if (currentCapturedSegments is null || !currentCapturedSegments.Any())
            {
                return this;
            }

            return new MetalRouteContext(
                MetalRouter,
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