using ClearBlazor;
using ClearBlazor.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsExamplesPage: ClearComponentBase
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

        private string GetLinkName()
        {
            return DocsInfo == null ? string.Empty : DocsInfo.ApiLink.Item1;
        }

        private string GetHRef()
        {
            return DocsInfo == null ? string.Empty : DocsInfo.ApiLink.Item2;
        }
    }
}