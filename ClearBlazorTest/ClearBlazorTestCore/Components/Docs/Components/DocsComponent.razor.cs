using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class DocsComponent:ClearComponentBase
    {
        [Parameter]
        public string ComponentName { get; set; } = string.Empty;

        [Parameter]
        public string ComponentDescription { get; set; } = string.Empty;

        [Parameter]
        public List<ApiComponentInfo>? ParameterApi { get; set; }

        [Parameter]
        public List<ApiComponentInfo>? PropertyApi { get; set; }

        [Parameter]
        public List<ApiComponentInfo>? MethodApi { get; set; }

        [Parameter]
        public List<ApiComponentInfo>? EventApi { get; set; }

    }
}