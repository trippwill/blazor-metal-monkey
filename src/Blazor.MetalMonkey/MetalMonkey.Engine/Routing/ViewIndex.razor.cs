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
    public partial class ViewIndex<TView> where TView : IActivatable
    {
        private IEnumerable<string> _segments = Utilities.EmptyStringList;

        public Section? ActiveSection { get; private set; }

        [Inject] private ILogger<ViewIndex<TView>>? Logger { get; set; }

        [Inject] private RoutingManager? RoutingManager { get; set; }

        private List<Section> SectionStore { get; } = new List<Section>();
        
        internal void Add(Section section)
        {
            SectionStore.Add(section);

            Logger.LogInformation("Added section {Id} with Name {Name}", section.Id, section.Name);

            if (ActiveSection is null && section.Name is not null)
            {
                Logger.LogInformation("ActiveSection is null");

                var currentSection = System.IO.Path.Join(Base, section.Name);
                var sectionSegments = currentSection.SplitPath().ToList();

                Assumes.Present(RoutingManager);
                Logger.LogInformation("SectionSegments='{0}', NavSegments='{1}'", sectionSegments.JoinPath(), RoutingManager.BaseRelativePath);
                
                if (RoutingManager.BaseRelativePath.SplitPath().SequenceStartsWith(sectionSegments, StringComparer.OrdinalIgnoreCase))
                {
                    Logger.LogInformation("Segments are a match! Id='{Id}', Name='{Name}'", section.Id, section.Name);
                    SetActive(section);
                }
            }
        }

        internal void SetActive(Section section)
        {
            if (ActiveSection != section)
            {
                ActiveSection = section;
                StateHasChanged();

                Assumes.Present(RoutingManager);
                RoutingManager.UpdateHistoryLocation(Base, section.Name);
            }
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && ActiveSection is null)
            {
                Logger.LogInformation("No active section. Selecting first section.");
                SectionStore.First().Activate();
            }
        }
    }
}
