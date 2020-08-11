using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    [DebuggerDisplay("{" + nameof(DebuggerDisplay ) + ",nq}")]
    public readonly struct MetalRouteContext
    {
        internal MetalRouteContext(IEnumerable<string> parentSegments, IEnumerable<string> currentSegments)
        {
            ParentSegments = parentSegments;
            CurrentSegments = currentSegments;
        }

        public IEnumerable<string> ParentSegments {get;}

        public IEnumerable<string> CurrentSegments {get;}

        internal static MetalRouteContext FromBaseRelativePath(string relativePath)
        {
            return new MetalRouteContext(
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
                ParentSegments.Concat(currentCapturedSegments),
                CurrentSegments.Skip(currentCapturedSegments.Count())
                );
        }

        public override string ToString()
        {
            return $"ParentSegments={ParentSegments.JoinPath()}, CurrentSegments={CurrentSegments.JoinPath()}";
        }

        private string DebuggerDisplay => ToString();
    }
}