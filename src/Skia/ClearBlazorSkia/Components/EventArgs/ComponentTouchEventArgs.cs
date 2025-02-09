
namespace ClearBlazor
{
    public class ComponentTouchEventArgs
    {
        public CanvasTouchEventArgs TouchEventArgs { get; set; }

        public ClearComponentBase Component { get; set; }

        public bool Handled { get; set; }

        public ComponentTouchEventArgs(CanvasTouchEventArgs touchEventArgs, 
                                         ClearComponentBase component)
        {
            TouchEventArgs = touchEventArgs;
            Component = component;
            Handled = false;
        }   
    }
}
