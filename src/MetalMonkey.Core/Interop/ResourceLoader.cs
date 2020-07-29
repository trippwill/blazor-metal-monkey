using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetalMonkey.Core.Interop
{
    public class ResourceLoader
    {
        private readonly IJSRuntime runtime;
        private readonly ILogger<ResourceLoader> logger;

        public ResourceLoader(IJSRuntime runtime, ILogger<ResourceLoader> logger)
        {
            this.runtime = runtime;
            this.logger = logger;
        }

        public ValueTask LoadJs(Uri uri)
        {
            try
            {
                if (uri.IsAbsoluteUri) throw new ArgumentException();
                return this.runtime.InvokeVoidAsync("ResourceLoader.js", uri.ToString());
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "While loading JS {0}", uri.ToString());
            }

            return new ValueTask();
        }

        public ValueTask LoadCss(Uri uri)
        {
            if (uri.IsAbsoluteUri) throw new ArgumentException();
            return this.runtime.InvokeVoidAsync("ResourceLoader.css", uri.ToString());
        }
    }
}
