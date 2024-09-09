using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public  class MouseOverEventArgs
    {
        public MouseOverEventArgs(MouseEventArgs eventArgs, ElementReference element)
        {
            EventArgs = eventArgs;
            Element = element;
        }
        public MouseEventArgs EventArgs { get; set; }

        public ElementReference Element { get; set; }

    }
}
