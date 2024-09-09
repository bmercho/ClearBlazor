using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public partial class ToggleIconButton : Button
    {

        [Parameter]
        public EventCallback<bool> OnToggleChanged { get; set; }

        [Parameter]
        public string? ToggledIcon { get; set; } = null;

        [Parameter]
        public Color? ToggledIconColor { get; set; } = null;

        [Parameter]
        public string Text { get; set; } = string.Empty;
 
        [Parameter]
        public string ToggledText { get; set; } = string.Empty;

        protected Size IconSize { get; set; }

        private async Task Clicked()
        {
            toggled = !toggled;
            await OnToggleChanged.InvokeAsync(toggled);
        }

        private bool toggled = false;
    }
}