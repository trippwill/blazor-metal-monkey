// Copyright (c) Charles Willis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine.Routing
{
    public class MetalRouter : IMetalRoute, IHandleAfterRender, IDisposable
    {
        private const string _invalidParameterFormat = "The MetalRouter component requires a value for the parameter {0}";

        private RenderHandle _renderHandle;
        private string? _baseUri;
        private string? _locationAbsolute;
        private bool _navigationInterceptionEnabled;
        private List<IMetalRoute> _registeredRoutes = new List<IMetalRoute>();

        [Parameter] public RenderFragment<MetalRouteContext>? Routes { get; set; }

        [Parameter] public RenderFragment? Unhandled { get; set; }

        [Inject] private NavigationManager? NavigationManager { get; set; }

        [Inject] private INavigationInterception? NavigationInterception { get; set; }

        [Inject] private ILogger<MetalRouter>? Logger { get; set; }

        public bool HandledRoute => _registeredRoutes.Any(imr => imr.HandledRoute);

        public void Reset() => _registeredRoutes.ForEach(imr => imr.Reset());

        public void Attach(RenderHandle renderHandle)
        {
            Assumes.Present(NavigationManager);

            _renderHandle = renderHandle;
            _baseUri = NavigationManager.BaseUri;
            _locationAbsolute = NavigationManager.Uri;
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);

            Verify.Operation(Routes is not null, string.Format(_invalidParameterFormat, nameof(Routes)));

            Assumes.Present(NavigationManager);
            Assumes.NotNull(_locationAbsolute);

            Refresh(isNavigationIntercepted: false);
            return Task.CompletedTask;
        }

        public Task OnAfterRenderAsync()
        {
            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;

                Assumes.Present(NavigationInterception);
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Assumes.Present(NavigationManager);

            NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }

        public void RegisterChildRoute(IMetalRoute route)
        {
            Logger.LogDebug("{MetalRouter} registering child route for segments '{Segments}'", nameof(MetalRouter), (route as SegmentRoute)?.Segments);
            _registeredRoutes.Add(route);
            //Refresh(_navigationInterceptionEnabled, skipUnhandled: true);
        }

        internal void Refresh(bool isNavigationIntercepted, bool skipUnhandled = false)
        {
            Assumes.Present(NavigationManager);
            Assumes.NotNull(_locationAbsolute);
            var locationPath = NavigationManager.ToBaseRelativePath(_locationAbsolute);
            var context = MetalRouteContext.FromBaseRelativePath(this, locationPath);

            Logger.LogDebug("Attempting to navigate using defined routes in response to path '{Path}' with base '{Base}' and intercepted '{Intercepted}'", locationPath, _baseUri, isNavigationIntercepted);

            Assumes.NotNull(Routes);
            _renderHandle.Render(Routes(context));

            if (HandledRoute)
            {
                Reset();
                return;
            }

            if (skipUnhandled)
                return;

            OnUnhandled(isNavigationIntercepted, locationPath);
        }

        internal virtual void OnUnhandled(bool isNavigationIntercepted, string locationPath)
        {
            if (!isNavigationIntercepted)
            {
                if (Unhandled is not null)
                {
                    Logger.LogDebug("Displaying Unhandled component in reponse to path '{Path}' with base '{Base}'", locationPath, _baseUri);
                    _renderHandle.Render(Unhandled);
                }
            }
            else
            {
                Assumes.Present(NavigationManager);
                Assumes.Present(_locationAbsolute);

                Logger.LogDebug("Navigating to non - component URI '{ExternalUri}' in response to path '{Path}' with base '{Base}'", _locationAbsolute, locationPath, _baseUri);
                NavigationManager.NavigateTo(_locationAbsolute, forceLoad: true);
            }
        }

        private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
        {
            _locationAbsolute = e.Location;
            if (_renderHandle.IsInitialized && Routes != null)
            {
                Refresh(e.IsNavigationIntercepted);
            }
        }
    }
}