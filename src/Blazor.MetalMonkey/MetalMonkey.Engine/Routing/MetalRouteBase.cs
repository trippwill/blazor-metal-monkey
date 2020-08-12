using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace MetalMonkey.Engine.Routing
{
    public abstract class MetalRouteBase : IComponent, IDisposable
    {
        internal IList<IRenderContainer> RenderContainers { get; } = new List<IRenderContainer>();

        internal void AddRenderContainer(IRenderContainer renderContainer)
        {
            RenderContainers.Add(renderContainer);
        }

        public abstract void Attach(RenderHandle renderHandle);

        public abstract Task SetParametersAsync(ParameterView parameters);

        public virtual void Dispose()
        {
            RenderContainers.Clear();
        }
    }
}
