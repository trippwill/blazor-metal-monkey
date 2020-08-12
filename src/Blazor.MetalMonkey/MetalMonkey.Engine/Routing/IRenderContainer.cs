using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    internal interface IRenderContainer
    {
        public uint Rank { get; set; }

        internal RenderFragment? GetRenderFragment();
    }
}