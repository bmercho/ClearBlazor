using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;

namespace ClearBlazor
{
    public partial class Popup : ClearComponentBase, IDisposable, IObserver<BrowserSizeInfo>, IObserver<bool>
    {
        [Parameter]
        public bool UseTransition { get; set; } = true;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public bool Open { get; set; } = false;

        [Parameter]
        public bool CloseOnOutsideClick { get; set; } = true;

        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;

        [Parameter]
        public bool AllowHorizontalFlip { get; set; } = true;

        [Parameter]
        public EventCallback<bool> OpenChanged { get; set; }

        [Parameter]
        public string? Text { get; set; } = null;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomCentre;

        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopCentre;

        [Parameter]
        public int? Delay { get; set; } = null; // Milliseconds

        private ElementReference PopupElement;

        private SizeInfo? SizeInfo = null;
        private bool _mouseOver = false;
        private IDisposable ScrollViewUnsubscriber;
        private IDisposable unsubscriber;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Subscribe(BrowserSizeService.Instance);
            ScrollViewer.Subscribe(this);
        }

        public virtual void Subscribe(IObservable<BrowserSizeInfo> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void OnNext(BrowserSizeInfo browserSizeInfo)
        {
            Open = false;
            OpenChanged.InvokeAsync(Open);
            StateHasChanged();
        }

        public virtual void OnCompleted()
        {
        }

        public virtual void OnError(Exception error)
        {
        }

        public virtual void OnNext(bool hasScrolled)
        {
            Open = false;
            OpenChanged.InvokeAsync(Open);
            StateHasChanged();
        }

        public virtual void Subscribe(IObservable<bool> provider)
        {
            ScrollViewUnsubscriber = provider.Subscribe(this);
        }

        public override void Dispose()
        {
            unsubscriber?.Dispose();
            ScrollViewUnsubscriber?.Dispose();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await JSRuntime.InvokeVoidAsync("window.clearBlazor.popup.initialize",
                                                DotNetObjectReference.Create(this));
            SizeInfo? existing = null;
            if (SizeInfo != null)
                existing = SizeInfo;
            SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", PopupElement);
            if (existing == null ||
                !existing.Equals(SizeInfo))
                StateHasChanged();
        }

        protected override string UpdateStyle(string css)
        {
            css += $"color: {ThemeManager.CurrentPalette.ToolTipTextColor.Value}; ";
            css += "z-index:100;";
            if (UseTransition)
                css += "transition: opacity .2s ease-in-out; ";
            css += "display: grid; ";
            css += Open ? "opacity: 1; " : "opacity: 0; ";
            css += GetLocationCss(Position, Transform);
            css += GetFontSize();
            if (SizeInfo != null)
                css += $"clip-path: rect({0}px {SizeInfo.WindowWidth - 10}px {SizeInfo.WindowHeight - 10}px {0}px); ";
            css += "white-space:pre; text-align:justify; ";
            return css;
        }

        private string GetFontSize()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return $"font-size: {ThemeManager.CurrentTheme.Typography.InputVerySmall.FontSize}; ";
                case Size.Small:
                    return $"font-size: {ThemeManager.CurrentTheme.Typography.InputSmall.FontSize}; ";
                case Size.Normal:
                    return $"font-size: {ThemeManager.CurrentTheme.Typography.InputNormal.FontSize}; ";
                case Size.Large:
                    return $"font-size: {ThemeManager.CurrentTheme.Typography.InputLarge.FontSize}; ";
                case Size.VeryLarge:
                    return $"font-size: {ThemeManager.CurrentTheme.Typography.InputVeryLarge.FontSize}; ";
            }
            return $"font-size: {ThemeManager.CurrentTheme.Typography.InputNormal.FontSize}; ";
        }

        private string GetLocationCss(PopupPosition position, PopupTransform transform)
        {
            (double x, double y) = GetXYPosition(position, transform);

            (PopupPosition newPosition, PopupTransform newTransform) = AdjustPosition(position, transform, x, y);

            if (newPosition != position)
                (x, y) = GetXYPosition(newPosition, newTransform);

            return $"position: absolute; top: {y}px; left: {x}px; ";
        }

        private (PopupPosition, PopupTransform) AdjustPosition(PopupPosition position, PopupTransform transform,
                                                              double x, double y)
        {
            PopupPosition newPosition = position;
            PopupTransform newTransform = transform;
            if (SizeInfo != null)
            {
                switch (position)
                {
                    case PopupPosition.TopLeft:
                        (newPosition, newTransform) = CheckVerticalFlip(newPosition, newTransform, true,
                                                                        y, PopupPosition.BottomLeft);
                        (newPosition, newTransform) = CheckHorizontalFlip(newPosition, newTransform, true,
                                                                          x, PopupPosition.TopRight);
                        break;
                    case PopupPosition.TopCentre:
                        (newPosition, newTransform) = CheckVerticalFlip(newPosition, newTransform, true,
                                                                        y, PopupPosition.BottomCentre);
                        break;
                    case PopupPosition.TopRight:
                        (newPosition, newTransform) = CheckVerticalFlip(newPosition, newTransform, true,
                                                                        y, PopupPosition.BottomRight);
                        (newPosition, newTransform) = CheckHorizontalFlip(newPosition, newTransform, false,
                                                                          x, PopupPosition.TopLeft);
                        break;

                    case PopupPosition.CentreLeft:
                        (newPosition, newTransform) = CheckHorizontalFlip(newPosition, newTransform, true,
                                                                          x, PopupPosition.CentreRight);
                        break;
                    case PopupPosition.CentreCentre:
                        break;
                    case PopupPosition.CentreRight:
                        (newPosition, newTransform) = CheckHorizontalFlip(newPosition, newTransform, false,
                                                                          x, PopupPosition.CentreLeft);
                        break;

                    case PopupPosition.BottomLeft:
                        (newPosition, newTransform) = CheckVerticalFlip(newPosition, newTransform, false,
                                                                        y, PopupPosition.TopLeft);
                        (newPosition, newTransform) = CheckHorizontalFlip(newPosition, newTransform, true,
                                                                          x, PopupPosition.BottomRight);
                        break;
                    case PopupPosition.BottomCentre:
                        (newPosition, newTransform) = CheckVerticalFlip(newPosition, newTransform, false,
                                                                        y, PopupPosition.TopCentre);
                        break;
                    case PopupPosition.BottomRight:
                        (newPosition, newTransform) = CheckVerticalFlip(newPosition, newTransform, false,
                                                                        y, PopupPosition.TopRight);
                        (newPosition, newTransform) = CheckHorizontalFlip(newPosition, newTransform, false,
                                                                          x, PopupPosition.BottomLeft);
                        break;
                }
            }
            return (newPosition, newTransform);
        }

        private (PopupPosition, PopupTransform) CheckVerticalFlip(PopupPosition position,
                                                                  PopupTransform transform,
                                                                  bool top,
                                                                  double y, PopupPosition newPosition)
        {
            if (!AllowVerticalFlip)
                return (position, transform);

            var topOverflow = IsTopOverflow(transform, top, y);
            var bottomOverflow = IsBottomOverflow(transform, top, y);
            if (topOverflow && bottomOverflow)
                return (position, transform);
            if (top && topOverflow)
                return (newPosition, InvertVertically(transform));
            if (!top && bottomOverflow)
                return (newPosition, InvertVertically(transform));
            return (position, transform);
        }

        private (PopupPosition, PopupTransform) CheckHorizontalFlip(PopupPosition position,
                                                                    PopupTransform transform,
                                                                    bool left,
                                                                    double x, PopupPosition newPosition)
        {
            if (!AllowHorizontalFlip)
                return (position, transform);

            var leftOverflow = IsLeftOverflow(transform, left, x);
            var rightOverflow = IsRightOverflow(transform, left, x);
            if (leftOverflow && rightOverflow)
                return (position, transform);
            if (left && leftOverflow)
                return (newPosition, InvertHorizontally(transform));
            if (!left && rightOverflow)
                return (newPosition, InvertHorizontally(transform));
            return (position, transform);
        }

        private bool IsTopOverflow(PopupTransform transform, bool top, double y)
        {
            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.TopCentre:
                case PopupTransform.TopRight:
                    if (top && y < 0)
                        return true;
                    if (!top && y - SizeInfo!.ElementHeight - SizeInfo.ParentHeight < 0)
                        return true;
                    break;
                case PopupTransform.CentreLeft:
                case PopupTransform.CentreCentre:
                case PopupTransform.CentreRight:
                    if (top && y < 0)
                        return true;
                    if (!top && y - SizeInfo!.ElementHeight / 2 + SizeInfo.ParentHeight < 0)
                        return true;
                    break;
                case PopupTransform.BottomLeft:
                case PopupTransform.BottomCentre:
                case PopupTransform.BottomRight:
                    if (top && y < 0)
                        return true;
                    if (!top && y - SizeInfo!.ParentHeight < 0)
                        return true;
                    break;
            }
            return false;
        }

        private bool IsBottomOverflow(PopupTransform transform, bool top, double y)
        {
            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.TopCentre:
                case PopupTransform.TopRight:
                    if (top && y + 2 * SizeInfo!.ElementHeight + SizeInfo.ParentHeight > SizeInfo.WindowHeight)
                        return true;
                    if (!top && y + SizeInfo!.ElementHeight > SizeInfo.WindowHeight)
                        return true;
                    break;
                case PopupTransform.CentreLeft:
                case PopupTransform.CentreCentre:
                case PopupTransform.CentreRight:
                    if (top && y + SizeInfo!.ElementHeight + SizeInfo.ParentHeight > SizeInfo.WindowHeight)
                        return true;
                    if (!top && y + SizeInfo!.ElementHeight / 2 > SizeInfo.WindowHeight)
                        return true;
                    break;
                case PopupTransform.BottomLeft:
                case PopupTransform.BottomCentre:
                case PopupTransform.BottomRight:
                    if (top && y + SizeInfo!.ParentHeight > SizeInfo.WindowHeight)
                        return true;
                    if (!top && y > SizeInfo!.WindowHeight)
                        return true;
                    break;
            }
            return false;
        }

        private bool IsLeftOverflow(PopupTransform transform, bool left, double x)
        {
            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.CentreLeft:
                case PopupTransform.BottomLeft:
                    if (left && x < 0)
                        return true;
                    if (!left && x - SizeInfo!.ElementWidth - SizeInfo.ParentWidth < 0)
                        return true;
                    break;
                case PopupTransform.TopCentre:
                case PopupTransform.CentreCentre:
                case PopupTransform.BottomCentre:
                    if (left && x < 0)
                        return true;
                    if (!left && x - SizeInfo!.ElementWidth / 2 + SizeInfo.ParentWidth < 0)
                        return true;
                    break;
                case PopupTransform.TopRight:
                case PopupTransform.CentreRight:
                case PopupTransform.BottomRight:
                    if (left && x < 0)
                        return true;
                    if (!left && x - SizeInfo!.ParentWidth < 0)
                        return true;
                    break;
            }
            return false;
        }

        private bool IsRightOverflow(PopupTransform transform, bool left, double x)
        {
            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.CentreLeft:
                case PopupTransform.BottomLeft:
                    if (left && x + 2 * SizeInfo!.ElementWidth + SizeInfo.ParentWidth > SizeInfo.WindowWidth)
                        return true;
                    if (!left && x + SizeInfo!.ElementWidth > SizeInfo.WindowWidth)
                        return true;
                    break;
                case PopupTransform.TopCentre:
                case PopupTransform.CentreCentre:
                case PopupTransform.BottomCentre:
                    if (left && x + SizeInfo!.ElementWidth + SizeInfo.ParentWidth > SizeInfo.WindowWidth)
                        return true;
                    if (!left && x + SizeInfo!.ElementWidth / 2 > SizeInfo.WindowWidth)
                        return true;
                    break;
                case PopupTransform.TopRight:
                case PopupTransform.CentreRight:
                case PopupTransform.BottomRight:
                    if (left && x + SizeInfo!.ParentWidth > SizeInfo.WindowWidth)
                        return true;
                    if (!left && x > SizeInfo!.WindowWidth)
                        return true;
                    break;
            }
            return false;
        }

        private PopupTransform InvertVertically(PopupTransform transform)
        {
            switch (transform)
            {
                case PopupTransform.TopLeft:
                    return PopupTransform.BottomLeft;
                case PopupTransform.TopCentre:
                    return PopupTransform.BottomCentre;
                case PopupTransform.TopRight:
                    return PopupTransform.BottomRight;
                case PopupTransform.CentreLeft:
                    return transform;
                case PopupTransform.CentreCentre:
                    return transform;
                case PopupTransform.CentreRight:
                    return transform;
                case PopupTransform.BottomLeft:
                    return PopupTransform.TopLeft;
                case PopupTransform.BottomCentre:
                    return PopupTransform.TopCentre;
                case PopupTransform.BottomRight:
                    return PopupTransform.TopRight;
            }
            return transform;
        }

        private PopupTransform InvertHorizontally(PopupTransform transform)
        {
            switch (transform)
            {
                case PopupTransform.TopLeft:
                    return PopupTransform.TopRight;
                case PopupTransform.TopCentre:
                    return transform;
                case PopupTransform.TopRight:
                    return PopupTransform.TopLeft;
                case PopupTransform.CentreLeft:
                    return PopupTransform.CentreRight;
                case PopupTransform.CentreCentre:
                    return transform;
                case PopupTransform.CentreRight:
                    return PopupTransform.CentreLeft;
                case PopupTransform.BottomLeft:
                    return PopupTransform.BottomRight;
                case PopupTransform.BottomCentre:
                    return transform;
                case PopupTransform.BottomRight:
                    return PopupTransform.BottomLeft;
            }
            return transform;
        }

        private (double x, double y) GetXYPosition(PopupPosition? position, PopupTransform transform)
        {
            if (SizeInfo == null || position == null)
                return (0, 0);

            double x = 0;
            double y = 0;
            switch (position)
            {
                case PopupPosition.TopLeft:
                    {
                        x = SizeInfo.ParentX;
                        y = SizeInfo.ParentY;
                        break;
                    }
                case PopupPosition.TopCentre:
                    {
                        x = SizeInfo.ParentX + SizeInfo.ParentWidth / 2;
                        y = SizeInfo.ParentY;
                        break;
                    }
                case PopupPosition.TopRight:
                    {
                        x = SizeInfo.ParentX + SizeInfo.ParentWidth;
                        y = SizeInfo.ParentY;
                        break;
                    }

                case PopupPosition.CentreLeft:
                    {
                        x = SizeInfo.ParentX;
                        y = SizeInfo.ParentY + SizeInfo.ParentHeight / 2;
                        break;
                    }
                case PopupPosition.CentreCentre:
                    {
                        x = SizeInfo.ParentX + SizeInfo.ParentWidth / 2;
                        y = SizeInfo.ParentY + SizeInfo.ParentHeight / 2;
                        break;
                    }
                case PopupPosition.CentreRight:
                    {
                        x = SizeInfo.ParentX + SizeInfo.ParentWidth;
                        y = SizeInfo.ParentY + SizeInfo.ParentHeight / 2;
                        break;
                    }

                case PopupPosition.BottomLeft:
                    {
                        x = SizeInfo.ParentX;
                        y = SizeInfo.ParentY + SizeInfo.ParentHeight;
                        break;
                    }
                case PopupPosition.BottomCentre:
                    {
                        x = SizeInfo.ParentX + SizeInfo.ParentWidth / 2;
                        y = SizeInfo.ParentY + SizeInfo.ParentHeight;
                        break;
                    }
                case PopupPosition.BottomRight:
                    {
                        x = SizeInfo.ParentX + SizeInfo.ParentWidth;
                        y = SizeInfo.ParentY + SizeInfo.ParentHeight;
                        break;
                    }
            }
            switch (transform)
            {
                case PopupTransform.TopLeft:
                    break;
                case PopupTransform.TopCentre:
                    {
                        x += -SizeInfo.ElementWidth / 2;
                        break;
                    }
                case PopupTransform.TopRight:
                    {
                        x += -SizeInfo.ElementWidth;
                        break;
                    }

                case PopupTransform.CentreLeft:
                    {
                        y += -SizeInfo.ElementHeight / 2;
                        break;
                    }
                case PopupTransform.CentreCentre:
                    {
                        x += -SizeInfo.ElementWidth / 2;
                        y += -SizeInfo.ElementHeight / 2;
                        break;
                    }
                case PopupTransform.CentreRight:
                    {
                        x += -SizeInfo.ElementWidth;
                        y += -SizeInfo.ElementHeight / 2;
                        break;
                    }

                case PopupTransform.BottomLeft:
                    {
                        y += -SizeInfo.ElementHeight;
                        break;
                    }
                case PopupTransform.BottomCentre:
                    {
                        x += -SizeInfo.ElementWidth / 2;
                        y += -SizeInfo.ElementHeight;
                        break;
                    }
                case PopupTransform.BottomRight:
                    {
                        x += -SizeInfo.ElementWidth;
                        y += -SizeInfo.ElementHeight;
                        break;
                    }
            }

            return (x, y);
        }

        protected void OnMouseEnter(MouseEventArgs e)
        {
            _mouseOver = true;
        }

        protected void OnMouseLeave(MouseEventArgs e)
        {
            _mouseOver = false;
        }

        [JSInvokable]
        public async Task MouseDown()
        {
            if (!CloseOnOutsideClick)
                return;

            if (!_mouseOver && Open)
            {
                Open = false;
                await OpenChanged.InvokeAsync(Open);
                StateHasChanged();
            }
        }
    }
}