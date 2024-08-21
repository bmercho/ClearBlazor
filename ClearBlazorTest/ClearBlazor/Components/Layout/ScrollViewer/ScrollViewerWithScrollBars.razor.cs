using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class ScrollViewer : ClearComponentBase, IContent
    {
        [Parameter]
        public ScrollMode HorizontalScrollMode { get; set; } = ScrollMode.Disabled;

        [Parameter]
        public ScrollMode VerticalScrollMode { get; set; } = ScrollMode.Auto;

        [Parameter]
        public bool ScrollSmoothly { get; set; } = true;

        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;


        private static List<IObserver<bool>> observers = new List<IObserver<bool>>();
        private double MarginTop = 0;
        private double MarginLeft = 0;
        private string ScrollId = Guid.NewGuid().ToString();
        private string VerticalThumbId = Guid.NewGuid().ToString();
        private string HorizontalThumbId = Guid.NewGuid().ToString();
        private SizeInfo? ScrollableSize = null;
        private SizeInfo? ScrollViewerSize = null;
        SizeInfo? previousScrollerViewerSize = null;

        private ElementReference ScrollElement;
        private ElementReference ScrollViewerElement;
        private string? PendingScrollIntoViewId = null;
        private string? PendingScrollToStartId = null;
        private string? PendingScrollToEndId = null;
        private int ScrollBarWidth = 0;
        private int ScrollBarHeight = 0;
        private int ScrollBarBorderRadius = 0;
        private int ScrollBarThumbBorderWidth = 0;
        private Color ScrollBarBackgroundColour = Color.Transparent;
        private Color ScrollBarThumbColour = Color.Transparent;
        private bool StopPropogation = false;
        private bool VerticalMouseDown = false;
        private bool HorizontalMouseDown = false;
        private ElementReference VerticalThumbElement;
        private ElementReference HorizontalThumbElement;


        protected override void OnInitialized()
        {
            base.OnInitialized();

            (ScrollBarWidth, ScrollBarHeight, ScrollBarBorderRadius, ScrollBarThumbBorderWidth) =
                ThemeManager.CurrentTheme.GetScrollBarProperties();
            ScrollBarBackgroundColour = ThemeManager.CurrentPalette.ScrollbarBackgroundColour;
            ScrollBarThumbColour = ThemeManager.CurrentPalette.ScrollbarThumbColour;
        }

        public async Task ScrollIntoView(string id, bool scrollSmoothly)
        {
            if (ScrollViewerSize == null)
            {
                PendingScrollIntoViewId = id;
                return;
            }

            var componentSize = await JSRuntime.InvokeAsync<SizeInfo>("GetSizeInfo", id);

            MarginTop -= componentSize.ElementY - ScrollViewerSize.ElementY -
                         ScrollViewerSize.ElementHeight / 2 +
                         componentSize.ElementHeight / 2;
            MarginTop = ConstrainMarginTop(MarginTop);
            await DoScrolling(scrollSmoothly);
        }

        public async Task ScrollToStart(string id, bool scrollSmoothly)
        {
            if (ScrollViewerSize == null)
            {
                PendingScrollToStartId = id;
                return;
            }

            var componentSize = await JSRuntime.InvokeAsync<SizeInfo>("GetSizeInfo", id);

            MarginTop = 0;
            await DoScrolling(scrollSmoothly);
        }

        public async Task ScrollToEnd(string id, bool scrollSmoothly)
        {
            if (ScrollViewerSize == null)
            {
                PendingScrollToEndId = id;
                return;
            }

            if (ScrollableSize == null)
                return;

            var componentSize = await JSRuntime.InvokeAsync<SizeInfo>("GetSizeInfo", id);

            MarginTop = -ScrollableSize.ElementHeight + ScrollViewerSize.ElementHeight;
            await DoScrolling(scrollSmoothly);
        }

        private async Task DoScrolling(bool scrollSmoothly)
        {
            if (scrollSmoothly)
                await JSRuntime.InvokeVoidAsync("SetClasses", ScrollId, "scrolling-transition");
            else
                await JSRuntime.InvokeVoidAsync("SetClasses", ScrollId, string.Empty);
            await JSRuntime.InvokeVoidAsync("SetStyleProperty", ScrollId, "margin-top", $"{MarginTop}px");
            // Delay stops smooth scrolling at startup
            await Task.Delay(100).ContinueWith(async t =>
                {
                    if (ScrollSmoothly)
                        await JSRuntime.InvokeVoidAsync("SetClasses", ScrollId, "scrolling-transition");
                    else
                        await JSRuntime.InvokeVoidAsync("SetClasses", ScrollId, string.Empty);
                });
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            IsScroller = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            ScrollableSize = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", ScrollElement);
            ScrollViewerSize = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", ScrollViewerElement);

            if (previousScrollerViewerSize == null || !previousScrollerViewerSize.Equals(ScrollableSize))
            {

                StateHasChanged();
                previousScrollerViewerSize = ScrollableSize;

                // Delay stops smooth scrolling at startup
                await Task.Delay(100).ContinueWith(async t =>
                {

                    await JSRuntime.InvokeVoidAsync("SetClasses", VerticalThumbId, "scrolling-transition");
                    await JSRuntime.InvokeVoidAsync("SetClasses", HorizontalThumbId, "scrolling-transition");
                });
            }

            if (PendingScrollIntoViewId != null)
            {
                await ScrollIntoView(PendingScrollIntoViewId, false);
                PendingScrollIntoViewId = null;
            }
            if (PendingScrollToStartId != null)
            {
                await ScrollToStart(PendingScrollToStartId, false);
                PendingScrollToStartId = null;
            }
            if (PendingScrollToEndId != null)
            {
                await ScrollToEnd(PendingScrollToEndId, false);
                PendingScrollToEndId = null;
            }
            if (!ScrollSmoothly)
                await JSRuntime.InvokeVoidAsync("SetClasses", ScrollId, "");
        }

        protected override string UpdateStyle(string css)
        {
            //switch (VerticalScrollMode)
            //{
            //    case ScrollMode.Disabled:
            //        css += $"overflow-y: hidden; ";
            //        break;
            //    case ScrollMode.Auto:
            //        css += $"overflow-y: auto; ";
            //        break;
            //    case ScrollMode.Enabled:
            //        //                    css += $"overflow-y: scroll;";
            //        css += $"overflow-y: scroll;  scrollbar-gutter:stable; ";
            //        break;
            //}

            //switch (HorizontalScrollMode)
            //{
            //    case ScrollMode.Disabled:
            //        css += $"overflow-x: hidden; ";
            //        break;
            //    case ScrollMode.Auto:
            //        css += $"overflow-x: auto;";
            //        break;
            //    case ScrollMode.Enabled:
            //        //                   css += $"overflow-x: scroll;";
            //        css += $"overflow-x: scroll; scrollbar-gutter:stable; ";
            //        break;
            //}
            css += "display:grid; grid-template-columns: 1fr auto;  grid-template-rows: 1fr auto; ";
            var margin = Thickness.Parse(Margin);
            var padding = Thickness.Parse(Padding);
            if (!css.Contains("width:"))
                css += $"width: Calc(100% - {margin.HorizontalThickness}px) - {padding.HorizontalThickness}px); ";
            if (!css.Contains("height:"))
                css += $"height: Calc(100% - {margin.VerticalThickness}px) - {padding.VerticalThickness}px); ";

            return css;
        }

        private bool ShowVerticalScrollBar()
        {
            if (VerticalScrollMode == ScrollMode.Disabled)
            {
                StopPropogation = false;
                return false;
            }

            if (VerticalScrollMode == ScrollMode.Enabled)
            {
                StopPropogation = true;
                return true;
            }

            if (ScrollableSize == null || ScrollViewerSize == null)
            {
                StopPropogation = false;
                return false;
            }

            if (VerticalScrollMode == ScrollMode.Auto && ScrollableSize.ElementHeight > ScrollViewerSize.ElementHeight)
            {
                StopPropogation = true;
                return true;
            }

            StopPropogation = false;
            return false;
        }

        private bool ShowHorizontalScrollBar()
        {
            if (HorizontalScrollMode == ScrollMode.Disabled)
                return false;

            if (HorizontalScrollMode == ScrollMode.Enabled)
                return true;

            if (ScrollableSize == null || ScrollViewerSize == null)
                return false;

            if (HorizontalScrollMode == ScrollMode.Auto && ScrollableSize.ElementWidth > ScrollViewerSize.ElementWidth)
                return true;

            return false;
        }

        private double GetVerticalThumbSize()
        {
            if (ScrollableSize == null || ScrollViewerSize == null)
                return 0;

            return (ScrollViewerSize.ElementHeight / ScrollableSize.ElementHeight) * ScrollViewerSize.ElementHeight;
        }

        private double GetHorizontalThumbSize()
        {
            if (ScrollableSize == null || ScrollViewerSize == null)
                return 0;

            return (ScrollViewerSize.ElementWidth / ScrollableSize.ElementWidth) * ScrollViewerSize.ElementWidth;
        }

        private string GetVerticalThumbStyle()

        {
            return $"height:{GetVerticalThumbSize()}px; width:{@ScrollBarWidth}px; margin-top:{GetVerticalThumbPosition()}px; " +
                   $"border-radius:{@ScrollBarBorderRadius}px;  background-color:{ScrollBarThumbColour.Value} ";
        }

        private string GetHorizontalThumbStyle()
        {
            return $"width:{GetHorizontalThumbSize()}px; height:{@ScrollBarHeight}px; margin-left:{GetHorizontalThumbPosition()}px;" +
                   $" border-radius:{@ScrollBarBorderRadius}px; background-color:{ScrollBarThumbColour.Value} ";
        }

        private double GetVerticalThumbPosition()
        {
            if (ScrollableSize == null || ScrollViewerSize == null)
                return 0;

            return -MarginTop / (ScrollableSize.ElementHeight - ScrollViewerSize.ElementHeight) *
                   (ScrollViewerSize.ElementHeight - GetVerticalThumbSize() - 1);
        }
        private double GetHorizontalThumbPosition()
        {
            if (ScrollableSize == null || ScrollViewerSize == null)
                return 0;

            return -MarginLeft / (ScrollableSize.ElementWidth - ScrollViewerSize.ElementWidth) *
                   (ScrollViewerSize.ElementWidth - GetHorizontalThumbSize() - 1);
        }

        private string GetHorizontalBackgroundStyle()
        {
            return $"height:{@ScrollBarHeight}px; border-style:solid; border-width:1px; border-color:{ScrollBarBackgroundColour.Lighten(4).Value};  border-radius:{@ScrollBarBorderRadius}px; background-color:{ScrollBarBackgroundColour.Value} ";
        }
        private string GetVerticalBackgroundStyle()
        {
            return $"width:{@ScrollBarWidth}px; border-style:solid;  border-width:1px; border-color:{ScrollBarBackgroundColour.Lighten(4).Value}; border-radius:{@ScrollBarBorderRadius}px; background-color:{ScrollBarBackgroundColour.Value} ";
        }

        private async Task OnMouseWheel(WheelEventArgs e)
        {
            if (ShowVerticalScrollBar() && VerticalScrollMode == ScrollMode.Auto || VerticalScrollMode == ScrollMode.Enabled)
            {
                MarginTop -= e.DeltaY;
                MarginTop = ConstrainMarginTop(MarginTop);
                await JSRuntime.InvokeVoidAsync("SetStyleProperty", ScrollId, "margin-top", $"{MarginTop}px");
                OnScroll();
                StateHasChanged();
            }
        }

        private double ConstrainMarginTop(double marginTop)
        {
            if (ScrollableSize == null || ScrollViewerSize == null)
                return 0;

            if (MarginTop > 0)
                return 0;

            if (MarginTop < -ScrollableSize.ElementHeight + ScrollViewerSize.ElementHeight)
                return -ScrollableSize.ElementHeight + ScrollViewerSize.ElementHeight;

            return marginTop;
        }

        public void OnScroll()
        {
            foreach (var observer in observers)
                observer.OnNext(true);
        }

        private async Task OnVerticalMouseDown(MouseEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("CaptureMouse", VerticalThumbElement, 1);

            VerticalMouseDown = true;
            StateHasChanged();
        }
        private async Task OnVerticalMouseUp(MouseEventArgs e)
        {
            VerticalMouseDown = false;
            await JSRuntime.InvokeVoidAsync("ReleaseMouseCapture", VerticalThumbElement, 1);
            StateHasChanged();

        }

        private async Task OnVerticalMouseMove(MouseEventArgs e)
        {
            if (VerticalMouseDown)
            {
                //if (SizeInfo == null)
                //    return;

                //var offset = e.ClientX - SizeInfo.ElementX;
                //double value = (offset / SizeInfo.ElementWidth) * (MaxDouble - MinDouble) + MinDouble;
                //value = Math.Round(value / StepDouble) * StepDouble;

                //if (value <= MinDouble)
                //    value = MinDouble;
                //if (value >= MaxDouble)
                //    value = MaxDouble;

                //Value = (TItem)Convert.ChangeType(value, typeof(TItem));
                //HandleValueChange();

                //await ValueChanged.InvokeAsync((TItem)Value);
                StateHasChanged();
            }
        }

        public static IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<bool>> _observers;
            private IObserver<bool> _observer;

            public Unsubscriber(List<IObserver<bool>> observers, IObserver<bool> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }

    }
}