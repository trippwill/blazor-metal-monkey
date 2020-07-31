using System;
using System.Collections.Generic;
using System.Text;

namespace MetalMonkey.Engine.Providers
{
    public delegate ITemplateProvider RouteHandler(IEnumerable<string> route);

    public interface ISiteProvider
    {
        public RouteHandler HandleRoute { get; }
    }
}
