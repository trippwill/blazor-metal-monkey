using MetalMonkey.Core.Providers.Assets;
using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Core.Providers
{
    interface ISectionProvider : IProvideScriptAssets
    {
        ITemplateProvider Template { get; }
    }
}
