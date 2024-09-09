using ClearBlazor;
using ClearBlazor.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsEnumPage:ClearComponentBase
    {
        [Parameter]
        public IOtherDocsInfo? DocsInfo { get; set; }

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }
    }
}