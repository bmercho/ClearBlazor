using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Numerics;

namespace ClearBlazor
{
    /// <summary>
    /// A slider component that allows users to select a value within a specified range using a thumb control.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public partial class Slider<TItem> : InputBase where TItem : struct, INumber<TItem>
    {
        /// <summary>
        /// Represents an optional value of type TItem. It can be set to null or a default value.
        /// </summary>
        [Parameter]
        public TItem? Value { get; set; } = default;

        /// <summary>
        /// Represents a callback that is invoked when the value changes. It allows for handling updates to the value in
        /// a component.
        /// </summary>
        [Parameter]
        public EventCallback<TItem> ValueChanged { get; set; }

        /// <summary>
        /// Defines a minimum value of type TItem, initialized to TItem.Zero.
        /// </summary>
        [Parameter]
        public TItem Min { get; set; } = TItem.Zero;

        /// <summary>
        /// Defines a maximum value of type TItem, initialized to a truncating value of 100. 
        /// </summary>
        [Parameter]
        public TItem Max { get; set; } = TItem.CreateTruncating(100);

        /// <summary>
        /// Represents a step value of type TItem, initialized to a truncating value of 1. It can be nullable.
        /// </summary>
        [Parameter]
        public TItem? Step { get; set; } = TItem.CreateTruncating(1);

        /// <summary>
        /// Indicates whether to track the background contrast. Defaults to false.
        /// </summary>
        [Parameter]
        public bool ContrastTrackBackground { get; set; } = false;

        [Parameter]
        public bool ShowTickMarks { get; set; } = false;

        [Parameter]
        public bool ShowTickMarkLabels { get; set; } = false;

        [Parameter]
        public bool ShowValueLabels { get; set; } = false;

        [Parameter]
        public string? TickMarkLabelFormat { get; set; } = null;

        [Parameter]
        public List<string>? TickMarkLabels { get; set; } = null;

        [Parameter]
        public string? BackgroundGradient1 { get; set; } = null;

        [Parameter]
        public string? BackgroundGradient2 { get; set; } = null;

        private Color BackgroundTrackColor = ThemeManager.CurrentColorScheme.OutlineVariant;
        private Color TrackColor = ThemeManager.CurrentColorScheme.Primary.Lighten(.3);
        private Color ThumbColor = ThemeManager.CurrentColorScheme.Primary;
        private int TrackHeight = 10;
        private double TrackWidth = 0;
        private string CornerRadius = "4";
        private int ThumbDiameter = 20;
        private string ThumbMargin = "0";
        private SizeInfo? SizeInfo = null;
        private ElementReference InputElement;
        private string ThumbElementId = Guid.NewGuid().ToString();
        private int NoOfTickMarks = 0;
        private double TickMarkSpacing = 10;
        private List<TickData> TickInfo = new List<TickData>();
        private bool DropdownOpen = false;
        private double MinDouble = 0;
        private double MaxDouble = 0;
        private double StepDouble = 0;
        private TItem? CurrentValue = default;
        private bool MouseDown = false;
        private string ValueLabel = string.Empty;
        private bool Initialising = true;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            MinDouble = (double)Convert.ChangeType(Min, typeof(double));
            MaxDouble = (double)Convert.ChangeType(Max, typeof(double));
            StepDouble = (double)Convert.ChangeType(Step!, typeof(double));
            if (Value != null)
            {
                var value = (double)Convert.ChangeType(Value, typeof(double));

                if (value > MaxDouble)
                    Value = Max;
                if (value < MinDouble)
                    Value = Min;
            }

            if (IsDisabled)
            {
                ThumbColor = ThemeManager.CurrentColorScheme.OutlineVariant;
                TrackColor = ThemeManager.CurrentColorScheme.OutlineVariant;

            }
            else if (Color != null)
            {
                ThumbColor = Color;
                if (ThemeManager.IsDarkMode)
                    TrackColor = Color.Darken(.6);
                else
                    TrackColor = Color.Lighten(.3);
            }
            else
            {
                if (ThemeManager.IsDarkMode)
                    TrackColor = ThemeManager.CurrentColorScheme.Primary.Darken(.6);
                else
                    TrackColor = ThemeManager.CurrentColorScheme.Primary.Lighten(.3);
                ThumbColor = ThemeManager.CurrentColorScheme.Primary;
            }
            if (ContrastTrackBackground)
                BackgroundTrackColor = ThemeManager.CurrentColorScheme.OutlineVariant;
            else
                BackgroundTrackColor = TrackColor;
            switch (Size)
            {
                case Size.VerySmall:
                    TrackHeight = 2;
                    CornerRadius = "1";
                    ThumbDiameter = 10;
                    break;
                case Size.Small:
                    TrackHeight = 6;
                    CornerRadius = "3";
                    ThumbDiameter = 14;
                    break;
                case Size.Normal:
                    TrackHeight = 10;
                    CornerRadius = "5";
                    ThumbDiameter = 20;
                    break;
                case Size.Large:
                    TrackHeight = 14;
                    CornerRadius = "7";
                    ThumbDiameter = 24;
                    break;
                case Size.VeryLarge:
                    TrackHeight = 18;
                    CornerRadius = "9";
                    ThumbDiameter = 30;
                    break;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);

            SizeInfo? existing = null;
            if (SizeInfo != null)
                existing = SizeInfo;
            SizeInfo = await JSRuntime.InvokeAsync<SizeInfo>("getSizeInfo", InputElement);
            if (existing == null ||
                !existing.Equals(SizeInfo) || CurrentValue != Value)
            {
                HandleValueChange();
                CalculateTickMarks();
                Initialising = false;
                StateHasChanged();
            }
        }

