using global::Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class Border : ClearComponentBase, IContent, IBorder, IBackground,IBackgroundGradient, IDraggable
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

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

        [Parameter]
        public string? BackgroundGradient1 { get; set; } = null;
        [Parameter]
        public string? BackgroundGradient2 { get; set; } = null;


        [Parameter]
        public bool IsDraggable { get; set; } = false;

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
        protected override string UpdateStyle(string css)
        {
            css += $"display : grid; overflow:visible; ";
            return css;
        }

        private string GetDraggable()
        {
            return IsDraggable ? "true" : "false";
        }
    }
}