using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Markdig;
using MetalMonkey.Engine.Components;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace MetalMonkey.Engine.Routing
{
    public class PageRouter : ComponentBase, IMetalRoutable
    {
        private string? _content;
        private bool _shouldRender;

        [Parameter] public MetalRouteContext RouteContext { get; set; }

        [Parameter] public string Root { get; set; } = string.Empty;

        [Parameter] public RenderFragment<PageContext>? ChildContent { get; set; }

        [Inject] private HttpClient? HttpClient { get; set; }

        [Inject] private MarkdownPipeline? MarkdownPipeline { get; set; }

        [Inject] private ILogger<PageRouter>? Logger { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            await base.SetParametersAsync(parameters);
            Verify.Operation(ChildContent is not null, $"{nameof(PageRouter)} requires a renderable child component");
        }

        protected override async Task OnInitializedAsync()
        {
            var req = Path.Join(Root, Path.ChangeExtension(RouteContext.CurrentSegments.JoinPath(), "md"));
            Logger.LogInformation("Requesting {req}", req);

            Assumes.Present(HttpClient);
            var response = await HttpClient.GetAsync(req);
            if (response.IsSuccessStatusCode)
            {
                _content = await response.Content.ReadAsStringAsync();
                _shouldRender = true;
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_shouldRender)
            {
                Assumes.NotNull(_content);

                PageContext pageContext = new PageContext(FrontMatter.Default, builder =>
                {
                    builder.AddMarkupContent(0, Markdown.ToHtml(_content, MarkdownPipeline));
                });

                builder.AddContent(0, ChildContent, pageContext);
            }
        }
    }
}
