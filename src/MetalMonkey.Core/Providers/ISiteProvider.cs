using MetalMonkey.Core.Providers.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Core.Providers
{
    interface ISiteProvider : IProvideScriptAssets
    {
        IEnumerable<ISectionProvider> Sections { get; }
    }
}
