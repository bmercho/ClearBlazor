using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Api:ClearComponentBase,IBackground,IBorder
    {
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