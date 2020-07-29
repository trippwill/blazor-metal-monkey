using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Core.Providers
{
    public interface ISectionProvider
    {
        string Name { get; }

        ITemplateProvider Template { get; }
    }
}
