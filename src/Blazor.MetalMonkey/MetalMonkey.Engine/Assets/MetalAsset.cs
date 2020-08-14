using System;
using System.Threading.Tasks;
using Microsoft;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace MetalMonkey.Engine.Assets
{
    public class MetalAsset : ComponentBase
    {
        public enum AssetType
        {
            Unknown,
            Script,
            ScriptDefer,
            StyleSheet
        }

        [Parameter] public AssetType Type { get; set; }

        [Parameter] public string Source { get; set; } = string.Empty;

        public override Task SetParametersAsync(ParameterView parameters)
        {
            base.SetParametersAsync(parameters);

            Verify.Operation(Type != AssetType.Unknown, $"The '{nameof(Type)}' attribute must be set to a valid value");
            Verify.Operation(Source.IsNotEmpty(), $"The '{nameof(Source)}' attribute must be set to valid value");

            return Task.CompletedTask;
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            Action<RenderTreeBuilder> operation = Type switch
            {
                AssetType.Script => (builder) => BuildScript(builder, defer: false),
                AssetType.ScriptDefer => (builder) => BuildScript(builder, defer: true),
                AssetType.StyleSheet => (builder) => BuildStylesheet(builder),
                _ => throw new NotImplementedException()
            };

            builder.OpenRegion(0);
            operation(builder);
            builder.CloseRegion();

            void BuildScript(RenderTreeBuilder _builder, bool defer = false)
            {
                _builder.OpenElement(0, "script");
                _builder.AddAttribute(1, "src", Source);
                _builder.AddAttribute(2, "defer", defer);
                _builder.CloseElement();
            }

            void BuildStylesheet(RenderTreeBuilder _builder)
            {
                _builder.OpenElement(0, "link");
                _builder.AddAttribute(1, "rel", "stylesheet");
                _builder.AddAttribute(2, "href", Source);
                _builder.CloseElement();
            }
        }
    }
}
