using MetalMonkey.Engine.Core.Extensions;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetalMonkey.Engine.Routing
{
    public abstract class MetalRouteBase : ComponentBase
    {
        [Inject] private ILogger<MetalRouteBase> Logger { get; set; }

        [CascadingParameter] private RouteTable? Parent { get; set; }

        [Parameter] public string Base { get; set; } = string.Empty;

        [Parameter] public int Priority { get; set; } = 0;

        public bool IsActive { get; private set; }

        public bool TryActivate(MetalRouteContext routeContext)
        {
            Logger.LogInformation("Trying to activate router {RouterType}[{Priority}]({Base}) for path: {Path}", GetType(), Priority, Base, routeContext.Segments.AsRelativePath());
            if (CanHandleRoute(routeContext))
            {
                IsActive = true;
                OnActivated();
            }

            return IsActive;
        }

        public void Deactivate()
        {
            IsActive = false;
        }

        protected override void OnInitialized()
        {
            Logger.LogInformation("Initialized router {RouterType}", GetType());
            Requires.Argument(Parent is RouteTable, nameof(Parent), $"A {nameof(MetalRouteBase)} must exist within a {nameof(RouteTable)}");
            Parent.AddRoute(this);
            base.OnInitialized();
        }

        protected override bool ShouldRender() => IsActive;

        protected virtual bool CanHandleRoute(MetalRouteContext routeContext)
        {
            bool v = routeContext.Segments.StartsWithRelativePath(Base);
            Logger.LogInformation("Base CanHandleRoute {0}", v);
            return Base.IsEmpty() || v;
        }

        protected virtual void OnActivated()
        {
        }

        protected virtual void OnDeactivated()
        {
        }
    }
}
