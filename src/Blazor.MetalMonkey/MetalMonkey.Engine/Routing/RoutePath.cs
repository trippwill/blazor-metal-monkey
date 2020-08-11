using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public class RoutePath : IComponent
    {
        private List<string> _segments = Utilities.EmptyStringList;

        [Parameter] public string Segments { get; set; } = string.Empty;
        
        [Parameter] public uint Rank { get; set;  }

        [Parameter] public RenderFragment<MetalRouteContext> ChildContent { get; set; } = _ => Utilities.EmptyRenderFragment;

        [CascadingParameter] public RouteTable? RouteTable { get; set; }

        public void Attach(RenderHandle _)
        {
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            _segments = Segments.SplitPath().ToList();

            Assumes.Present(RouteTable);
            if (RouteTable.RouteContext.CurrentSegments.SequenceStartsWith(
                _segments,
                StringComparer.OrdinalIgnoreCase))
            {
                RouteTable.Add(this);
            }

            return Task.CompletedTask;
        }

        public RenderFragment GetRenderFragment(MetalRouteContext currentContext) =>
            ChildContent(currentContext.MoveCapturedToParent(_segments));
    }
}
