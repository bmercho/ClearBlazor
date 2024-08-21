using ClearBlazor.Components.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    public partial class ToolTip : ClearComponentBase
    {
        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Text { get; set; } = null;

        [Parameter]
        public Size Size { get; set; } = Size.Normal;

        [Parameter]
        public ToolTipPosition? Position { get; set; } = null;

        [Parameter]
        public int? Delay { get; set; } = null; // Milliseconds

        private ElementReference ToolTipElement;

        private bool ToolTipVisible = false;
        private bool ToolTipHidden = true;
        private SizeInfo? SizeInfo = null;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", ToolTipElement);
        }

        protected override string UpdateStyle(string css)
        {
            css += $"color: {ThemeManager.CurrentPalette.ToolTipTextColour.Value}; ";
            css += $"background-color: {ThemeManager.CurrentPalette.ToolTipBackgroundColour.Value}; ";
            css += ToolTipVisible ? "visibility: visible; opacity: 1; " : "visibility: hidden; opacity: 0; ";
            css += GetLocationCss(Position);
            css += GetFontSize();
            css += "white-space:pre; text-align:justify; ";
            return css;
        }

        public string GetClasses()
        {
            if (Position == null)
                Position = ToolTipPosition.Bottom;

            var position = AdjustPosition(Position);

            switch (position)
            {
                case ToolTipPosition.Bottom:
                    return "tooltip tooltip-bottom";
                case ToolTipPosition.Top:
                    return "tooltip tooltip-top";
                case ToolTipPosition.Left:
                    return "tooltip tooltip-left";
                case ToolTipPosition.Right:
                    return "tooltip tooltip-right";
            }
            return "tooltip tooltip-top";
        }

        public async Task ShowToolTip()
        {
            ToolTipHidden = false;
            if (Delay == null || Delay < 0 || Delay > 2000)
                await Task.Delay(ThemeManager.CurrentTheme.ToolTipDelay);
            else
                await Task.Delay((int)Delay);
            if (!ToolTipHidden)
            {
                ToolTipVisible = true;
                StateHasChanged();
            }
        }

        public void HideToolTip()
        {
            ToolTipVisible = false;
            ToolTipHidden = true;
            StateHasChanged();
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

        private string GetLocationCss(ToolTipPosition? position)
        {
            if (position == null)
                position = ToolTipPosition.Bottom;

            position = AdjustPosition(position);

            (double x, double y) = GetXYPosition(position);
            return $"position: fixed; top: {y}px; left: {x}px; ";
        }

        private ToolTipPosition? AdjustPosition(ToolTipPosition? position)
        {
            if (SizeInfo != null)
            {
                (double x, double y) = GetXYPosition(position);

                switch (position)
                {
                    case ToolTipPosition.Bottom:
                        if (y + SizeInfo.ElementHeight > SizeInfo.WindowHeight)
                            return ToolTipPosition.Top;
                        break;
                    case ToolTipPosition.Top:
                        if (y - SizeInfo.ElementHeight < 0)
                            return ToolTipPosition.Bottom;
                        break;
                    case ToolTipPosition.Left:
                        if (x - SizeInfo.ElementHeight < 0)
                            return ToolTipPosition.Right;
                        break;
                    case ToolTipPosition.Right:
                        if (x + SizeInfo.ElementHeight > SizeInfo.WindowWidth)
                            return ToolTipPosition.Left;
                        break;
                }
            }
            return position;
        }

        private (double x, double y) GetXYPosition(ToolTipPosition? position)
        {
            if (SizeInfo == null || position == null)
                return (0, 0);

            switch (position)
            {
                case ToolTipPosition.Bottom:
                    return (SizeInfo.ParentX + SizeInfo.ParentWidth / 2 - SizeInfo.ElementWidth / 2,
                            SizeInfo.ParentY + SizeInfo.ParentHeight + 8);
                case ToolTipPosition.Top:
                    return (SizeInfo.ParentX + SizeInfo.ParentWidth / 2 - SizeInfo.ElementWidth / 2,
                            SizeInfo.ParentY - SizeInfo.ElementHeight - 8);
                case ToolTipPosition.Left:
                    return (SizeInfo.ParentX - SizeInfo.ElementWidth - 8,
                            SizeInfo.ParentY + SizeInfo.ParentHeight / 2 - SizeInfo.ElementHeight / 2);
                case ToolTipPosition.Right:
                    return (SizeInfo.ParentX + SizeInfo.ParentWidth + 8,
                            SizeInfo.ParentY + SizeInfo.ParentHeight / 2 - SizeInfo.ElementHeight / 2);
            }

            return (0, 0);
        }
    }
}