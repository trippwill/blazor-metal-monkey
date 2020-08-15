using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MetalMonkey.Engine.Interop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine.Routing
{
    public class RoutingManager : IDisposable
    {
        private readonly NavigationManager _navigationManager;
        private readonly EngineInterop _engineInterop;
        private readonly ILogger<RoutingManager> _logger;

        public RoutingManager(NavigationManager navigationManager, EngineInterop engineInterop, ILogger<RoutingManager> logger)
        {
            _navigationManager = navigationManager;
            _engineInterop = engineInterop;
            _logger = logger;

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
            var newLocation = $"{BaseUri}{Path.Join(relativeSegments)}";
            _engineInterop.PushLocation(newLocation);
            _logger.LogInformation("Pushed location update '{0}'", newLocation);
        }

        public void UpdateHistoryLocation() => UpdateHistoryLocation(CurrentTopRouteContext.AllSegments.ToArray());

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
