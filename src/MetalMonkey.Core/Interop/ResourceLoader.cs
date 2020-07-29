using Microsoft;
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
        private const string ResourceLoaderJs = "ResourceLoader.js";
        private const string ResourceLoaderCss = "ResourceLoader.css";

        private readonly IJSInProcessRuntime runtime;
        private readonly ILogger<ResourceLoader> logger;

        public ResourceLoader(IJSRuntime runtime, ILogger<ResourceLoader> logger)
        {
            this.runtime = (IJSInProcessRuntime)runtime;
            this.logger = logger;
        }

        private static void RequireUriIsRelative(Uri uri)
        {
            Requires.Argument(!uri.IsAbsoluteUri, nameof(uri), "The uri must be relative to the wwwroot");
        }

        public ValueTask LoadJsAsync(Uri uri)
        {
            RequireUriIsRelative(uri);

            string args = uri.ToString();

            try
            {
                return this.runtime.InvokeVoidAsync(ResourceLoaderJs, args);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "While loading JS {0} resource", args);
            }

            return new ValueTask();
        }

        public void LoadJs(Uri uri)
        {
            RequireUriIsRelative(uri);

            string args = uri.ToString();

            try
            {
                this.runtime.InvokeVoid(ResourceLoaderJs, args);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "While loading JS {0} resource", args);
            }
        }

        public ValueTask LoadCssAsync(Uri uri)
        {
            RequireUriIsRelative(uri);

            string args = uri.ToString();

            try
            {
                return this.runtime.InvokeVoidAsync(ResourceLoaderCss, args);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "While loading CSS {0} resource", args);
            }

            return new ValueTask();
        }

        public void LoadCss(Uri uri)
        {
            RequireUriIsRelative(uri);

            string args = uri.ToString();

            try
            {
                this.runtime.InvokeVoid(ResourceLoaderCss, args);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "While loading CSS {0} resource", args);
            }
        }
    }
}
