using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace MetalMonkey.Engine.Interop
{
    public class EngineInterop
    {
        private readonly IJSRuntime _runtime;

        public EngineInterop(IJSRuntime runtime)
        {
            _runtime = runtime;
        }

        public ValueTask PushLocationAsync(string location) => _runtime.InvokeVoidAsync("MetalMonkeyEngine.pushLocation", location);

        public void PushLocation(string location) => (_runtime as IJSInProcessRuntime)!.InvokeVoid("MetalMonkeyEngine.pushLocation", location);
    }
}
