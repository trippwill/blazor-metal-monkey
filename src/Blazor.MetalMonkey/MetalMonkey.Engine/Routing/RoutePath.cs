using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public class RoutePath : IComponent, IRenderContainer
    {
        private List<string> _segments = Utilities.EmptyStringList;

        [Parameter] public string Segments { get; set; } = string.Empty;
        
        [Parameter] public uint Rank { get; set;  }

        [Parameter] public RenderFragment<MetalRouteContext> ChildContent { get; set; } = _ => Utilities.EmptyRenderFragment;

        [CascadingParameter] public RouteTable? RouteTable { get; set; }

        [CascadingParameter] public MetalRouteContext RouteContext { get; set; }

        public void Attach(RenderHandle _)
        {
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            _segments = Segments.SplitPath().ToList();

            Assumes.Present(RouteTable);
            if (RouteContext.CurrentSegments.SequenceStartsWith(
                _segments,
                StringComparer.OrdinalIgnoreCase))
            {
                RouteTable.AddRenderContainer(this);
            }

            return Task.CompletedTask;
        }

        RenderFragment? IRenderContainer.GetRenderFragment() =>
            ChildContent(RouteContext.CaptureSegments(_segments));
    }
}
