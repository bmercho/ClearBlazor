using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// The AppBar displays information and actions relating to the AppBar's content.
    /// </summary>
    public class AppBar:DockPanel,IColour
    {
        /// <summary>
        /// The foreground color of the AppBar
        /// </summary>
        [Parameter]
        public Color? Colour { get; set; } = null;

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (BackgroundColour == null)
                BackgroundColour = Color.Primary;
            Colour = Color.ContrastingColor(BackgroundColour); 
        }
    }
}