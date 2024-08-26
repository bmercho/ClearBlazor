using Microsoft.AspNetCore.Components;
using ClearBlazor;

namespace ClearBlazorTest
{
    public partial class ApiComponent : ClearComponentBase, IBackground, IBorder
    {
        [Parameter]
        public List<ApiComponentInfo>? ParameterApi { get; set; }

        [Parameter]
        public List<ApiComponentInfo>? PropertyApi { get; set; }

        [Parameter]
        public List<ApiComponentInfo>? MethodApi { get; set; }

        [Parameter]
        public List<ApiComponentInfo>? EventApi { get; set; }

        [Parameter]
        public string? BorderThickness { get; set; } = null;

        [Parameter]
        public Color? BorderColour { get; set; } = null;

        [Parameter]
        public string? CornerRadius { get; set; } = "0";

        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColour { get; set; } = null;

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }

    }
}