using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
