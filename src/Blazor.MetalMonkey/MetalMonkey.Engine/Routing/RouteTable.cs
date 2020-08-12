using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public class RouteTable : MetalRouteBase, IRenderContainer
    {
        private RenderHandle _renderHandle;

        [Parameter] public uint Rank { get; set; } = 50;

        [Parameter] public RenderFragment ChildContent { get; set; } = Utilities.EmptyRenderFragment;

        [CascadingParameter] public MetalRouter? MetalRouter { get; set; }

        public override void Attach(RenderHandle renderHandle)
        {
            _renderHandle = renderHandle;
        }

        public override Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            Assumes.NotNull(MetalRouter);

            MetalRouter.AddRenderContainer(this);

            _renderHandle.Render(builder =>
            {
                builder.OpenComponent<CascadingValue<RouteTable>>(0);
                builder.AddAttribute(1, nameof(CascadingValue<RouteTable>.Value), this);
                builder.AddAttribute(2, nameof(CascadingValue<RouteTable>.ChildContent), ChildContent);
                builder.CloseComponent();
            });

            return Task.CompletedTask;
        }

        RenderFragment? IRenderContainer.GetRenderFragment() => RenderContainers
            .OrderBy(rc => rc.Rank)
            .FirstOrDefault()?
            .GetRenderFragment();
    }
}