        private void HandleValueChange()
        {
            CurrentValue = Value;
            if (Value == null || SizeInfo == null)
                return;
            var value = (double)Convert.ChangeType(Value, typeof(double));
            if (TickMarkLabelFormat == null)
                ValueLabel = value.ToString();
            else
                ValueLabel = value.ToString(TickMarkLabelFormat);

            TrackWidth = (SizeInfo.ElementWidth - ThumbDiameter) * (value - MinDouble) / (MaxDouble - MinDouble) + ThumbDiameter / 2;
            ThumbMargin = $"{(SizeInfo.ElementWidth - ThumbDiameter) * (value - MinDouble) / (MaxDouble - MinDouble)}";
        }

        private void CalculateTickMarks()
        {

            NoOfTickMarks = (int)Math.Ceiling((MaxDouble - MinDouble + StepDouble) / StepDouble);
            if (SizeInfo != null)
                TickMarkSpacing = (SizeInfo.ElementWidth - ThumbDiameter) / (NoOfTickMarks - 1);
            TickInfo.Clear();
            for (int i = 0; i < NoOfTickMarks; i++)
            {
                bool last = i == NoOfTickMarks - 1;
                if (TickMarkLabels != null && TickMarkLabels.Count > i)
                {
                    var label = TickMarkLabels[i];
                    TickInfo.Add(new TickData(label,
                                              GetMargin(label, i, last, false),
                                              GetMargin(label, i, last, true)));
                }
                else
                {
                    var label = GetValueString(i);
                    TickInfo.Add(new TickData(label, GetMargin(label, i, last, false), GetMargin(label, i, last, true)));
                }
            }
        }

