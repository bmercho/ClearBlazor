using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public class ComponentPointerEventArgs
    {
        public PointerEventArgs PointerEventArgs { get; set; }

        public ClearComponentBase Component { get; set; }

        public bool Handled { get; set; }

        public ComponentPointerEventArgs(PointerEventArgs pointerEventArgs, 
                                         ClearComponentBase component)
        {
            PointerEventArgs = pointerEventArgs;
            Component = component;
            Handled = false;
        }   
    }
}
