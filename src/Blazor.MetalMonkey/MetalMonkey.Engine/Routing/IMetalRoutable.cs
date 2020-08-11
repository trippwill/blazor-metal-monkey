using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public interface IMetalRoutable : IComponent
    {
        MetalRouteContext RouteContext { get; set; }
    }
}
