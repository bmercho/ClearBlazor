using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// The AppBar displays information and actions relating to the AppBar's content.
    /// </summary>
    public class AppBar:DockPanel,IColor
    {
        /// <summary>
        /// The foreground color of the AppBar
        /// </summary>
        [Parameter]
        public Color? Color { get; set; } = null;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (BackgroundColor == null)
                BackgroundColor = Color.Primary;
            Color = Color.ContrastingColor(BackgroundColor); 
        }
    }
}