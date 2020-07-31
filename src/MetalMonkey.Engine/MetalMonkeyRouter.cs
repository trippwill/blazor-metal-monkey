using MetalMonkey.Engine.Routing;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace MetalMonkey.Engine
{
    public class MetalMonkeyRouter : IComponent, IHandleAfterRender, IDisposable
    {
        private readonly List<MetalRouteBase> _routers = new List<MetalRouteBase>();
        private RenderHandle _renderHandle;
        private string? _locationAbsolute;
        private string? _baseUri;
        private bool _navigationInterceptionEnabled;
        private RouteTable _routeTable;

        [Inject] private NavigationManager? NavigationManager { get; set; }

        [Inject] private INavigationInterception? NavigationInterception { get; set; }

        [Inject] private ILogger<MetalMonkeyRouter>? _logger { get; set; }

        [Parameter] public RenderFragment? ChildContent { get; set; }

        public void Attach(RenderHandle renderHandle)
        {
            Assumes.Present(NavigationManager);

            _renderHandle = renderHandle;
            _baseUri = NavigationManager.BaseUri;
            _locationAbsolute = NavigationManager.Uri;
            NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        }

        public Task OnAfterRenderAsync()
        {
            Assumes.Present(NavigationInterception);

            if (!_navigationInterceptionEnabled)
            {
                _navigationInterceptionEnabled = true;
                return NavigationInterception.EnableNavigationInterceptionAsync();
            }

            return Task.CompletedTask;
        }

        public Task SetParametersAsync(ParameterView parameters)
        {
            parameters.SetParameterProperties(this);
            _logger.LogCritical("ROUTE TABLE: {0}", ChildContent.GetType());
            Refresh(isNavigationIntercepted: false);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Assumes.Present(NavigationManager);
            NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
        }

        internal void AddRouter(MetalRouteBase router)
        {
            Requires.NotNull(router, nameof(router));
            _routers.Add(router);
            this.Refresh(isNavigationIntercepted: false);
        }

        private void NavigationManager_LocationChanged(object sender, LocationChangedEventArgs e)
        {
            _locationAbsolute = e.Location;
            if (_renderHandle.IsInitialized && ChildContent != null)
            {
                Refresh(e.IsNavigationIntercepted);
            }
        }

        private void Refresh(bool isNavigationIntercepted)
        {
            Assumes.Present(NavigationManager);
            Assumes.NotNull(_logger);
            Assumes.NotNull(_locationAbsolute);
            Assumes.NotNull(_baseUri);

            this._routers.ForEach(router => router.Deactivate());

            var path = NavigationManager.ToBaseRelativePath(_locationAbsolute);
            var context = new MetalRouteContext(path);

            var activeRouter = _routers
                .OrderByDescending(r => r.Priority)
                .FirstOrDefault(router => router.TryActivate(context));

            Assumes.Present(ChildContent);
            _renderHandle.Render(builder =>
            {
                builder.OpenComponent<RouteTable>(0);
                builder.AddAttribute(1, "RouteContext", context);
                builder.AddAttribute(2, "ChildContent", ChildContent);
                builder.AddComponentReferenceCapture(3, component => _routeTable = (RouteTable)component);
                builder.CloseComponent();
            });

            //Assumes.NotNull(_routeTable);
            if (!_routeTable?.HasActiveRoute ?? false)
            {
                if (!isNavigationIntercepted)
                {
                    throw new NavigationException(NavigationManager.Uri);
                }

                Log.NavigatingToExternalUri(_logger, _locationAbsolute, path, _baseUri, null);
                NavigationManager.NavigateTo(_locationAbsolute, forceLoad: true);
            }
        }

        private static class Log
        {
            public static Action<ILogger, Type, string, Exception?> NavigatingToRouter =
                LoggerMessage.Define<Type, string>(LogLevel.Debug, new EventId(1, nameof(NavigatingToRouter)), "Navigating to router {RouterType} for path {Path}");

            public static readonly Action<ILogger, string, string, string, Exception?> NavigatingToExternalUri =
                LoggerMessage.Define<string, string, string>(LogLevel.Debug, new EventId(3, nameof(NavigatingToExternalUri)), "Navigating to non-component URI '{ExternalUri}' in response to path '{Path}' with base URI '{BaseUri}'");
        }
    }
}
