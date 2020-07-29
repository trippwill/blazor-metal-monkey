using MetalMonkey.Core.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Site.Templates.FutureImperfect
{
    public readonly struct FutureImperfect : ITemplateProvider
    {
        public Type IndexLayoutType => typeof(MainLayout);

        public Type PageLayoutType => typeof(Single);
    }
}
