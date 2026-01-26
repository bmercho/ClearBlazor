using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// A popup control that can be used to display additional information.
    /// </summary>
    public partial class PopupCursor : ClearComponentBase
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Indicates whether the popup cursor is open or closed.
        /// </summary>
        [Parameter]
        public bool Open { get; set; } = false;

        /// <summary>
        /// Event that is raised when the popup is opened or closed.
        /// </summary>
        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        [Parameter]
        public double XPos { get; set; } = 0;
        [Parameter]
        public double YPos { get; set; } = 0;

        protected override string UpdateStyle(string css)
        {
            css += "z-index:100;";
            css += "display: grid; ";
            css += GetLocationCss();
            css += "white-space:pre; text-align:justify; ";
            return css;
        }

        private string GetLocationCss()
        {
            return $"position: absolute; top: {YPos}px; left: {XPos}px; ";
        }
    }
}