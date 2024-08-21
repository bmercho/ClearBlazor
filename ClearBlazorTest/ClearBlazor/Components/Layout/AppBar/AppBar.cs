using Microsoft.AspNetCore.Components;

namespace ClearBlazor
{
    public class AppBar:DockPanel,IColour
    {
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