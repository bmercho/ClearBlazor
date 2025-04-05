using Microsoft.AspNetCore.Components;
using Markdig;

namespace ClearBlazor
{
    public partial class MarkdownViewer : ClearComponentBase
    {
        /// <summary>
        /// Represents a string containing Markdown content. Initialized to an empty string by default.
        /// </summary>
        [Parameter]
        public string MarkdownString { get; set; } = string.Empty;

        string htmlString = string.Empty;
        MarkdownPipeline pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();


        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            var result = Markdown.ToHtml(MarkdownString, pipeline);
            htmlString = Markdown.ToHtml(MarkdownString, pipeline);
        }
    }
}