using System.Collections.Generic;
using System.Linq;

namespace MetalMonkey.Engine.Routing
{
    public readonly struct MetalRouteContext
    {
        public static implicit operator string[](MetalRouteContext context) => context.AllSegments.ToArray();

        internal MetalRouteContext(IEnumerable<string> parentSegments, IEnumerable<string> currentSegments)
        {
            CapturedSegments = parentSegments;
            CurrentSegments = currentSegments;
        }

        public IEnumerable<string> CapturedSegments {get;}

        public IEnumerable<string> CurrentSegments {get;}

        public IEnumerable<string> AllSegments => CapturedSegments.Concat(CurrentSegments);

        public override string ToString()
        {
            return $"ParentSegments={CapturedSegments.JoinPath()}, CurrentSegments={CurrentSegments.JoinPath()}";
        }

        internal static MetalRouteContext FromBaseRelativePath(string relativePath)
        {
            return new MetalRouteContext(
                Enumerable.Empty<string>(),
                relativePath.Split('/')
            );
        }

        internal MetalRouteContext CaptureSegments(params string[] capturedSegments)
        {
            var currentCapturedSegments = CurrentSegments.TakeWhile((cseg, idx) => capturedSegments.ElementAtOrDefault(idx).EqualsIgnoreCase(cseg));
            if (currentCapturedSegments is null || !currentCapturedSegments.Any())
            {
                return this;
            }

            return new MetalRouteContext(
                CapturedSegments.Concat(currentCapturedSegments),
                CurrentSegments.Skip(currentCapturedSegments.Count())
                );
        }
    }
}