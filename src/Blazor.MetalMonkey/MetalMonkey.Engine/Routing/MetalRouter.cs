// Copyright (c) Charles Willis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine.Routing
{
    public class MetalRouter : IComponent, IHandleAfterRender, IDisposable
    {
        private const string _invalidParameterFormat = "The MetalRouter component requires a value for the parameter {0}";

        private RenderHandle _renderHandle;
        private string? _baseUri;
        private string? _locationAbsolute;
        private bool _navigationInterceptionEnabled;

        [Parameter] public RenderFragment<MetalRouteContext> ChildContent { get; set; } = _ => Utilities.EmptyRenderFragment;

        [Inject] private NavigationManager? NavigationManager { get; set; }

        [Inject] private INavigationInterception? NavigationInterception { get; set; }

        [Inject] private ILogger<MetalRouter>? Logger { get; set; }

        private RouteTable? _routeTable;

        public bool CanHandleRoute(MetalRouteContext nextContext) => true;

        public bool Handled { get; set; }

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

        internal void SetRouteTable(RouteTable routeTable) => _routeTable = routeTable;

        internal void Refresh(bool isNavigationIntercepted)
        {
            Assumes.Present(NavigationManager);
            Assumes.NotNull(_locationAbsolute);
            var locationPath = NavigationManager.ToBaseRelativePath(_locationAbsolute);
            var context = MetalRouteContext.FromBaseRelativePath(locationPath);

            Logger.LogDebug("Attempting to navigate using defined routes in response to path '{Path}' with base '{Base}' and intercepted '{Intercepted}'", locationPath, _baseUri, isNavigationIntercepted);

            _renderHandle.Render(builder =>
            {
                builder.OpenComponent<CascadingValue<MetalRouter?>>(0);
                builder.AddAttribute(1, nameof(CascadingValue<MetalRouter?>.Value), this);
                builder.AddAttribute(2, nameof(CascadingValue<MetalRouter?>.ChildContent), ChildContent(context));
                builder.CloseComponent();
            });

            _renderHandle.Render(builder =>
            {
                var frag = _routeTable?.GetRenderFragment();
                if (frag is not null)
                {
                    builder.AddContent(3, frag);
                }
                else
                {
                    OnUnhandled(isNavigationIntercepted, locationPath);
                }
            });
        }

        internal virtual void OnUnhandled(bool isNavigationIntercepted, string locationPath)
        {
            if (!isNavigationIntercepted)
            {
                {
                    Logger.LogDebug("Displaying Unhandled component in reponse to path '{Path}' with base '{Base}'", locationPath, _baseUri);
                    _renderHandle.Render(builder =>
                    {
                        builder.AddMarkupContent(0, "<h1>Nothing Here</h1>");
                    });
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
            if (_renderHandle.IsInitialized)
            {
                Refresh(e.IsNavigationIntercepted);
            }
        }
    }
}