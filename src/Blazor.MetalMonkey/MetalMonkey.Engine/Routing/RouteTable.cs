using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public class RouteTable : IComponent
    {
        private List<RoutePath> _routePaths = new List<RoutePath>();
        private RenderHandle _renderHandle;

        [Parameter] public RenderFragment ChildContent { get; set; } = Utilities.EmptyRenderFragment;

        //[Parameter] public MetalRouteContext RouteContext { get; set; }

        [CascadingParameter] public MetalRouter? MetalRouter { get; set; }

        public void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            Assumes.NotNull(MetalRouter);
            MetalRouter.SetRouteTable(this);

            _renderHandle.Render(builder =>
            {
                builder.OpenComponent<CascadingValue<RouteTable>>(0);
                builder.AddAttribute(1, nameof(CascadingValue<RouteTable>.Value), this);
                builder.AddAttribute(2, nameof(CascadingValue<RouteTable>.ChildContent), ChildContent);
                builder.CloseComponent();
            });

            return Task.CompletedTask;
        }

        internal void Add(RoutePath routePath) => _routePaths.Add(routePath);

        internal RenderFragment? GetRenderFragment() => _routePaths
            .OrderBy(rp => rp.Rank)
            .FirstOrDefault()?
            .GetRenderFragment(RouteContext);
    }
}
