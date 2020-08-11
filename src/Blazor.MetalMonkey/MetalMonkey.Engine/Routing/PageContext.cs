using MetalMonkey.Engine.Components;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public readonly struct PageContext
    {
        public PageContext(FrontMatter frontMatter, RenderFragment content)
        {
            FrontMatter = frontMatter;
            Content = content;
        }

        public FrontMatter FrontMatter { get; }

        public RenderFragment Content { get; }
    }
}
