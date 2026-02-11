using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A Dialog control.
    /// </summary>
    public partial class Dialog : ClearComponentBase
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        protected override string UpdateStyle(string css)
        {
            css += "z-index:100;";
            css += "display: grid; ";
            css += $"background-color: {ThemeManager.CurrentColorScheme.Overlay.Value}; ";
            return css;
        }
    }
}