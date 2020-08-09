using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace MetalMonkey.Engine.Interop
{
    public class MetalBody : ComponentBase
    {
        [Inject] private IJSRuntime? JSRuntime { get; set; }

        [Parameter] public string Class { get; set; } = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && Class.IsNotEmpty())
            {
                Assumes.Present(JSRuntime);
                await JSRuntime.InvokeVoidAsync("MetalMonkeyEngine.addClassByTag", "body", 0, Class);
            }
        }
    }
}
