using ClearBlazor;
using ClearBlazor.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsApiPage: ClearComponentBase
    {
        [Parameter]
        public IComponentDocsInfo? DocsInfo { get; set; }

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }

        private readonly List<string> InheritExclusions = new List<string>() { "ComponentBase"};
        private readonly List<string> ImplementsExclusions = new List<string>() 
            { "IDisposable", "IHandleEvent", "IObserver<BrowserSizeInfo>"  };

        private MarkupString GetExamplesLink()
        {
            return new MarkupString($"<a href={(DocsInfo == null ? string.Empty : DocsInfo.ExamplesLink.Item2)}>" +
                $"{(DocsInfo == null ? string.Empty : DocsInfo.ExamplesLink.Item1)}</a>");
        }

        private MarkupString GetInheritLink()
        {
            if (DocsInfo == null || DocsInfo?.InheritsLink.Item1 == string.Empty)
                return new MarkupString(string.Empty);

            if (InheritExclusions.Contains(DocsInfo!.InheritsLink.Item1))
                return new MarkupString($"Inherits from: {DocsInfo!.InheritsLink.Item1.Trim()}");
            else
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
                if (ImplementsExclusions.Contains(implement.Item1.Trim()))
                    implementsString += $"{implement.Item1.Trim()}  ";
                else
                    implementsString += $"<a href ={implement.Item2}> {implement.Item1}</a>  ";
            }

            return new MarkupString(implementsString);
        }

    }
}