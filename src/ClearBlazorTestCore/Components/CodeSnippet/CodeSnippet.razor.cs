using ClearBlazor;
using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;

namespace ClearBlazorTest
{
    public partial class CodeSnippet: ClearComponentBase, IBackground
    {

        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = Color.Transparent;

        [Parameter]
        public string Code { get; set; } = string.Empty;

        [Parameter]
        public string HighLight { get; set; } = string.Empty;

        [Parameter]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public string Description { get; set; } = string.Empty;

        public bool ShowCode { get; set; } = false;

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }

        protected string GetBorderThickness()
        {
            if (ShowCode)
                return "1,1,0,1";
            else
                return "1,1,1,1";
        }

        protected string GetCornerRadius()
        {
            if (ShowCode)
                return "8,8,0,0";
            else
                return "8,8,8,8";
        }

        RenderFragment CodeComponent(string code) => builder =>
        {
            try
            {
                var names = typeof(CodeSnippet).Assembly.GetManifestResourceNames();
                var key = typeof(CodeSnippet).Assembly.GetManifestResourceNames().FirstOrDefault(x => x.Contains($".{code}Code.html"));
                if (key == null)
                    return;
                using (var stream = typeof(CodeSnippet).Assembly.GetManifestResourceStream(key))
                {
                    if (stream == null)
                        return;
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
            }
            catch (Exception)
            {
                // todo: log this
            }
        };

        void ShowTheCode()
        {
            ShowCode = true;
            StateHasChanged();
        }
        void HideTheCode()
        {
            ShowCode = false;
            StateHasChanged();
        }

    }
}