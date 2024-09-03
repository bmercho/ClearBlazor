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
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColour { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }

        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColour { get; set; } = null;

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }

        private string GetMarkupString(string value)
        {
            return new MarkupString(value).ToString();
        }
    }
}