        private string GetMargin(string label, int i, bool last, bool forText)
        {
            if (forText)
            {
                return $"0,0,0,{ThumbDiameter / 2 - 1 - GetOffset(label.ToString().Length, i == 0, last) +
                                                  TickMarkSpacing * i}";
            }
            else
                return $"0,0,0,{ThumbDiameter / 2 - 1 + TickMarkSpacing * i}";
        }

        private int GetOffset(int strLength, bool first, bool last)
        {
            if (first)
                return ThumbDiameter / 2;

            int pixelsPerChar = 8;
            switch (Size)
            {
                case Size.VerySmall:
                    pixelsPerChar = 4;
                    break;
                case Size.Small:
                    pixelsPerChar = 6;
                    break;
                case Size.Normal:
                    pixelsPerChar = 8;
                    break;
                case Size.Large:
                    pixelsPerChar = 10;
                    break;
                case Size.VeryLarge:
                    pixelsPerChar = 12;
                    break;
            }

            if (last)
                return strLength * pixelsPerChar - ThumbDiameter / 2;
            else
                return strLength * pixelsPerChar / 2;

        }

        private string GetValueString(int i)
        {
            if (TickMarkLabelFormat != null)
                return (MinDouble + StepDouble * i).ToString(TickMarkLabelFormat);
            else
                return (MinDouble + StepDouble * i).ToString();
        }

        private int GetTickHeight()
        {
            switch (Size)
            {
                case Size.VerySmall:
                    return 6;
                case Size.Small:
                    return 8;
                case Size.Normal:
                    return 10;
                case Size.Large:
                    return 12;
                case Size.VeryLarge:
                    return 14;
            }
            return 10;
        }

        private string GetThumbStyle()
        {
            string css = $"height:{ThumbDiameter}px; width:{ThumbDiameter}px; " +
                         $"border-radius:{ThumbDiameter / 2}px; grid-area: 1 / 1 / span 1 / span 1; " +
                         $"background-color: {ThumbColor.Value}; margin-left:{ThumbMargin}px; ";
            return css;
        }

        private string GetValueMargin()
        {
            var offset = GetOffset(ValueLabel.Length, false, false);
            return $"{double.Parse(ThumbMargin) - offset},-21,0,0";
        }

        private async Task OnTrackClicked(MouseEventArgs e)
        {
            if (SizeInfo == null || IsDisabled || IsReadOnly)
                return;

            double value = (e.OffsetX / SizeInfo.ElementWidth) * (MaxDouble - MinDouble) + MinDouble;

            value = Math.Round(value / StepDouble) * StepDouble;
            Value = (TItem)Convert.ChangeType(value, typeof(TItem));

            HandleValueChange();

            await ValueChanged.InvokeAsync((TItem)Value);
            StateHasChanged();
        }

        private async Task OnMouseDown(MouseEventArgs e)
        {
            if (IsDisabled || IsReadOnly)
                return;

            await JSRuntime.InvokeVoidAsync("CaptureMouse", ThumbElementId, 1);

            MouseDown = true;
            DropdownOpen = true;
            StateHasChanged();
        }
        private async Task OnMouseUp(MouseEventArgs e)
        {
            MouseDown = false;
            await JSRuntime.InvokeVoidAsync("ReleaseMouseCapture", ThumbElementId, 1);
            DropdownOpen = false;
            StateHasChanged();

        }

        private async Task OnMouseMove(MouseEventArgs e)
        {
            if (MouseDown)
            {
                if (SizeInfo == null)
                    return;

                var offset = e.ClientX - SizeInfo.ElementX;
                double value = (offset / SizeInfo.ElementWidth) * (MaxDouble - MinDouble) + MinDouble;
                value = Math.Round(value / StepDouble) * StepDouble;

                if (value <= MinDouble)
                    value = MinDouble;
                if (value >= MaxDouble)
                    value = MaxDouble;

                Value = (TItem)Convert.ChangeType(value, typeof(TItem));

                HandleValueChange();

                await ValueChanged.InvokeAsync((TItem)Value);
                StateHasChanged();
            }
        }

        private class TickData
        {
            public string Text { get; set; } = string.Empty;
            public string TickMargin { get; set; } = string.Empty;
            public string TextMargin { get; set; } = string.Empty;

            public TickData(string text, string tickMargin, string textMargin)
            {
                Text = text;
                TickMargin = tickMargin;
                TextMargin = textMargin;
            }
        }
    }
}