using ClearBlazor;
using ClearBlazor.Common;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class ApiFields : ClearComponentBase, IBackground, IBorder
    {
        [Parameter]
        public List<ApiFieldInfo>? FieldApi { get; set; }

        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColor { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }

        [Parameter]
        public int? BoxShadow { get; set; } = null;

        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        protected override string UpdateStyle(string css)
        {
            return css + "display:grid; ";
        }

    }
}