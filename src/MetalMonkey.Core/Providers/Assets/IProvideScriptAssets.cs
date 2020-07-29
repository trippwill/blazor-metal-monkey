using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Core.Providers.Assets
{
    public interface IProvideScriptAssets
    {
        IEnumerable<Uri> ScriptAssets { get; }
    }
}
