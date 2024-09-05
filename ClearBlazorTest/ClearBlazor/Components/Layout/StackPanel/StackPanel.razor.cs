using global::Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class StackPanel:ClearComponentBase, IBackground, IBorder, IBoxShadow
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

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

        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        [Parameter]
        public double Spacing { get; set; } = 0;

        [Parameter]
        public string? DropZoneName { get; set; } = null;

        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnElementMouseEnter { get; set; }

        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnElementMouseLeave { get; set; }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override string UpdateStyle(string css)
        {
            if (Orientation == Orientation.Landscape)
                css += $"display: flex; flex-direction: row;";
            else
                css += $"display: flex; flex-direction: column; ";
            if (Spacing != 0)
                css += $"gap: {Spacing}px; ";

            return css;
        }
    }
}