using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Runtime.Serialization;

namespace ClearBlazor
{
    public partial class ScrollViewerWithScrollBars : ClearComponentBase
    {
        /// <summary>
        /// The horizontal scroll bar mode.
        /// </summary>
        [Parameter]
        public ScrollMode HorizontalScrollMode { get; set; } = ScrollMode.Disabled;

        /// <summary>
        /// The vertical scroll bar mode.
        /// </summary>
        [Parameter]
        public ScrollMode VerticalScrollMode { get; set; } = ScrollMode.Auto;

        /// <summary>
        /// Indicates when the vertical scrollbar gutter exists
        /// </summary>
        [Parameter]
        public ScrollbarGutter VerticalGutter { get; set; } = ScrollbarGutter.OnlyWhenOverflowed;

        /// <summary>
        /// Indicates when the horizontal scrollbar gutter exists
        /// </summary>
        [Parameter]
        public ScrollbarGutter HorizontalGutter { get; set; } = ScrollbarGutter.OnlyWhenOverflowed;

        /// <summary>
        /// Defines what happens when the boundary of a scrolling area is reached in the vertical direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour VerticalOverscrollBehaviour { get; set; } = OverscrollBehaviour.Auto;

        /// <summary>
        /// Defines what happens when the boundary of a scrolling area is reached in the horizontal direction. 
        /// </summary>
        [Parameter]
        public OverscrollBehaviour HorizontalOverscrollBehaviour { get; set; } = OverscrollBehaviour.Auto;

        /// <summary>
        /// Indicates if scrollbars will 'overlay' style scrollbars where the scrollbars are overlayed 
        /// on top of the content. Overlay scrollbars do not show the scrollbar background.
        /// </summary>
        [Parameter]
        public bool UseOverlayScrollbars { get; set; } = false;

        /// <summary>
        /// The child content for this component
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; } = null;

        /// <summary>
        /// Called whenever scrolling has occurred. 
        /// Does not cause re-rendering of the ScrollViewer.
        /// </summary>
        [Parameter]
        public Func<ScrollState, Task>? ScrollCallback { get; set; }

        const int ScrollIncrement = 40;
        const int PageIncrement = 400;
        const double MinThumbSize = 30;

        private DotNetObjectReference<ScrollViewerWithScrollBars> _thisComponent = null!;
        private static List<IObserver<bool>> _observers = new List<IObserver<bool>>();
        internal string? _resizeObserverId = null;
        private double _marginTop = 0;
        private double _marginLeft = 0;
        private string _scrollId = Guid.NewGuid().ToString();
        private string _scrollViewerId = Guid.NewGuid().ToString();
        private string _verticalThumbId = Guid.NewGuid().ToString();
        private string _horizontalThumbId = Guid.NewGuid().ToString();
        private string _verticalScrollId = Guid.NewGuid().ToString();
        private string _horizontalScrollId = Guid.NewGuid().ToString();
        private ObservedSize? _scrollableSize = null;
        private ObservedSize? _scrollViewerSize = null;
        private string? _pendingScrollIntoViewId = null;
        private Alignment _pendingScrollIntoViewAlignment = Alignment.Start;
        private int _scrollbarWidth = 0;
        private int _scrollbarCornerRadius = 0;
        private int _scrollbarBackgroundBoxShadowWidth = 0;
        private Color _scrollbarBackgroundColor = Color.Transparent;
        private Color _scrollbarThumbColor = Color.Transparent;
        private Color _scrollbarBackgroundBoxShadowColor = Color.Transparent;
        private Color _scrollbarOverlayThumbColor = Color.Transparent;
        private bool _verticalMouseDown = false;
        private bool _horizontalMouseDown = false;
        private bool _overVerticalThumb = false;
        private bool _overHorizontalThumb = false;
        private bool _verticalScrolling = false;
        private bool _horizontalScrolling = false;
        private double _verticalThumbPosition = 0;
        private double _horizontalThumbPosition = 0;
        private bool _processingkey = false;
        private bool _scrollSmoothly = false;
        private ScrollState _lastScrollState = new ScrollState();
        private double _vertStartPos = 0;
        private double _horizStartPos = 0;

