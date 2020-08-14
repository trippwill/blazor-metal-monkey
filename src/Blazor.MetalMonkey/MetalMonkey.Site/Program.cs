// Copyright (c) Charles Willis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace MetalMonkey.Site
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Engine.Engine.Initialize(args, Microsoft.Extensions.Logging.LogLevel.Information);

            builder.RootComponents.Add<App>("app");

            await builder.Build().RunAsync();
        }
    }
}
