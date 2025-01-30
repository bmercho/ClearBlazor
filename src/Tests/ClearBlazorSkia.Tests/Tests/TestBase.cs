using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorSkia.Tests.Tests
{
    public class TestBase:ComponentBase, IHandleEvent
    {
        [Parameter]
        public EventCallback<bool> TestComplete { get; set; }

        protected bool CheckComponentLocation(ClearComponentBase component,
                                              double actualWidth,
                                              double actualHeight,
                                              double left,
                                              double top)
        {
            if (component == null ||
                component.ActualHeight != actualHeight ||
                component.ActualWidth != actualWidth ||
                component.Top != top ||
                component.Left != left)
                return false;
            return true;
        }
        protected bool CheckComponentBorder(ClearComponentBase component,
                                            double borderTop,
                                            double borderRight,
                                            double borderBottom,
                                            double borderLeft)
        {
            if (component == null ||
                component._borderThickness.Top != borderTop ||
                component._borderThickness.Right != borderRight ||
                component._borderThickness.Bottom != borderBottom ||
                component._borderThickness.Left != borderLeft)
                return false;
            return true;
        }


        Task IHandleEvent.HandleEventAsync(
              EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

    }
}
