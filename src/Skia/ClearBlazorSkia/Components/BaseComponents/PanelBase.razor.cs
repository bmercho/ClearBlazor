using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A base for all Panel type components.
    /// </summary>
    public abstract partial class PanelBase : ClearComponentBase, IBorder, IBackground, IBackgroundGradient
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        // IBorder

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string BorderThickness { get; set; } = "0";

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

        // IBoxShadow

        /// <summary>
        /// See <a href="IBoxShadowApi">IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; }

        // IBackground

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; }

        // IBackgroundGradient

        /// <summary>
        /// See <a href="IBackgroundGradientApi">IBackgroundGradient</a>
        /// </summary>
        [Parameter]
        public string? BackgroundGradient1 { get; set; }


        /// <summary>
        /// See <a href="IBackgroundGradientApi">IBackgroundGradient</a>
        /// </summary>
        [Parameter]
        public string? BackgroundGradient2 { get; set; }

    }
}