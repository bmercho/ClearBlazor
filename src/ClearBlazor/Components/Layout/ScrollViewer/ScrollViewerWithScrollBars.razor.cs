using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

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
        private DotNetObjectReference<ScrollViewerWithScrollBars> _thisComponent = null!;
        private static List<IObserver<bool>> _observers = new List<IObserver<bool>>();
        private double _marginTop = 0;
        private double _marginLeft = 0;
        private string _scrollId = Guid.NewGuid().ToString();
        private string _verticalThumbId = Guid.NewGuid().ToString();
        private string _horizontalThumbId = Guid.NewGuid().ToString();
        private string _verticalScrollId = Guid.NewGuid().ToString();
        private string _horizontalScrollId = Guid.NewGuid().ToString();
        private SizeInfo? _scrollableSize = null;
        private SizeInfo? _scrollViewerSize = null;
        SizeInfo? _previousScrollerViewerSize = null;
        private ElementReference _scrollElement;
        private ElementReference _scrollViewerElement;
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

        public static IDisposable Subscribe(IObserver<bool> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }

        public async Task SetScrollTop(int top)
        {
            _marginTop = ConstrainMarginTop(-top);
            await DoVerticalScrolling();
        }

        public async Task SetScrollLeft(int left)
        {
            _marginLeft = ConstrainMarginLeft(-left);
            await DoVerticalScrolling();
        }

        public async Task ScrollToStart(ScrollDirection scrollDirection)
        {
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                _marginLeft = 0;
                await DoHorizontalScrolling();
            }
            else
            {
                _marginTop = 0;
                await DoVerticalScrolling();
            }
        }

        public async Task ScrollToEnd(ScrollDirection scrollDirection)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;
            
            if (scrollDirection == ScrollDirection.Horizontal)
            {
                _marginLeft = -_scrollableSize.ElementWidth + _scrollViewerSize.ElementWidth;
                await DoHorizontalScrolling();
            }
            else
            {
                _marginTop = -_scrollableSize.ElementHeight + _scrollViewerSize.ElementHeight;
                await DoVerticalScrolling();
            }
        }

        public async Task ScrollIntoView(string id, Alignment alignment)
        {
            if (_scrollViewerSize == null)
            {
                _pendingScrollIntoViewId = id;
                _pendingScrollIntoViewAlignment = alignment;
                return;
            }

            var componentSize = await JSRuntime.InvokeAsync<SizeInfo>("GetSizeInfo", id);

            switch (alignment)
            {
                case Alignment.Stretch:
                case Alignment.Center:
                    _marginTop -= componentSize.ElementY - _scrollViewerSize.ElementY -
                                 _scrollViewerSize.ElementHeight / 2 +
                                 componentSize.ElementHeight / 2;
                    _marginTop = ConstrainMarginTop(_marginTop);
                    break;
                case Alignment.Start:
                    _marginTop = 0;
                    break;
                case Alignment.End:
                    if (_scrollableSize != null)
                        _marginTop = -_scrollableSize.ElementHeight + _scrollViewerSize.ElementHeight;
                    break;
            }
            await DoVerticalScrolling();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            _scrollbarWidth = ThemeManager.CurrentTheme.ScrollbarWidth;
            _scrollbarBackgroundBoxShadowWidth = ThemeManager.CurrentTheme.ScrollbarBackgroundBoxShadowWidth;
            _scrollbarCornerRadius = ThemeManager.CurrentTheme.ScrollbarCornerRadius;
            _scrollbarBackgroundColor = ThemeManager.CurrentPalette.ScrollbarBackgroundColor;
            _scrollbarThumbColor = ThemeManager.CurrentPalette.ScrollbarThumbColor;
            _scrollbarBackgroundBoxShadowColor = ThemeManager.CurrentPalette.ScrollbarBackgroundBoxShadowColor;
            _scrollbarOverlayThumbColor = ThemeManager.CurrentPalette.ScrollbarOverlayThumbColor;
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
            }

            _scrollableSize = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", _scrollElement);
            _scrollViewerSize = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", _scrollViewerElement);

            if (_previousScrollerViewerSize == null || !_previousScrollerViewerSize.Equals(_scrollableSize))
            {
                StateHasChanged();
                _previousScrollerViewerSize = _scrollableSize;
            }

            if (_pendingScrollIntoViewId != null)
            {
                await ScrollIntoView(_pendingScrollIntoViewId, _pendingScrollIntoViewAlignment);
                _pendingScrollIntoViewId = null;
            }

        }

        private async Task VerticalScroll(int verticalAmount)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;

            bool doScroll = false;
            if (ShowVerticalScrollBar())
            {
                var top = _marginTop - verticalAmount;
                top = ConstrainMarginTop(top);
                if (top != _marginTop)
                {
                    _marginTop = top;
                    doScroll = true;
                }
            }

            if (doScroll)
                await DoVerticalScrolling();
        }

        private async Task HorizontalScroll(int amount)
        {
            if (_scrollViewerSize == null || _scrollableSize == null)
                return;

            bool doScroll = false;
            if (ShowHorizontalScrollBar())
            {
                var left = _marginLeft - amount;
                left = ConstrainMarginLeft(left);
                if (left != _marginLeft)
                {
                    _marginLeft = left;
                    doScroll = true;
                }
            }

            if (doScroll)
                await DoHorizontalScrolling();
        }

        private async Task DoVerticalScrolling()
        {
            await JSRuntime.InvokeVoidAsync("SetStyleProperty", _scrollId, "margin-top", $"{_marginTop}px");
            await JSRuntime.InvokeVoidAsync("SetStyleProperty", _verticalThumbId,
                                "margin-top", $"{GetVerticalThumbPosition()}px");
            await OnScroll();
        }

        private async Task DoHorizontalScrolling()
        {
            await JSRuntime.InvokeVoidAsync("SetStyleProperties",
                                           _scrollId, "margin-left", $"{_marginLeft}px",
                                           _horizontalThumbId, "margin-left",
                                           $"{GetHorizontalThumbPosition()}px");
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

            await ScrollCallback(new ScrollState
            {
                ScrollTop = Math.Abs(_marginTop),
                ScrollLeft = Math.Abs(_marginLeft),
                ScrollHeight = _scrollableSize.ElementHeight,
                ScrollWidth = _scrollableSize.ElementWidth,
                ClientHeight = _scrollViewerSize.ElementHeight,
                ClientWidth = _scrollViewerSize.ElementWidth
            });
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

            return (_scrollViewerSize.ElementHeight / _scrollableSize.ElementHeight) * viewerHeight;
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

            return (_scrollViewerSize.ElementWidth / _scrollableSize.ElementWidth) * viewerWidth;
        }

        private double GetVerticalThumbPosition()
        {
            if (_scrollableSize == null || _scrollViewerSize == null)
                return 0;

            double viewerHeight = _scrollViewerSize.ElementHeight;
            if (UseOverlayScrollbars)
                viewerHeight -= _scrollbarWidth;

            return -_marginTop / (_scrollableSize.ElementHeight - _scrollViewerSize.ElementHeight) *
                   (viewerHeight - GetVerticalThumbSize() - 1);
        }
        private double GetHorizontalThumbPosition()
        {
            if (_scrollableSize == null || _scrollViewerSize == null)
                return 0;

            double viewerWidth = _scrollViewerSize.ElementWidth;
            if (UseOverlayScrollbars)
                viewerWidth -= _scrollbarWidth;

            return -_marginLeft / (_scrollableSize.ElementWidth - _scrollViewerSize.ElementWidth) *
                   (viewerWidth - GetHorizontalThumbSize() - 1);
        }

        private string GetVerticalThumbStyle()
        {
            string css = $"height:{GetVerticalThumbSize()}px; width:{_scrollbarWidth}px; " +
                         $"margin-top:{GetVerticalThumbPosition()}px; " +
                         $"border-radius:{_scrollbarCornerRadius}px; user-select:none; ";
            if (UseOverlayScrollbars)
                css += $"background-color:{_scrollbarOverlayThumbColor.Value}; ";
            else
                css += $"background-color:{_scrollbarThumbColor.Value}; ";

            return css;
        }

        private string GetHorizontalThumbStyle()
        {
            string css = $"width:{GetHorizontalThumbSize()}px; height:{_scrollbarWidth}px; " +
                         $"margin-left:{GetHorizontalThumbPosition()}px;" +
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
            if (ShowVerticalScrollBar() && VerticalScrollMode == ScrollMode.Auto || 
                                           VerticalScrollMode == ScrollMode.Enabled)
            {
                _marginTop -= e.DeltaY;
                _marginTop = ConstrainMarginTop(_marginTop);
                await DoVerticalScrolling();
            }
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
            switch(key)
            {
                case "ArrowDown":
                    await VerticalScroll(ScrollIncrement);
                    break;
                case "ArrowUp":
                    await VerticalScroll(-ScrollIncrement);
                    break;
                case "ArrowRight":
                    await HorizontalScroll(ScrollIncrement);
                    break;
                case "ArrowLeft":
                    await HorizontalScroll(-ScrollIncrement);
                    break;
                case "PageUp":
                    await VerticalScroll(-PageIncrement);
                    break;
                case "PageDown":
                    await VerticalScroll(PageIncrement);
                    break;
                case "Home":
                    await VerticalScroll((int)_marginTop);
                    break;
                case "End":
                    await VerticalScroll(int.MaxValue);
                    break;
            }
            await Task.CompletedTask;
        }

        [JSInvokable]
        public async Task KeyUp(string key, bool shift, bool ctrl, bool alt)
        {
            await Task.CompletedTask;
        }

        private double ConstrainMarginTop(double marginTop)
        {
            if (_scrollableSize == null || _scrollViewerSize == null)
                return 0;

            if (marginTop > 0)
                return 0;

            if (marginTop < -_scrollableSize.ElementHeight + _scrollViewerSize.ElementHeight)
                return -_scrollableSize.ElementHeight + _scrollViewerSize.ElementHeight;

            return marginTop;
        }

        private double ConstrainMarginLeft(double marginLeft)
        {
            if (_scrollableSize == null || _scrollViewerSize == null)
                return 0;

            if (marginLeft > 0)
                return 0;

            if (marginLeft < -_scrollableSize.ElementWidth + _scrollViewerSize.ElementWidth)
                return -_scrollableSize.ElementWidth + _scrollViewerSize.ElementWidth;

            return marginLeft;
        }

        private async Task OnHorizontalMouseDown(MouseEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("CaptureMouse", _horizontalScrollId, 1);
            _horizontalMouseDown = true;
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

                var factor = _scrollableSize.ElementWidth / _scrollViewerSize.ElementWidth;
                await HorizontalScroll((int)(e.MovementX * factor));
                _horizontalScrolling = true;
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
            var position = GetHorizontalThumbPosition();

            if (e.OffsetX > position + thumbSize)
                await HorizontalScroll(PageIncrement);
            else if (e.OffsetX < position)
                await HorizontalScroll(-PageIncrement);
        }

        private async Task OnVerticalMouseDown(MouseEventArgs e)
        {
            await JSRuntime.InvokeVoidAsync("CaptureMouse", _verticalScrollId, 1);
            _verticalMouseDown = true;
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

                var factor = _scrollableSize.ElementHeight / _scrollViewerSize.ElementHeight;
                await VerticalScroll((int)(e.MovementY * factor));
                _verticalScrolling = true;
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
            var position = GetVerticalThumbPosition();

            if (e.OffsetY > position + thumbSize)
                await VerticalScroll(PageIncrement);
            else if (e.OffsetY < position)
                await VerticalScroll(-PageIncrement);
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