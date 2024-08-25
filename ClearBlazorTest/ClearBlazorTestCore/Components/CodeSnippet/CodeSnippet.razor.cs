using ClearBlazor;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace ClearBlazorTest
{
    public partial class CodeSnippet:IBackground
    {

        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public string? Name { get; set; } = null;

        [Parameter]
        public Color? BackgroundColour { get; set; } = Color.Transparent;

        [Parameter]
        public string Code { get; set; } = string.Empty;

        [Parameter]
        public string HighLight { get; set; } = string.Empty;

        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public string Description { get; set; } = string.Empty;

        public bool ShowCode { get; set; } = false;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (string.IsNullOrEmpty(Code))
                ShowCode = false;
            else
                ShowCode = true;
        }
        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }

        RenderFragment CodeComponent(string code) => builder =>
        {
            try
            {
                var names = typeof(CodeSnippet).Assembly.GetManifestResourceNames();
                var key = typeof(CodeSnippet).Assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains($".{code}Code.html"));
                using (var stream = typeof(CodeSnippet).Assembly.GetManifestResourceStream(key))
                using (var reader = new StreamReader(stream))
                {
                    var read = reader.ReadToEnd();

                    if (!string.IsNullOrEmpty(HighLight))
                    {
                        if (HighLight.Contains(","))
                        {
                            var highlights = HighLight.Split(",");

                            foreach (var value in highlights)
                            {
                                read = Regex.Replace(read, $"{value}(?=\\s|\")", $"<mark>$&</mark>");
                            }
                        }
                        else
                        {
                            read = Regex.Replace(read, $"{HighLight}(?=\\s|\")", $"<mark>$&</mark>");
                        }
                    }

                    builder.AddMarkupContent(0, read);
                }
            }
            catch (Exception)
            {
                // todo: log this
            }
        };


    }
}