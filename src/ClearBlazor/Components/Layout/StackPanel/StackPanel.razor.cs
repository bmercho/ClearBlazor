using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace ClearBlazor
{
    /// <summary>
    /// Arranges children elements into a single line that can be oriented horizontally or vertically.
    /// </summary>
    public partial class StackPanel:ClearComponentBase, IBackground, IBorder, IBoxShadow
    {
        /// <summary>
        /// Defines the orientation of the stack panel. Landscape or portrait
        /// </summary>
        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        /// <summary>
        /// The spacing between children in the direction defined by Orientation.
        /// </summary>
        [Parameter]
        public double Spacing { get; set; } = 0;

        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href="IBoxShadowApi">IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; } = null;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = null;

        /// <summary>
        /// Event raised when the mouse enters the component 
        /// </summary>
        [Parameter]
        public virtual EventCallback<MouseEventArgs> OnElementMouseEnter { get; set; }

        /// <summary>
        /// Event raised when the mouse leaves the component 
        /// </summary>
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