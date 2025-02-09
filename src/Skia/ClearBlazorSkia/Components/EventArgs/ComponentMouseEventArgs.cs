using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public class ComponentMouseEventArgs
    {
        public MouseEventArgs MouseEventArgs { get; set; }

        public ClearComponentBase Component { get; set; }

        public bool Handled { get; set; }

        public ComponentMouseEventArgs(MouseEventArgs mouseEventArgs, 
                                       ClearComponentBase component)
        {
            MouseEventArgs = mouseEventArgs;
            Component = component;
            Handled = false;
        }   
    }
}
