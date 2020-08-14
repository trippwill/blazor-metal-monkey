using System;
using System.IO;
using MetalMonkey.Engine.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MetalMonkey.Engine.Routing
{
    public class RoutingManager : IDisposable
    {
        private readonly NavigationManager _navigationManager;
        private readonly EngineInterop _engineInterop;

        public RoutingManager(NavigationManager navigationManager, EngineInterop engineInterop)
        {
            _navigationManager = navigationManager;
            _engineInterop = engineInterop;

            _navigationManager.LocationChanged += _navigationManager_LocationChanged;
            BaseUri = _navigationManager.BaseUri;
            AbsoluteUri = _navigationManager.Uri;
            BaseRelativePath = _navigationManager.ToBaseRelativePath(AbsoluteUri);
            CurrentTopRouteContext = MetalRouteContext.FromBaseRelativePath(BaseRelativePath);
        }

        public string BaseUri { get; }

        public string AbsoluteUri { get; private set; }
        
        public string BaseRelativePath { get; private set; }

        public bool IsNavigationIntercepted { get; private set; }

        public MetalRouteContext CurrentTopRouteContext { get; private set; }

        public void UpdateHistoryLocation(params string?[] relativeSegments)
        {
            var newLocation = Path.Join(relativeSegments);
            _engineInterop.PushLocation($"{BaseUri}{newLocation}");
        }

        public void Dispose()
        {
            _navigationManager.LocationChanged -= _navigationManager_LocationChanged;
        }

        private void _navigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
        {
            IsNavigationIntercepted = e.IsNavigationIntercepted;
            AbsoluteUri = e.Location;
            BaseRelativePath = _navigationManager.ToBaseRelativePath(AbsoluteUri);
            CurrentTopRouteContext = MetalRouteContext.FromBaseRelativePath(BaseRelativePath);
        }

    }
}
