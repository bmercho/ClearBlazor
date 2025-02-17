using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorSkia.Tests.Tests
{
    public class TestBase : ComponentBase, IHandleEvent
    {
        [Parameter]
        public EventCallback<bool> TestComplete { get; set; }

        bool _hideCheckLogs = false;
        bool _hideAllLogs = false;
        protected bool CheckComponentLocation(ClearComponentBase component,
                                              double actualWidth,
                                              double actualHeight,
                                              double left,
                                              double top)
        {
            if (!_hideCheckLogs && !_hideAllLogs)
            {
                Console.WriteLine($"CheckComponentLocation: " +
                                  $"Width:{component.ActualWidth} Height:{component.ActualHeight} " +
                                  $"Left:{component.Left} Top:{component.Top}");
                Console.WriteLine($"width:{actualWidth} height:{actualHeight} " +
                                  $"left:{left} top:{top}");
            }
            if (!_hideAllLogs)
                ShowComponentLayoutParams(component);
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
        protected void ShowComponentLayoutParams(ClearComponentBase component)
        {
            string name = "";
            if (string.IsNullOrEmpty(component.Name))
                name = component.GetType().Name;
            else
                name = component.Name;

            Console.WriteLine($"Name: {name} \n" +
                                  $"DesiredSize:{component.DesiredSize.Width}:" +
                                  $"{component.DesiredSize.Height} \n" +
                                  $"RenderSize:{component.RenderSize.Width}:" +
                                  $"{component.RenderSize.Height} \n" +
                                  $"ContentSize:{component.ContentSize.Width}:" +
                                  $"{component.ContentSize.Height} \n");
            //                         $"ClipRect:{component.ClipRect.Width}:" +
            //                         $"{component.ClipRect.Height} " +
            //                        $"{component.ClipRect.Top} " +
            //                       $"{component.ClipRect.Left} ");

        }

        Task IHandleEvent.HandleEventAsync(
              EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

    }
}
