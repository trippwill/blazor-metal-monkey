using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Core.Providers
{
    public interface ITemplateProvider
    {
        Type IndexLayoutType { get; }

        Type PageLayoutType { get; }
    }
}
