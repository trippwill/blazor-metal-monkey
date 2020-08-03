namespace MetalMonkey.Engine.Routing
{
    public interface IMetalRoute
    {
        bool HandledRoute { get; }

        bool CanHandleRoute(MetalRouteContext context);
        void Reset();
    }
}