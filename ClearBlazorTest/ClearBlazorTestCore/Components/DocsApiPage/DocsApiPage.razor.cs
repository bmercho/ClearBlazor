using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsApiPage: ClearComponentBase
    {
        [Parameter]
        public IDocsInfo? DocsInfo { get; set; }

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }

        private MarkupString GetExamplesLink()
        {
            return new MarkupString($"<a href={(DocsInfo == null ? string.Empty : DocsInfo.ExamplesLink.Item2)}>" +
                $"{(DocsInfo == null ? string.Empty : DocsInfo.ExamplesLink.Item1)}</a>");
        }

        private MarkupString GetInheritLink()
        {
            return new MarkupString($"Inherits from: <a href={(DocsInfo == null ? string.Empty : DocsInfo.InheritsLink.Item2)}>" +
                                    $" {(DocsInfo == null ? string.Empty : DocsInfo.InheritsLink.Item1)}</a>");
        }

        private MarkupString GetImplementsLink()
        {
            if (DocsInfo == null || DocsInfo.ImplementsLinks == null || DocsInfo.ImplementsLinks.Count == 0)
                return new MarkupString(string.Empty);

            string implementsString = $"Implements: ";
            foreach(var implement in  DocsInfo.ImplementsLinks)
            {
                implementsString += $"<a href ={implement.Item2}> {implement.Item1}</a>  ";
            }

            return new MarkupString(implementsString);
        }

    }
}