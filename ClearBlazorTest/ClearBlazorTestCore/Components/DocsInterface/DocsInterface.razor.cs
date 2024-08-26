using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsInterface:ClearComponentBase
    {
        [Parameter]
        public string InterfaceName { get; set; } = string.Empty;

        [Parameter]
        public string InterfaceDescription { get; set; } = string.Empty;

        [Parameter]
        public List<ApiFieldInfo>? FieldApi { get; set; }

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }
    }
}