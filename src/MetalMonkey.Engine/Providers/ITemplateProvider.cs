using MetalMonkey.Core.Exceptions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MetalMonkey.Engine.Providers
{
    public delegate Type LayoutResolver(IEnumerable<string> route);

    public interface ITemplateProvider
    {
        IReadOnlyDictionary<string, string> Meta { get; }

        IReadOnlyList<string> Stylesheets { get; }

        LayoutResolver ResolveLayout { get; }
    }

    public readonly struct EmptyTemplate : ITemplateProvider
    {
        public LayoutResolver ResolveLayout => route => throw new EmptyLayoutException(route);

        public IReadOnlyDictionary<string, string> Meta { get; }

        public IReadOnlyList<string> Stylesheets { get; }
    }


}
