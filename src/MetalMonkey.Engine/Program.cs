using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MetalMonkey.Core.Interop;
using Blazor.Extensions.Logging;
using MetalMonkey.Core.Providers;
using MetalMonkey.Site;
using System.Linq;
using MetalMonkey.Core;
using Microsoft;

namespace MetalMonkey.Engine
{
    public delegate ISiteProvider ResolveSite(string siteName);

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Logging.SetMinimumLevel(LogLevel.Trace);
            builder.Logging.AddBrowserConsole();

            builder.Services.AddSingleton<ResourceLoader>();
            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            var siteTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(CoreExtensions.GetTypesWithInterface<ISiteProvider>);

            builder.Services.AddSingleton<ResolveSite>(builder =>
            {
                var logger = builder.GetService<ILogger<Program>>();
                Assumes.Present(logger);

                logger.LogInformation(">>>===<<<: {0}", string.Join("|", AppDomain.CurrentDomain.GetAssemblies().Select(assm => assm.FullName)));
                logger.LogInformation("<<<===>>>: {0}", string.Join("|", siteTypes.Select(t => t.Name)));

                return siteName => (ISiteProvider)Activator.CreateInstance(siteTypes.First());
                //.Single(t => t.Name.CaseInsensitiveEquals(siteName)));
            });

            await builder.Build().RunAsync();
        }
    }
}
