using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    /// <summary>
    /// Represents a bar used to display actions, branding, navigation and screen titles.
    /// </summary>
    public class AppBar:DockPanel,IColour
    {
        /// <summary>
        /// The foreground color of the AppBar
        /// </summary>
        [Parameter]
        public Color? Colour { get; set; } = null;

        /// <summary>
        /// Do some thing
        /// </summary>
        public void DoSomething()
        {

        }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (BackgroundColour == null)
                BackgroundColour = Color.Primary;
            Colour = Color.ContrastingColor(BackgroundColour); 
        }
    }
}