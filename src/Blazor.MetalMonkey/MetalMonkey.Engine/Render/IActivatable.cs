using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Render
{
    public interface IActivatable
    {
        public string? Name { get; }

        public RenderFragment? Heading { get; }

        public void Activate();
    }
}
