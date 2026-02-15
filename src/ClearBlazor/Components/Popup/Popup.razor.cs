using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;

namespace ClearBlazor
{
    /// <summary>
    /// A popup control that can be used to display additional information.
    /// </summary>
    public partial class Popup : ClearComponentBase, IObserver<bool>
    {
        /// <summary>
        /// The child content of this control.
        /// </summary>
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// Indicates whether to use a transition when opening or closing the popup.
        /// </summary>
        [Parameter]
        public bool UseTransition { get; set; } = true;

        /// <summary>
        /// Indicates whether the popup should close when the user clicks outside of it.
        /// </summary>
        [Parameter]
        public bool CloseOnOutsideClick { get; set; } = true;

        /// <summary>
        /// Indicates whether the popup should allow vertical flipping.
        /// </summary>
        [Parameter]
        public bool AllowVerticalFlip { get; set; } = true;

        /// <summary>
        /// Indicates whether the popup should allow horizontal flipping.
        /// </summary>
        [Parameter]
        public bool AllowHorizontalFlip { get; set; } = true;

        /// <summary>
        /// The size of the popup.
        /// </summary>
        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        /// <summary>
        /// The position of the popup.
        /// </summary>
        [Parameter]
        public PopupPosition Position { get; set; } = PopupPosition.BottomCentre;

        /// <summary>
        /// The transform of the popup.
        /// </summary>
        [Parameter]
        public PopupTransform Transform { get; set; } = PopupTransform.TopCentre;

        /// <summary>
        /// The delay before the popup is displayed. (in milliseconds)
        /// </summary>
        [Parameter]
        public int? Delay { get; set; } = null;

        private ElementReference PopupElement;

        private SizeInfo? PopupSizeInfo = null;
        private SizeInfo? ParentSizeInfo = null;
        private bool _mouseOver = false;
        private IDisposable ScrollViewUnsubscriber = null!;
        BrowserSizeService _browserSizeService = BrowserSizeService.GetInstance();
        private bool _sizedFound = false; 

        protected override void OnInitialized()
        {
            base.OnInitialized();
            _browserSizeService.OnBrowserResize += BrowserResized;
            ScrollViewer.Subscribe(this);
            _sizedFound = false;    
        }

        private async Task BrowserResized(BrowserSizeInfo browserSizeInfo)
        {
            await HidePopup();
        }

        public virtual void OnCompleted()
        {
        }

        public virtual void OnError(Exception error)
        {
        }

        public virtual void OnNext(bool hasScrolled)
        {
            Task task = HidePopup();
        }

        public virtual void Subscribe(IObservable<bool> provider)
        {
            ScrollViewUnsubscriber = provider.Subscribe(this);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
                await JSRuntime.InvokeVoidAsync("window.clearBlazor.popup.initialize",
                                                DotNetObjectReference.Create(this));
            SizeInfo? existingPopup = null;
            if (PopupSizeInfo != null)
                existingPopup = PopupSizeInfo;
            PopupSizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", PopupElement);

            SizeInfo? existingParent = null;
            if (ParentSizeInfo != null)
                existingParent = ParentSizeInfo;
            if (PopupParent != null)
                ParentSizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("GetSizeInfo", PopupParent.Id);

            if (existingPopup == null || !existingPopup.Equals(PopupSizeInfo) ||
                existingParent == null || !existingParent.Equals(ParentSizeInfo))
            {
                _sizedFound = true;
                StateHasChanged();
            }
        }

        protected override string UpdateStyle(string css)
        {
            css += "z-index:100;";
            if (UseTransition)
                css += "transition: opacity .2s ease-in-out; ";
            css += "display: grid; ";
            css += GetLocationCss(Position, Transform);
            css += GetFontSize();
            if (PopupSizeInfo != null)
                css += $"clip-path: rect({0}px {PopupSizeInfo.WindowWidth - 10}px {PopupSizeInfo.WindowHeight - 10}px {0}px); ";
            css += "white-space:pre; text-align:justify; ";
            if (!_sizedFound)
                css += $"visibility: hidden; ";
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
            if (PopupSizeInfo != null && ParentSizeInfo != null)
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
            var elementHeight = PopupSizeInfo!.ElementHeight;
            var parentHeight = ParentSizeInfo!.ElementHeight;

            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.TopCentre:
                case PopupTransform.TopRight:
                    if (top && y < 0)
                        return true;
                    if (!top && y - elementHeight - parentHeight < 0)
                        return true;
                    break;
                case PopupTransform.CentreLeft:
                case PopupTransform.CentreCentre:
                case PopupTransform.CentreRight:
                    if (top && y < 0)
                        return true;
                    if (!top && y - elementHeight / 2 + parentHeight < 0)
                        return true;
                    break;
                case PopupTransform.BottomLeft:
                case PopupTransform.BottomCentre:
                case PopupTransform.BottomRight:
                    if (top && y < 0)
                        return true;
                    if (!top && y - parentHeight < 0)
                        return true;
                    break;
            }
            return false;
        }

