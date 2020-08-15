using System;
using System.Collections.Generic;
using System.Linq;
using Blazor.Extensions.Logging;
using MetalMonkey.Engine.Render;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine.Routing
{
    public partial class ViewIndex<TView> where TView : class, IActivatable
    {
        private IEnumerable<string> _segments = Utilities.EmptyStringList;

        public TView? ActiveView { get; private set; }

        [Inject] private ILogger<ViewIndex<TView>>? Logger { get; set; }

        [Inject] private RoutingManager? RoutingManager { get; set; }

        //private List<TView> ViewStore { get; } = new List<TView>();

        internal void Add(TView view)
        {
            //ViewStore.Add(view);

            Logger.LogInformation("[{Id}]: Adding view {ViewId} with Name {Name}", Id, view.Id, view.Name);

            if (ActiveView is null && view.Name is not null)
            {
                Logger.LogInformation("[{Id}]: ActiveView is null", Id);

                var currentView = System.IO.Path.Join(Base, view.Name);
                var viewSegments = currentView.SplitPath().ToList();

                Assumes.Present(RoutingManager);
                Logger.LogInformation("[{Id}]: ViewSegments='{0}', NavSegments='{1}'",Id ,viewSegments.JoinPath(), RoutingManager.BaseRelativePath);

                if (RoutingManager.BaseRelativePath.SplitPath().SequenceStartsWith(viewSegments, StringComparer.OrdinalIgnoreCase))
                {
                    Logger.LogInformation("[{Id}]: Segments are a match! Id='{Id}', Name='{Name}'", Id, view.Id, view.Name);
                    SetActive(view);
                }
            }
        }

        internal void SetActive(TView view)
        {
            if (ActiveView != view)
            {
                ActiveView = view;
                StateHasChanged();

                Assumes.Present(RoutingManager);

                //RoutingManager.UpdateHistoryLocation(Base, view.Name);
            }
        }
    }
}
