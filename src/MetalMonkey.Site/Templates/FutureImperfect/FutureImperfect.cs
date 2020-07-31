using MetalMonkey.Engine.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Site.Templates.FutureImperfect
{
    public readonly struct FutureImperfect : ITemplateProvider
    {
        public IReadOnlyDictionary<string, string> Meta { get; }
        public IReadOnlyList<string> Stylesheets { get; }
        public LayoutResolver ResolveLayout { get; }
    }
}
