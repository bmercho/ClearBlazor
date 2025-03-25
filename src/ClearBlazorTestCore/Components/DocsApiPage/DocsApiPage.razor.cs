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

        private string GetExamplesHRef()
        {
            return DocsInfo == null ? string.Empty : DocsInfo.ExamplesLink.Item2;
        }
        private string GetExamplesLinkName()
        {
            return DocsInfo == null ? string.Empty : DocsInfo.ExamplesLink.Item1;
        }

        private bool ShowInheritsAsLink()
        {
            if (DocsInfo == null || DocsInfo?.InheritsLink.Item1 == string.Empty)
                return false;

            if (InheritExclusions.Contains(DocsInfo!.InheritsLink.Item1))
                return false;

            return true;
        }

        private string GetInheritHRef()
        {
            return DocsInfo == null ? string.Empty : DocsInfo.InheritsLink.Item2;
        }

        private string GetInheritLinkName()
        {
            if (DocsInfo == null || DocsInfo?.InheritsLink.Item1 == string.Empty)
                return string.Empty;

            return $"  {DocsInfo!.InheritsLink.Item1.Trim()}";
        }

        private string @GetImplementsHRef()
        {
            return "";
        }
        private string @GetImplementsLinkName()
        {
            if (DocsInfo == null || DocsInfo.ImplementsLinks == null || DocsInfo.ImplementsLinks.Count == 0)
                return string.Empty;
            return "";
        }   
        
        private bool HasImplementLink()
        {
            if (DocsInfo == null || DocsInfo.ImplementsLinks == null || DocsInfo.ImplementsLinks.Count == 0)
                return false;
            return true;
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
        private MarkupString GetMarkupString(string value)
        {
            return new MarkupString(value);
        }

    }
}