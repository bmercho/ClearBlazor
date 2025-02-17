using ClearBlazor;
using Microsoft.AspNetCore.Components;

namespace ClearBlazorSkia.Tests.Tests
{
    public class TestBase : ComponentBase, IHandleEvent
    {
        [Parameter]
        public EventCallback<bool> TestComplete { get; set; }

        bool _hideAllLogs = true;
        bool _showCheckLocationLogs = true;
        bool _showComponentLayoutParams = false;

        protected bool CheckComponentLocation(ClearComponentBase component,
                                              double actualWidth,
                                              double actualHeight,
                                              double left,
                                              double top,
                                              double clipLeft = 0,
                                              double clipTop = 0,
                                              double clipWidth = 0,
                                              double clipHeight = 0)

        {
            string name = "";
            if (string.IsNullOrEmpty(component.Name))
                name = component.GetType().Name;
            else
                name = component.Name;


            if (!_hideAllLogs && _showCheckLocationLogs)
            {
                Console.WriteLine();
                Console.WriteLine($"CheckComponentLocation: Name: {name} \n" +
                                  $"    Left:{component.Left} Top:{component.Top} " +
                                  $"Width:{component.ActualWidth} Height:{component.ActualHeight} ");
                Console.WriteLine($"    left:{left} top:{top} " +
                                  $"width:{actualWidth} height:{actualHeight} ");
                Console.WriteLine($"    ClipLeft:{component.ClipRect.Left} " +
                                  $"ClipTop:{component.ClipRect.Top} " +
                                  $"ClipWidth:{component.ClipRect.Width} " +
                                  $"ClipHeight:{component.ClipRect.Height} ");
                Console.WriteLine($"    clipLeft:{clipLeft} clipTop:{clipTop} " +
                                  $"clipWidth:{clipWidth} clipHeight:{clipHeight}\n");
            }
            if (!_hideAllLogs && _showComponentLayoutParams)
                ShowComponentLayoutParams(component);

            bool close = !DoubleUtils.AreClose((double)component.ClipRect.Width, clipWidth);

            if (component == null ||
                !DoubleUtils.AreClose(component.ActualHeight, actualHeight) ||
                !DoubleUtils.AreClose(component.ActualWidth, actualWidth) ||
                !DoubleUtils.AreClose(component.Top, top) ||
                !DoubleUtils.AreClose(component.Left, left) ||
                !DoubleUtils.AreClose((double)component.ClipRect.Left, clipLeft) ||
                !DoubleUtils.AreClose((double)component.ClipRect.Top, clipTop) ||
                !DoubleUtils.AreClose((double)component.ClipRect.Width, clipWidth) ||
                !DoubleUtils.AreClose((double)component.ClipRect.Height, clipHeight))
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

            Console.WriteLine($"ComponentLayoutParams: Name: {name} \n" +
                                  $"    DesiredSize:{component.DesiredSize.Width}:" +
                                  $"{component.DesiredSize.Height} \n" +
                                  $"    RenderSize:{component.RenderSize.Width}:" +
                                  $"{component.RenderSize.Height} \n" +
                                  $"    ContentSize:{component.ContentSize.Width}:" +
                                  $"{component.ContentSize.Height} \n");
        }

        Task IHandleEvent.HandleEventAsync(
              EventCallbackWorkItem callback, object? arg) => callback.InvokeAsync(arg);

    }
}
