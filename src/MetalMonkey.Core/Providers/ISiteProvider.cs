using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Core.Providers
{
    public interface ISiteProvider
    {
        IEnumerable<ISectionProvider> Sections { get; }
    }
}
