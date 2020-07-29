using MetalMonkey.Core.Providers;
using MetalMonkey.Site.Templates.FutureImperfect;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Site
{
    public readonly struct MetalSite : ISiteProvider
    {
        public IEnumerable<ISectionProvider> Sections  => new List<ISectionProvider>
        {
            new IndexSection(),
            new PageSection(),
        };

        public readonly struct IndexSection : ISectionProvider
        {
            public string Name => string.Empty;

            public ITemplateProvider Template => new FutureImperfect();
        }

        public readonly struct PageSection : ISectionProvider
        {
            public string Name => "Page";

            public ITemplateProvider Template => new AlternativeTemplate();
        }

        public readonly struct AlternativeTemplate : ITemplateProvider
        {
            public Type IndexLayoutType => typeof(SingleLayout);

            public Type PageLayoutType => typeof(SingleLayout);
        }
    }
}
