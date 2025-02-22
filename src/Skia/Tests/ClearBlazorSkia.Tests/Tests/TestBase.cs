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

        protected bool CheckIntermediateValues(ClearComponentBase component,
                                               double measureInWidth,
                                               double measureInHeight,
                                               double measureOutWidth,
                                               double measureOutHeight,
                                               double arrangeInWidth,
                                               double arrangeInHeight,
                                               double arrangeOutWidth,
                                               double arrangeOutHeight)
        {
            string name = "";
            if (string.IsNullOrEmpty(component.Name))
                name = component.GetType().Name;
            else
                name = component.Name;


            if (!_hideAllLogs && _showCheckLocationLogs)
            {
                Console.WriteLine();
                Console.WriteLine($"CheckIntermediateValues: Name: {name} \n" +
                                  $"    MeasureIn:{component._measureIn.Height}-{component._measureIn.Width} " +
                                  $"MeasureOut:{component._measureOut.Width}-{measureOutWidth} ");
                Console.WriteLine($"    measureIn:{measureInWidth}:{measureInHeight} " +
                                  $"measureOut:{measureOutWidth}:{measureOutHeight} ");
                Console.WriteLine($"    ArrangeIn:{component._arrangeIn.Height}:{component._arrangeIn.Width} " +
                                  $"ArrangeOut:{component._arrangeOut.Width}:{component._arrangeOut.Width} ");
                Console.WriteLine($"    arrangeIn:{arrangeInWidth}:{arrangeInHeight} " +
                                  $"arrangeOut:{arrangeOutWidth}:{arrangeOutHeight} \n");
            }

            if (component == null ||
                !DoubleUtils.AreClose(component._measureIn.Height, measureInHeight) ||
                !DoubleUtils.AreClose(component._measureIn.Width, measureInWidth) ||
                !DoubleUtils.AreClose(component._measureOut.Height, measureOutHeight) ||
                !DoubleUtils.AreClose(component._measureOut.Width, measureOutWidth) ||
                !DoubleUtils.AreClose(component._arrangeIn.Height, arrangeInHeight) ||
                !DoubleUtils.AreClose(component._arrangeIn.Width, arrangeInWidth) ||
                !DoubleUtils.AreClose(component._arrangeOut.Height, arrangeOutHeight) ||
                !DoubleUtils.AreClose(component._arrangeOut.Width, arrangeOutWidth))
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
