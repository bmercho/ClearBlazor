using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsEnum:ClearComponentBase
    {
        [Parameter]
        public string EnumName { get; set; } = string.Empty;

        [Parameter]
        public string EnumDescription { get; set; } = string.Empty;

        [Parameter]
        public List<ApiFieldInfo>? FieldApi { get; set; }

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }
    }
}