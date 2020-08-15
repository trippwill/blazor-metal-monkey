using System;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace MetalMonkey.Engine.Render
{
    public class ComponentView : ComponentBase
    {
        [Parameter] public Type? Component { get; set; }

        [Parameter] public RenderFragment? ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Assumes.Present(Component);

            builder.OpenRegion(0);
            builder.OpenComponent(1, Component);
            builder.AddAttribute(2, "ChildContent", ChildContent);
            builder.CloseComponent();
            builder.CloseRegion();
        }
    }
}
