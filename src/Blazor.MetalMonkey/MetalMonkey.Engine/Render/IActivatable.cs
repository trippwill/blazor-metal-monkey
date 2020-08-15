using MetalMonkey.Engine.Routing;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Render
{
    public interface IActivatable
    {
        public string? Id { get; }

        public string? Name { get; }

        public RenderFragment? Heading { get; }

        public void Activate();

        public RenderFragment? GetContent(MetalRouteContext routeContext);
    }
}
