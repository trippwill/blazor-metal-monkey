using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine.Routing
{
    public class SegmentRoute : ComponentBase, IMetalRoute
    {
        private IEnumerable<string> _segmentCollection = Enumerable.Empty<string>();

        [Inject] private ILogger<SegmentRoute>? Logger { get; set; }

        [Parameter] public string Segments { get; set; } = string.Empty;

        [Parameter] public MetalRouteContext RouteContext { get; set; }

        [Parameter] public RenderFragment<MetalRouteContext> ChildContent { get; set; } = _ => Utilities.EmptyRenderFragment;

        [Parameter] public Type Layout { get; set; }

        public bool HandledRoute { get; private set; }

        public bool CanHandleRoute(MetalRouteContext context)
        {
            int segmentCount = _segmentCollection.Count();
            Logger.LogDebug($"[{Segments}]: Count={segmentCount}");

            IEnumerable<string> capturedSegments = RouteContext.CurrentSegments.Take(segmentCount);
            Logger.LogDebug($"[{Segments}]: Captured Segments: {capturedSegments.JoinPath()}");

            return capturedSegments.SequenceEqual(_segmentCollection);
        }

        public void Reset() => HandledRoute = false;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            _segmentCollection = Segments.Split('/', System.StringSplitOptions.TrimEntries | System.StringSplitOptions.RemoveEmptyEntries);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Assumes.Present(RouteContext);
            RouteContext.MetalRouter.RegisterRoute(this);
        }

        protected override void BuildRenderTree(Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
        {
            if (!CanHandleRoute(RouteContext))
            {
                Logger.LogInformation($"[{Segments}]: No match. Bypassing route");
                return;
            }

            MetalRouteContext context = RouteContext.MoveCapturedToParent(_segmentCollection);
            Logger.LogInformation($"[{Segments}]: Match. Handling route");
            Logger.LogInformation($"[{Segments}]: New Context: {context}");
            builder.OpenComponent<LayoutView>(0);
            builder.AddAttribute(1, "Layout", Layout);
            builder.AddContent(2, ChildContent, context);
            builder.CloseComponent();
            HandledRoute = true;
        }
    }
}