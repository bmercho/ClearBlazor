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
            Console.WriteLine($"CheckComponentLocation: " +
                              $"Width:{component.ActualWidth} Height:{component.ActualHeight} " +
                              $"Left:{component.Left} Top:{component.Top}");
            Console.WriteLine($"width:{actualWidth} height:{actualHeight} " +
                              $"left:{left} top:{top}");
            if (component == null ||
                !DoubleUtils.AreClose(component.ActualHeight, actualHeight) ||
                !DoubleUtils.AreClose(component.ActualWidth, actualWidth) ||
                !DoubleUtils.AreClose(component.Top, top) ||
                !DoubleUtils.AreClose(component.Left, left))
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
