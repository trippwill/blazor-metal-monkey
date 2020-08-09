using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine.Routing
{
    public class SegmentRoute : ComponentBase, IMetalRoute
    {
        private IEnumerable<string> _segmentCollection = Enumerable.Empty<string>();
        private List<IMetalRoute> _registeredRoutes = new List<IMetalRoute>();
        private bool _selfHandledRoute;

        [Inject] private ILogger<SegmentRoute>? Logger { get; set; }

        [Parameter] public string Segments { get; set; } = string.Empty;

        [Parameter] public MetalRouteContext RouteContext { get; set; }

        [Parameter] public RenderFragment<MetalRouteContext> ChildContent { get; set; } = _ => Utilities.EmptyRenderFragment;

        [Parameter] public Type? Layout { get; set; }

        [Parameter] public Type? Index { get; set; }

        public void RegisterChildRoute(IMetalRoute route)
        {
            _registeredRoutes.Add(route);
        }

        public bool HandledRoute => _selfHandledRoute || _registeredRoutes.Any(imr => imr.HandledRoute);

        public void Reset()
        {
            _selfHandledRoute = false;
            _registeredRoutes.ForEach(imr => imr.Reset());
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            if (Layout is not null)
            {
                Verify.Operation(typeof(LayoutComponentBase).IsAssignableFrom(Layout), $"If provided, {nameof(Layout)} must inherit {nameof(LayoutComponentBase)}");
            }

            if (Index is not null)
            {
                Verify.Operation(typeof(IComponent).IsAssignableFrom(Index), $"If provided, {nameof(Index)} must implement {nameof(IComponent)}");
            }

            return Task.CompletedTask;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            _segmentCollection = Segments.SplitPath();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            RouteContext.ParentRoute.RegisterChildRoute(this);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (!CanHandleRoute(RouteContext))
            {
                Logger.LogInformation($"[{Segments}]: No match. Bypassing route");
                return;
            }

            MetalRouteContext nextcontext = RouteContext.MoveCapturedToParent(this, _segmentCollection);
            Logger.LogInformation($"[{Segments}]: Match. Handling route");
            Logger.LogInformation($"[{Segments}]: New Context: {nextcontext}");
            builder.OpenRegion(0);            

            if (Layout is not null)
            {
                builder.OpenComponent<LayoutView>(0);
                builder.AddAttribute(1, "Layout", Layout);
                builder.AddAttribute(2, "ChildContent", ChildContent(nextcontext));
                builder.CloseComponent();
            }
            else
            {
                builder.AddContent(0, ChildContent(nextcontext));
            }

            builder.CloseRegion();
            _selfHandledRoute = true;
        }

        private bool CanHandleRoute(MetalRouteContext context)
        {
            int segmentCount = _segmentCollection.Count();
            Logger.LogDebug($"[{Segments}]: Count={segmentCount}");

            IEnumerable<string> capturedSegments = RouteContext.CurrentSegments.Take(segmentCount);
            Logger.LogDebug($"[{Segments}]: Captured Segments: {capturedSegments.JoinPath()}");

            return capturedSegments.SequenceEqual(_segmentCollection);
        }
    }
}