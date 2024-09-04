using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public class CanvasTouchEventArgs
    {
        public TouchEventArgs TouchEventArgs { get; set; }
        public double XOffset { get; set; }
        public double YOffset { get; set; }

        public CanvasTouchEventArgs(TouchEventArgs touchEventArgs, double xOffset, double yOffset)
        {
            TouchEventArgs = touchEventArgs;
            XOffset = xOffset;
            YOffset = yOffset;
        }   
    }
}