        private bool IsBottomOverflow(PopupTransform transform, bool top, double y)
        {
            var elementHeight = PopupSizeInfo!.ElementHeight;
            var parentHeight = ParentSizeInfo!.ElementHeight;

            var windowHeight = PopupSizeInfo!.WindowHeight;

            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.TopCentre:
                case PopupTransform.TopRight:
                    if (top && y + 2 * elementHeight + parentHeight > windowHeight)
                        return true;
                    if (!top && y + elementHeight > windowHeight)
                        return true;
                    break;
                case PopupTransform.CentreLeft:
                case PopupTransform.CentreCentre:
                case PopupTransform.CentreRight:
                    if (top && y + elementHeight + parentHeight > windowHeight)
                        return true;
                    if (!top && y + elementHeight / 2 > windowHeight)
                        return true;
                    break;
                case PopupTransform.BottomLeft:
                case PopupTransform.BottomCentre:
                case PopupTransform.BottomRight:
                    if (top && y + parentHeight > windowHeight)
                        return true;
                    if (!top && y > windowHeight)
                        return true;
                    break;
            }
            return false;
        }

        private bool IsLeftOverflow(PopupTransform transform, bool left, double x)
        {
            var elementWidth = PopupSizeInfo!.ElementWidth;
            var parentWidth = ParentSizeInfo!.ElementWidth;

            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.CentreLeft:
                case PopupTransform.BottomLeft:
                    if (left && x < 0)
                        return true;
                    if (!left && x - elementWidth - parentWidth < 0)
                        return true;
                    break;
                case PopupTransform.TopCentre:
                case PopupTransform.CentreCentre:
                case PopupTransform.BottomCentre:
                    if (left && x < 0)
                        return true;
                    if (!left && x - elementWidth / 2 + parentWidth < 0)
                        return true;
                    break;
                case PopupTransform.TopRight:
                case PopupTransform.CentreRight:
                case PopupTransform.BottomRight:
                    if (left && x < 0)
                        return true;
                    if (!left && x - parentWidth < 0)
                        return true;
                    break;
            }
            return false;
        }

        private bool IsRightOverflow(PopupTransform transform, bool left, double x)
        {
            var elementWidth = PopupSizeInfo!.ElementWidth;
            var parentWidth = ParentSizeInfo!.ElementWidth;
            var windowWidth = PopupSizeInfo!.WindowWidth;   

            switch (transform)
            {
                case PopupTransform.TopLeft:
                case PopupTransform.CentreLeft:
                case PopupTransform.BottomLeft:
                    if (left && x + 2 * elementWidth + parentWidth > windowWidth)
                        return true;
                    if (!left && x + elementWidth > windowWidth)
                        return true;
                    break;
                case PopupTransform.TopCentre:
                case PopupTransform.CentreCentre:
                case PopupTransform.BottomCentre:
                    if (left && x + elementWidth + parentWidth > windowWidth)
                        return true;
                    if (!left && x + elementWidth / 2 > windowWidth)
                        return true;
                    break;
                case PopupTransform.TopRight:
                case PopupTransform.CentreRight:
                case PopupTransform.BottomRight:
                    if (left && x + parentWidth > windowWidth)
                        return true;
                    if (!left && x > windowWidth)
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
            if (PopupSizeInfo == null || ParentSizeInfo == null || position == null)
                return (0, 0);

            var elementWidth = PopupSizeInfo!.ElementWidth;
            var parentWidth = ParentSizeInfo!.ElementWidth;
            var elementHeight = PopupSizeInfo!.ElementHeight;
            var parentHeight = ParentSizeInfo!.ElementHeight;
            var parentX = ParentSizeInfo!.ElementX;
            var parentY = ParentSizeInfo!.ElementY;


            double x = 0;
            double y = 0;
            switch (position)
            {
                case PopupPosition.TopLeft:
                    {
                        x = parentX;
                        y = parentY;
                        break;
                    }
                case PopupPosition.TopCentre:
                    {
                        x = parentX + parentWidth / 2;
                        y = parentY;
                        break;
                    }
                case PopupPosition.TopRight:
                    {
                        x = parentX + parentWidth;
                        y = parentY;
                        break;
                    }

                case PopupPosition.CentreLeft:
                    {
                        x = parentX;
                        y = parentY + parentHeight / 2;
                        break;
                    }
                case PopupPosition.CentreCentre:
                    {
                        x = parentX + parentWidth / 2;
                        y = parentY + parentHeight / 2;
                        break;
                    }
                case PopupPosition.CentreRight:
                    {
                        x = parentX + parentWidth;
                        y = parentY + parentHeight / 2;
                        break;
                    }

                case PopupPosition.BottomLeft:
                    {
                        x = parentX;
                        y = parentY + parentHeight;
                        break;
                    }
                case PopupPosition.BottomCentre:
                    {
                        x = parentX + parentWidth / 2;
                        y = parentY + parentHeight;
                        break;
                    }
                case PopupPosition.BottomRight:
                    {
                        x = parentX + parentWidth;
                        y = parentY + parentHeight;
                        break;
                    }
            }
            switch (transform)
            {
                case PopupTransform.TopLeft:
                    break;
                case PopupTransform.TopCentre:
                    {
                        x += -elementWidth / 2;
                        break;
                    }
                case PopupTransform.TopRight:
                    {
                        x += -elementWidth;
                        break;
                    }

                case PopupTransform.CentreLeft:
                    {
                        y += -elementHeight / 2;
                        break;
                    }
                case PopupTransform.CentreCentre:
                    {
                        x += -elementWidth / 2;
                        y += -elementHeight / 2;
                        break;
                    }
                case PopupTransform.CentreRight:
                    {
                        x += -elementWidth;
                        y += -elementHeight / 2;
                        break;
                    }

                case PopupTransform.BottomLeft:
                    {
                        y += -elementHeight;
                        break;
                    }
                case PopupTransform.BottomCentre:
                    {
                        x += -elementWidth / 2;
                        y += -elementHeight;
                        break;
                    }
                case PopupTransform.BottomRight:
                    {
                        x += -elementWidth;
                        y += -elementHeight;
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

            if (!_mouseOver)
            {
                await HidePopup();
            }
        }
        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            _browserSizeService.OnBrowserResize -= BrowserResized;
            ScrollViewUnsubscriber?.Dispose();
        }
    }
}