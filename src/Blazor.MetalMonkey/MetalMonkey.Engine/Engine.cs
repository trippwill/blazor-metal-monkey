// Copyright (c) Charles Willis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using Blazor.Extensions.Logging;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine
{
    public static class Engine
    {
        public static WebAssemblyHostBuilder Initialize(string[] args, LogLevel logLevel = LogLevel.Warning)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            builder.Services.AddLogging(builder => builder
                .AddBrowserConsole()
                .SetMinimumLevel(logLevel));

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            return builder;
        }
    }
}
