using ClearBlazor;
using ClearBlazor.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsEnum:ClearComponentBase
    {
        [Parameter]
        public IEnumerationDocsInfo? DocsInfo { get; set; }

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }
    }
}