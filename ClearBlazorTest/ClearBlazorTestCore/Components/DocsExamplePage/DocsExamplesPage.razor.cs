using ClearBlazor;
using ClearBlazor.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsExamplesPage: ClearComponentBase,IContent
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public IComponentDocsInfo? DocsInfo { get; set; }


        protected override async  Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            var filename = Directory.GetCurrentDirectory(); 
        }


        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; "; 
        }

        private MarkupString GetApiLink()
        {
            return new MarkupString($"<a href={(DocsInfo == null ? string.Empty : DocsInfo.ApiLink.Item2)}>{DocsInfo.ApiLink.Item1}</a>");
        }

    }
}