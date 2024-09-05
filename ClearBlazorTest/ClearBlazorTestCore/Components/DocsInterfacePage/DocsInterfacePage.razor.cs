using ClearBlazor;
using ClearBlazor.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsInterfacePage:ClearComponentBase
    {
        [Parameter]
        public IOtherDocsInfo? DocsInfo { get; set; }

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }
    }
}