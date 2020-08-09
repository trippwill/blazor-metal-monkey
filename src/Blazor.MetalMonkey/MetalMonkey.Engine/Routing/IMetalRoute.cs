using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public interface IMetalRoute : IComponent
    {
        bool HandledRoute { get; }

        void RegisterChildRoute(IMetalRoute route);

        void Reset();
    }
}