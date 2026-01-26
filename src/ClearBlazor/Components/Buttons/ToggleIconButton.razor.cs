using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// A button that has an on and off state.
    /// When toggled shows the ToggledIcon and ToggledText otherwise shows th Icon and Text
    /// </summary>
    public partial class ToggleIconButton : Button
    {
        /// <summary>
        /// An event that is raised when the button is toggled
        /// </summary>
        [Parameter]
        public EventCallback<bool> OnToggleChanged { get; set; }

        /// <summary>
        /// The toggled icon. Icon is used for the un-toggled icon.
        /// </summary>
        [Parameter]
        public string? ToggledIcon { get; set; } = null;

        /// <summary>
        /// The toggled icon color. Color is used for the un-toggled icon color.
        /// </summary>
        [Parameter]
        public Color? ToggledIconColor { get; set; } = null;

        /// <summary>
        /// The un-toggled text
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// The toggled text
        /// </summary>
        [Parameter]
        public string ToggledText { get; set; } = string.Empty;

        private bool toggled = false;


        protected override string UpdateStyle(string css)
        {
            css += $"display: grid; background-color:transparent; ";
            return css;
        }

        /// <summary>
        /// Toggles the button.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> Toggle()
        {
            toggled = !toggled;
            await OnToggleChanged.InvokeAsync(toggled);
            return toggled;
        }

        private async Task Clicked()
        {
            await Toggle();
        }
    }
}