        public static IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }
        public async Task SetScrollTop(double top)
        {
            var position = top / GetVerticalFactor();
            await VerticalScrollTo(position, false);
        }

        public async Task SetScrollLeft(double left)
        {
            var position = left / GetHorizontalFactor();
            await HorizontalScrollTo(position, false);
        }

        public ScrollState GetScrollState()
        {
            return _lastScrollState;
        }
        public async Task ScrollToStart(ScrollDirection scrollDirection)
        {
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                await HorizontalScrollTo(0, false);
            }
            else
                await VerticalScrollTo(0, false);
        }

        public async Task ScrollToEnd(ScrollDirection scrollDirection)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;

            if (scrollDirection == ScrollDirection.Horizontal)
            {
                await HorizontalScrollTo(_scrollViewerSize.ElementWidth - GetHorizontalThumbSize(), false);
            }
            else
                await VerticalScrollTo(_scrollViewerSize.ElementHeight-GetVerticalThumbSize(), false);
        }

        public async Task ScrollIntoView(string id, Alignment alignment)
        {
            var componentSize = await JSRuntime.InvokeAsync<SizeInfo>("GetSizeInfo", id);

            if (_scrollViewerSize == null || componentSize == null)
            {
                _pendingScrollIntoViewId = id;
                _pendingScrollIntoViewAlignment = alignment;
                return;
            }

            double top = 0;
            switch (alignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    top = (componentSize.ElementY - componentSize.ParentY + componentSize.ElementHeight/2 - 
                           _scrollViewerSize.ElementHeight / 2 ) / GetVerticalFactor();
                    break;
                case Alignment.Start:
                    top = (componentSize.ElementY - componentSize.ParentY) / GetVerticalFactor();
                    break;
                case Alignment.End:
                    top = (componentSize.ElementY - componentSize.ParentY + componentSize.ElementHeight -
                           _scrollViewerSize.ElementHeight) / GetVerticalFactor();
                    break;
            }
            await VerticalScrollTo(top, false);
        }
        public bool AtVerticalScrollStart()
        {
            if (_marginTop == 0)
                return true;
            return false;
        }
        public bool AtVerticalScrollEnd()
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return false;
            if (_marginTop == -_scrollableSize.ElementHeight + _scrollViewerSize.ElementHeight)
                return true;
            return false;
        }

        public bool AtHorizontalScrollStart()
        {
            if (_marginLeft == 0)
                return true;
            return false;
        }
        public bool AtHorizontalScrollEnd()
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return false;
            if (_marginLeft == -_scrollableSize.ElementWidth + _scrollViewerSize.ElementWidth)
                return true;
            return false;
        }
        protected override void OnInitialized()
        {
            base.OnInitialized();

            _scrollbarWidth = ThemeManager.CurrentTheme.ScrollbarWidth;
            _scrollbarBackgroundBoxShadowWidth = ThemeManager.CurrentTheme.ScrollbarBackgroundBoxShadowWidth;
            _scrollbarCornerRadius = ThemeManager.CurrentTheme.ScrollbarCornerRadius;
            _scrollbarBackgroundColor = ThemeManager.CurrentColorScheme.SurfaceContainerLow;
            _scrollbarThumbColor = ThemeManager.CurrentColorScheme.Outline;
            _scrollbarBackgroundBoxShadowColor = ThemeManager.CurrentColorScheme.Outline;
            _scrollbarOverlayThumbColor = ThemeManager.CurrentColorScheme.SurfaceContainerHigh;
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            IsScroller = true;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _thisComponent = DotNetObjectReference.Create(this);
                await JSRuntime.InvokeVoidAsync("window.StopPropagation", Id, "wheel");

                List<string> elementIds = new List<string>() { _scrollId, _scrollViewerId };
                _resizeObserverId = await ResizeObserverService.Service.
                                    AddResizeObserver(NotifyObservedSizes, elementIds);
            }

            if (_pendingScrollIntoViewId != null)
            {
                await ScrollIntoView(_pendingScrollIntoViewId, _pendingScrollIntoViewAlignment);
                _pendingScrollIntoViewId = null;
            }
        }

        private async Task VerticalScroll(double thumbPositionDelta, bool scrollSmoothly)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;

            var position = _verticalThumbPosition + thumbPositionDelta / GetVerticalFactor();
            await VerticalScrollTo(position, scrollSmoothly);
        }

        private async Task VerticalScrollTo(double thumbPosition, bool scrollSmoothly)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;

            bool doScroll = false;
            if (ShowVerticalScrollBar())
            {
                if (SetVerticalThumbPosition(thumbPosition))
                {
                    var marginTop = -_verticalThumbPosition * GetVerticalFactor();

                    if (marginTop != _marginTop)
                    {
                        _marginTop = marginTop;
                        doScroll = true;
                    }
                }
            }

            if (doScroll)
                await DoVerticalScrolling(scrollSmoothly);
        }

        private async Task HorizontalScroll(double thumbPositionDelta, bool scrollSmoothly)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;

            var position = _horizontalThumbPosition + thumbPositionDelta / GetHorizontalFactor();
            await HorizontalScrollTo(position, scrollSmoothly);
        }

        private async Task HorizontalScrollTo(double thumbPosition, bool scrollSmoothly)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;

            bool doScroll = false;
            if (ShowHorizontalScrollBar())
            {
                if (SetHorizontalThumbPosition(thumbPosition))
                {
                    var marginLeft = -_horizontalThumbPosition * GetHorizontalFactor();

                    if (marginLeft != _marginLeft)
                    {
                        _marginLeft = marginLeft;
                        doScroll = true;
                    }
                }
            }

            if (doScroll)
                await DoHorizontalScrolling(scrollSmoothly);
        }

        private async Task DoVerticalScrolling(bool scrollSmoothly)
        {
            if (_scrollSmoothly != scrollSmoothly)
            {
                //if (scrollSmoothly)
                //{
                //    //await JSRuntime.InvokeVoidAsync("SetClasses", _verticalThumbId, "scrolling-transition");
                //    //await JSRuntime.InvokeVoidAsync("SetClasses", _horizontalThumbId, "scrolling-transition");
                //    await JSRuntime.InvokeVoidAsync("SetClasses", _scrollId, "scrolling-transition");
                //}
                //else
                //{
                //await JSRuntime.InvokeVoidAsync("SetClasses", _verticalThumbId, string.Empty);
                //await JSRuntime.InvokeVoidAsync("SetClasses", _horizontalThumbId, string.Empty);
                //await JSRuntime.InvokeVoidAsync("SetClasses", _scrollId, string.Empty);
                //}
                _scrollSmoothly = scrollSmoothly;
            }
            await JSRuntime.InvokeVoidAsync("SetStyleProperties",
                                           _scrollId, "transform", $"translateY({_marginTop}px)",
//                                           _verticalThumbId, "margin-top",
//                                           $"{_verticalThumbPosition}px");
                                           _verticalThumbId, "transform",
                                           $"translateY({_verticalThumbPosition}px)");
            await OnScroll();
        }

        private async Task DoHorizontalScrolling(bool scrollSmoothly)
        {
            if (_scrollSmoothly != scrollSmoothly)
            {
                if (scrollSmoothly)
                {
                    await JSRuntime.InvokeVoidAsync("SetClasses", _verticalThumbId, "scrolling-transition");
                    await JSRuntime.InvokeVoidAsync("SetClasses", _horizontalThumbId, "scrolling-transition");
                    await JSRuntime.InvokeVoidAsync("SetClasses", _scrollId, "scrolling-transition");
                }
                else
                {
                    await JSRuntime.InvokeVoidAsync("SetClasses", _verticalThumbId, string.Empty);
                    await JSRuntime.InvokeVoidAsync("SetClasses", _horizontalThumbId, string.Empty);
                    await JSRuntime.InvokeVoidAsync("SetClasses", _scrollId, string.Empty);
                }
                _scrollSmoothly = scrollSmoothly;
            }
            await JSRuntime.InvokeVoidAsync("SetStyleProperties",
                                           _scrollId, "margin-left", $"{_marginLeft}px",
                                           _horizontalThumbId, "margin-left",
                                           $"{_horizontalThumbPosition}px");
            await OnScroll();
        }

        private async Task OnScroll()
        {
            foreach (var observer in _observers)
                observer.OnNext(true);

            if (_scrollableSize == null || _scrollViewerSize == null)
                return;

            if (ScrollCallback == null)
                return;

            var scrollState = new ScrollState
            {
                ScrollTop = Math.Abs(_marginTop),
                ScrollLeft = Math.Abs(_marginLeft),
                ScrollHeight = _scrollableSize.ElementHeight,
                ScrollWidth = _scrollableSize.ElementWidth,
                ClientHeight = _scrollViewerSize.ElementHeight,
                ClientWidth = _scrollViewerSize.ElementWidth
            };

            if (_lastScrollState == scrollState)
                return;

            _lastScrollState = scrollState;
            await ScrollCallback(scrollState);
        }
        protected override string UpdateStyle(string css)
        {
            var width = _scrollbarWidth;
            css += $"display:grid; grid-template-columns: {width}px 1fr {width}px; " +
                   $"grid-template-rows: {width}px 1fr {width}px; ";
            var margin = Thickness.Parse(Margin);
            var padding = Thickness.Parse(Padding);
            if (!css.Contains("width:"))
                css += $"width: Calc(100% - {margin.HorizontalThickness}px) - {padding.HorizontalThickness}px); ";
            if (!css.Contains("height:"))
                css += $"height: Calc(100% - {margin.VerticalThickness}px) - {padding.VerticalThickness}px); ";

            return css;
        }

        private string GetContentStyle()
        {
            Console.WriteLine($"GetContentStyle");
            string css = "overflow: hidden; display: grid; ";
            if (UseOverlayScrollbars)
                return css + "grid-column: 1 / span 3; grid-row: 1 / span 3; ";

            return css + GetContentArea();
        }

        private bool ShowVerticalScrollBar()
        {
            if (VerticalScrollMode == ScrollMode.Disabled)
                return false;

            if (VerticalScrollMode == ScrollMode.Enabled)
                return true;

            if (_scrollableSize == null || _scrollViewerSize == null)
                return false;

            if (VerticalScrollMode == ScrollMode.Auto && _scrollableSize.ElementHeight > _scrollViewerSize.ElementHeight)
                return true;

            return false;
        }

        private bool ShowHorizontalScrollBar()
        {
            if (HorizontalScrollMode == ScrollMode.Disabled)
                return false;

            if (HorizontalScrollMode == ScrollMode.Enabled)
                return true;

            if (_scrollableSize == null || _scrollViewerSize == null)
                return false;

            if (HorizontalScrollMode == ScrollMode.Auto && _scrollableSize.ElementWidth > _scrollViewerSize.ElementWidth)
                return true;

            return false;
        }

        private double GetVerticalThumbSize()
        {
            if (_scrollableSize == null || _scrollViewerSize == null)
                return 0;

            if (_scrollViewerSize.ElementHeight == _scrollableSize.ElementHeight)
                return 0;

            double viewerHeight = _scrollViewerSize.ElementHeight;
            if (UseOverlayScrollbars)
                viewerHeight -= _scrollbarWidth;

            var size = (_scrollViewerSize.ElementHeight / _scrollableSize.ElementHeight) * viewerHeight;
            return Math.Max(size, MinThumbSize);
        }

        private double GetHorizontalThumbSize()
        {
            if (_scrollableSize == null || _scrollViewerSize == null)
                return 0;

            if (_scrollViewerSize.ElementWidth == _scrollableSize.ElementWidth)
                return 0;

            double viewerWidth = _scrollViewerSize.ElementWidth;
            if (UseOverlayScrollbars)
                viewerWidth -= _scrollbarWidth;

            var size = (_scrollViewerSize.ElementWidth / _scrollableSize.ElementWidth) * viewerWidth;
            return Math.Max(size, MinThumbSize);
        }

        private string GetVerticalThumbStyle()
        {
            string css = $"height:{GetVerticalThumbSize()}px; width:{_scrollbarWidth}px; " +
                         $"transform:translateY({_marginTop}px); " + 
//                         $"margin-top:{_verticalThumbPosition}px; " +
                         $"border-radius:{_scrollbarCornerRadius}px; user-select:none; ";
            if (UseOverlayScrollbars)
                css += $"background-color:{_scrollbarOverlayThumbColor.Value}; ";
            else
                css += $"background-color:{_scrollbarThumbColor.Value}; ";

            Console.WriteLine($"GetVerticalThumbStyle: {css}");
            return css;
        }

        private string GetHorizontalThumbStyle()
        {
            string css = $"width:{GetHorizontalThumbSize()}px; height:{_scrollbarWidth}px; " +
                         $"margin-left:{_horizontalThumbPosition}px;" +
                         $"border-radius:{_scrollbarCornerRadius}px; " +
                         $"background-color:{_scrollbarThumbColor.Value}; user-select:none; ";

            if (UseOverlayScrollbars)
                css += $"background-color:{_scrollbarOverlayThumbColor.Value}; ";
            else
                css += $"background-color:{_scrollbarThumbColor.Value}; ";

            return css;
        }

        private string GetContentArea()
        {
            string css = string.Empty;

            if (VerticalScrollMode != ScrollMode.Disabled)
            {
                if (VerticalGutter == ScrollbarGutter.Always)
                    css += "grid-column: 1 / span 2; ";
                else if (VerticalGutter == ScrollbarGutter.AlwaysBothEdges)
                    css += "grid-column: 2 / span 1; ";
                else if (ShowVerticalScrollBar())
                    css += "grid-column: 1 / span 2; ";
                else
                    css += "grid-column: 1 / span 3; ";
            }
            else
                css += "grid-column: 1 / span 3; ";

            if (HorizontalScrollMode != ScrollMode.Disabled)
            {
                if (HorizontalGutter == ScrollbarGutter.Always)
                    css += "grid-row: 1 / span 2; ";
                else if (HorizontalGutter == ScrollbarGutter.AlwaysBothEdges)
                    css += "grid-row: 2 / span 1; ";
                else if (ShowHorizontalScrollBar())
                    css += "grid-row: 1 / span 2; ";
                else
                    css += "grid-row: 1 / span 3; ";
            }
            else
                css += "grid-row: 1 / span 3; ";
            return css;
        }

        private string GetVerticalScrollbarStyle()
        {
            if (HorizontalScrollMode == ScrollMode.Disabled)
                return "display: grid; grid-area: 1 / 3 / span 3 / span 1; ";

            if (UseOverlayScrollbars)
                if (ShowVerticalScrollBar())
                    return "display: grid; grid-area: 1 / 3 / span 2 / span 1; ";
                else
                    return "display: grid; grid-area: 1 / 3 / span 3 / span 1; ";

            switch (HorizontalGutter)
            {
                case ScrollbarGutter.OnlyWhenOverflowed:
                    if (ShowHorizontalScrollBar())
                        return "display: grid; grid-area: 1 / 3 / span 2 / span 1; ";
                    else
                        return "display: grid; grid-area: 1 / 3 / span 3 / span 1; ";
                case ScrollbarGutter.Always:
                    return "display: grid; grid-area: 1 / 3 / span 2 / span 1; ";
                case ScrollbarGutter.AlwaysBothEdges:
                    return "display: grid; grid-area: 2 / 3 / span 1 / span 1; ";
                default:
                    return "display: grid; grid-area: 1 / 3 / span 2 / span 1; ";
            }
        }

        private string GetHorizontalScrollbarStyle()
        {
            if (VerticalScrollMode == ScrollMode.Disabled)
                return "display: grid; grid-area: 3 / 1 / span 1 / span 3; ";

            if (UseOverlayScrollbars)
                if (ShowVerticalScrollBar())
                    return "display: grid; grid-area: 3 / 1 / span 1 / span 2; ";
                else
                    return "display: grid; grid-area: 3 / 1 / span 1 / span 3; ";

            switch (VerticalGutter)
            {
                case ScrollbarGutter.OnlyWhenOverflowed:
                    if (ShowVerticalScrollBar())
                        return "display: grid; grid-area: 3 / 1 / span 1 / span 2; ";
                    else
                        return "display: grid; grid-area: 3 / 1 / span 1 / span 3; ";
                case ScrollbarGutter.Always:
                    return "display: grid; grid-area: 3 / 1 / span 1 / span 2; ";
                case ScrollbarGutter.AlwaysBothEdges:
                    return "display: grid; grid-area: 3 / 2 / span 1 / span 1; ";
                default:
                    return "display: grid; grid-area: 3 / 1 / span 1 / span 2; ";
            }
        }

        private string GetHorizontalBackgroundStyle()
        {
            if (UseOverlayScrollbars)
                return "background-color:transparent; ";

            var css = string.Empty;
            css += $"height:{_scrollbarWidth}px; " +
                   $"border-radius:{_scrollbarCornerRadius}px; " +
                   $"background-color:{_scrollbarBackgroundColor.Value}; ";
            if (_scrollbarBackgroundBoxShadowWidth > 0)
                css += $"box-shadow: inset 0 0 " +
                       $"{_scrollbarBackgroundBoxShadowWidth}px" +
                       $" {_scrollbarBackgroundBoxShadowColor.Value};";
            return css;
        }

        private string GetVerticalBackgroundStyle()
        {
            if (UseOverlayScrollbars)
                return "background-color:transparent; ";

            var css = string.Empty;
            css += $"width:{_scrollbarWidth}px; " +
                   $"border-radius:{_scrollbarCornerRadius}px; " +
                   $"background-color:{_scrollbarBackgroundColor.Value}; ";
            if (_scrollbarBackgroundBoxShadowWidth > 0)
                css += $"box-shadow: inset 0 0 " +
                       $"{_scrollbarBackgroundBoxShadowWidth}px" +
                       $" {_scrollbarBackgroundBoxShadowColor.Value};";
            return css;
        }

        private async Task OnMouseWheel(WheelEventArgs e)
        {
            if (_scrollViewerSize == null)
                return;

            if (ShowVerticalScrollBar() && VerticalScrollMode == ScrollMode.Auto ||
                                           VerticalScrollMode == ScrollMode.Enabled)
                await VerticalScroll(e.DeltaY, true);
        }

        private async Task OnMouseEnter(MouseEventArgs e)
        {
            string[] keys = {"ArrowLeft","ArrowRight","ArrowUp","ArrowDown",
                             "PageUp","PageDown","Home","End" };
            await JSRuntime.InvokeVoidAsync("EnableKeyboardCapture", _thisComponent, keys);
        }

        private async Task OnMouseLeave(MouseEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("DisableKeyboardCapture");
        }

        [JSInvokable]
        public async Task KeyDown(string key, bool shift, bool ctrl, bool alt)
        {
            if (_processingkey)
                return;
            _processingkey = true;

            try
            {
                switch (key)
                {
                    case "ArrowDown":
                        await VerticalScroll(ScrollIncrement, false);
                        break;
                    case "ArrowUp":
                        await VerticalScroll(-ScrollIncrement, false);
                        break;
                    case "ArrowRight":
                        await HorizontalScroll(ScrollIncrement, false);
                        break;
                    case "ArrowLeft":
                        await HorizontalScroll(-ScrollIncrement, false);
                        break;
                    case "PageUp":
                        await VerticalScroll(-PageIncrement, false);
                        break;
                    case "PageDown":
                        await VerticalScroll(PageIncrement, false);
                        break;
                    case "Home":
                        await VerticalScroll((int)_marginTop, false);
                        break;
                    case "End":
                        await VerticalScroll(int.MaxValue, false);
                        break;
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                // Throttle the keys just a bit
                await Task.Delay(0).ContinueWith(t =>
                {
                    _processingkey = false;
                });
            }
            await Task.CompletedTask;
        }

        [JSInvokable]
        public async Task KeyUp(string key, bool shift, bool ctrl, bool alt)
        {
            await Task.CompletedTask;
        }

        private async Task OnHorizontalMouseDown(MouseEventArgs e)
        {
            if (_overHorizontalThumb)
            {
                _horizontalMouseDown = true;
                _horizStartPos = e.OffsetX;
            }
            await JSRuntime.InvokeVoidAsync("CaptureMouse", _horizontalScrollId, 1);
        }

        private async Task OnHorizontalMouseUp(MouseEventArgs e)
        {
            _horizontalMouseDown = false;
            await JSRuntime.InvokeVoidAsync("ReleaseMouseCapture", _horizontalScrollId, 1);
        }

        private async Task OnHorizontalMouseMove(MouseEventArgs e)
        {
            if (_horizontalMouseDown)
            {
                if (_scrollableSize == null || _scrollViewerSize == null)
                    return;

                if (_processingkey)
                    return;
                if (e.MovementY == 0)
                    return;
                _processingkey = true;
                try
                {
                    _horizontalScrolling = true;
                    var position = e.OffsetX - _horizStartPos;
                    await HorizontalScrollTo(position, true);
                }
                finally
                {
                    _processingkey = false;
                }
            }
            else
                _horizontalScrolling = false;
        }

        private void OnHorizontalMouseEnter(MouseEventArgs e)
        {
            _overHorizontalThumb = true;
        }

        private void OnHorizontalMouseLeave(MouseEventArgs e)
        {
            _overHorizontalThumb = false;
        }

        private async Task OnHorizontalScrollbarClick(MouseEventArgs e)
        {
            if (_overHorizontalThumb || _horizontalScrolling)
                return;

            var thumbSize = GetHorizontalThumbSize();
            var position = _horizontalThumbPosition;

            if (e.OffsetX > position + thumbSize)
                await HorizontalScroll(PageIncrement, true);
            else if (e.OffsetX < position)
                await HorizontalScroll(-PageIncrement, true);
        }

        private async Task OnVerticalMouseDown(MouseEventArgs e)
        {
            if (_overVerticalThumb)
            {
                _verticalMouseDown = true;
                _vertStartPos = e.OffsetY;
                await JSRuntime.InvokeVoidAsync("CaptureMouse", _verticalScrollId, 1);
            }
        }

        private async Task OnVerticalMouseUp(MouseEventArgs e)
        {
            _verticalMouseDown = false;
            await JSRuntime.InvokeVoidAsync("ReleaseMouseCapture", _verticalScrollId, 1);
        }

        private async Task OnVerticalMouseMove(MouseEventArgs e)
        {
            if (_verticalMouseDown)
            {
                if (_scrollableSize == null || _scrollViewerSize == null)
                    return;

                if (_processingkey)
                    return;
                if (e.MovementY == 0)
                    return;
                _processingkey = true;
                try
                {
                    _verticalScrolling = true;
                    var position = e.OffsetY - _vertStartPos;
                    await VerticalScrollTo(position, true);
                }
                finally
                {
                    _processingkey = false;
                }
            }
            else
                _verticalScrolling = false;
        }

        private void OnVerticalMouseEnter(MouseEventArgs e)
        {
            _overVerticalThumb = true;
            StateHasChanged();
        }

        private void OnVerticalMouseLeave(MouseEventArgs e)
        {
            _overVerticalThumb = false;
            StateHasChanged();
        }

        private async Task OnVerticalScrollbarClick(MouseEventArgs e)
        {
            if (_overVerticalThumb || _verticalScrolling)
                return;

            var thumbSize = GetVerticalThumbSize();
            var position = _verticalThumbPosition;

            if (e.OffsetY > position + thumbSize)
                await VerticalScroll(PageIncrement, true);
            else if (e.OffsetY < position)
                await VerticalScroll(-PageIncrement, true);
        }

        internal async Task NotifyObservedSizes(List<ObservedSize> observedSizes)
        {
            if (observedSizes == null)
                return;

            bool changed = false;
            foreach (var observedSize in observedSizes)
            {
                if (observedSize.TargetId == _scrollId)
                {
                    if (observedSize.ElementHeight > 0 &&
                        (_scrollableSize == null ||
                         _scrollableSize.ElementHeight != observedSize.ElementHeight ||
                         _scrollableSize.ElementWidth != observedSize.ElementWidth))
                    {
                        _scrollableSize = observedSize;
                        changed = true;
                    }
                }
                else if (observedSize.TargetId == _scrollViewerId)
                {
                    if (observedSize.ElementHeight > 0 &&
                        (_scrollViewerSize == null ||
                         _scrollViewerSize.ElementHeight != observedSize.ElementHeight ||
                         _scrollViewerSize.ElementWidth != observedSize.ElementWidth))
                    {
                        _scrollViewerSize = observedSize;
                        changed = true;
                    }
                }
            }
            if (changed)
                StateHasChanged();
            await Task.CompletedTask;
        }

        private bool SetVerticalThumbPosition(double position)
        {
            if (_scrollViewerSize == null)
                return false;

            Console.WriteLine($"SetVerticalThumbPosition: position:{ position}");
            var ht = _scrollViewerSize.ElementHeight - GetVerticalThumbSize();
            if (position <= 0 && _verticalThumbPosition >= 0)
            {
                _verticalThumbPosition = 0;
                Console.WriteLine($"SetVerticalThumbPosition1: position:{position}");
                return true;
            }

            if (position >= ht && _verticalThumbPosition <= ht)
            {
                _verticalThumbPosition = ht;
                Console.WriteLine($"SetVerticalThumbPosition2: position:{position} pos:{ht}");
                return true;
            }

            _verticalThumbPosition = position;
            Console.WriteLine($"SetVerticalThumbPosition3: position:{position}");
            return true;
        }

        private bool SetHorizontalThumbPosition(double position)
        {
            if (_scrollViewerSize == null)
                return false;

            var width = _scrollViewerSize.ElementWidth - GetHorizontalThumbSize();
            if (position <= 0 && _horizontalThumbPosition >= 0)
            {
                _horizontalThumbPosition = 0;
                return true;
            }

            if (position >= width && _horizontalThumbPosition <= width)
            {
                _horizontalThumbPosition = width;
                return true;
            }

            _horizontalThumbPosition = position;
            return true;
        }

        private double GetVerticalFactor()
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return 1;
            
            return (_scrollableSize.ElementHeight - _scrollViewerSize.ElementHeight) /
                   (_scrollViewerSize.ElementHeight - GetVerticalThumbSize());
        }

        private double GetHorizontalFactor()
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return 1;

            return (_scrollableSize.ElementWidth - _scrollViewerSize.ElementWidth) /
                   (_scrollViewerSize.ElementWidth - GetHorizontalThumbSize());
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