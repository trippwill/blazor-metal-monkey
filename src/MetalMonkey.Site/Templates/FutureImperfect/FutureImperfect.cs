using MetalMonkey.Core.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Site.Templates.FutureImperfect
{
    public class FutureImperfect : ITemplateProvider
    {
        public IEnumerable<Uri> ScriptAssets { get; }
    }
}
