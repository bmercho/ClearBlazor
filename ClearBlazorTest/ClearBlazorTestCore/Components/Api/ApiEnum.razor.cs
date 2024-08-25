using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorTest
{
    public partial class ApiEnum : ClearComponentBase, IBackground, IBorder
    {
        [Parameter]
        public List<ApiFieldInfo>? FieldApi { get; set; }

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
    }
}