using Excubo.Blazor.Canvas;
using Excubo.Blazor.Canvas.Contexts;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace ClearBlazor
{
    /// <summary>
    /// Control to select a time.
    /// </summary>
    public partial class TimePicker : InputBase, IBorder,IBackground, IBoxShadow
    {
        /// <summary>
        /// The initially selected time 
        /// </summary>
        [Parameter]
        public TimeOnly? Time { get; set; } = null;

        /// <summary>
        /// Event raised when the time selection has changed
        /// </summary>
        [Parameter]
        public EventCallback<TimeOnly?> TimeChanged { get; set; }

        /// <summary>
        /// Event raised when the minute value has been selected indicating that 
        /// the time selection has been completed
        /// </summary>
        [Parameter]
        public EventCallback MinuteSelected { get; set; }

        /// <summary>
        /// Indicates if the selection mode is 24 hours.
        /// </summary>
        [Parameter]
        public bool Hours24 { get; set; } = false;

        /// <summary>
        /// Indicates the step value as the minute handle is dragged or minute clicked  
        /// </summary>
        [Parameter]
        public MinuteStep MinuteStep { get; set; } = MinuteStep.One;

        /// <summary>
        /// The orientation of the control
        /// </summary>
        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        /// <summary>
        /// See <a href="IBackgroundApi">IBackground</a>
        /// </summary>
        [Parameter]
        public Color? BackgroundColor { get; set; } = ThemeManager.CurrentColorScheme.SurfaceContainerHighest;

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? BorderThickness { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public Color? BorderColor { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        /// <summary>
        /// See <a href="IBorderApi">IBorder</a>
        /// </summary>
        [Parameter]
        public string? CornerRadius { get; set; }

        /// <summary>
        /// See <a href="IBoxShadowApi">IBoxShadow</a>
        /// </summary>
        [Parameter]
        public int? BoxShadow { get; set; }

        public string CanvasId { get; set; } = Guid.NewGuid().ToString();

        const int PickerBodySize = 250;
        const int PickerHeaderSize = 250 + 20;
        const double PickerRadius = PickerBodySize / 2.0;
        const double PickerOuterTextRadius = PickerRadius * 0.85;
        const double PickerInnerTextRadius = PickerRadius * 0.6;

        CanvasSize? CanvasSize;
        DrawingCanvas? MyCanvas;
        private int Hour = 0;
        private int Minute = 0;
        private bool IsAM = false;
        private bool Moving = false;
        private PickerMode PickerMode = PickerMode.Hour12;
        private TimeOnly? CurrentTime;
        private bool TouchMode = false;
        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Color == null)
                Color = Color.Primary;
            if (Time == null)
                Time = new TimeOnly(0, 0);
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);

            if (firstRender)
                if (Hours24)
                    PickerMode = PickerMode.Hour24;
                else
                    PickerMode = PickerMode.Hour12;


            if (Time == null || CurrentTime != Time)
            {
                GetHourMinuteAmPmFromTime();
                CurrentTime = Time;
                StateHasChanged();
            }
        }

        protected override string UpdateStyle(string css)
        {
            if (Orientation == Orientation.Portrait)
                css += $"display:grid; width:{PickerHeaderSize}px; ";
            else
                css += $"display:grid; align-self:start; height:{PickerHeaderSize}px; ";
            return css;
        }

        internal void SetMode(PickerMode mode)
        {
            PickerMode = mode;
            MyCanvas?.RefreshCanvas();
        }

        private void CanvasSizeChange(CanvasSize size)
        {
            CanvasSize = size;
        }

        private async Task PaintCanvas(Batch2D context)
        {
            if (CanvasSize == null)
                return;

            await context.SaveAsync();
            await context.ClearRectAsync(0, 0, PickerBodySize, PickerBodySize);
            var radius = PickerRadius;
            await context.Transformations.TranslateAsync(radius, radius);
            await DrawFace(context, radius);
            await DrawHand(context);
            await DrawNumbers(context);
            await context.RestoreAsync();
        }

        private async Task DrawFace(Batch2D context, double radius)
        {
            await context.BeginPathAsync();
            await context.ArcAsync(0, 0, radius, 0, 2 * Math.PI);
            await context.FillStyleAsync(ThemeManager.CurrentColorScheme.OutlineVariant.Value);
            await context.FillAsync(FillRule.NonZero);
            await context.ClosePathAsync();

            await context.BeginPathAsync();
            await context.FillStyleAsync(Color!.Value);
            await context.ArcAsync(0, 0, 5, 0, 2 * Math.PI);
            await context.ClosePathAsync();
            await context.FillAsync(FillRule.NonZero);
        }

        private async Task DrawHand(Batch2D context)
        {
            double angle = 0;
            double endCircleSize = 0;
            double radius = PickerRadius;
            double hour = Hour;
            double minute = Minute;
            switch (PickerMode)
            {
                case PickerMode.Hour12:
                    hour = hour % 12;
                    angle = hour * Math.PI / 6;
                    endCircleSize = 13;
                    radius = PickerOuterTextRadius;
                    break;
                case PickerMode.Hour24:
                    hour = hour % 12;
                    angle = hour * Math.PI / 6;
                    endCircleSize = 13;
                    if (Hour > 12 || Hour == 0)
                        radius = PickerInnerTextRadius;
                    else
                        radius = PickerOuterTextRadius;
                    break;
                case PickerMode.Minute:
                    minute = minute % 60;
                    angle = minute * Math.PI / 30;
                    if (minute % 5 == 0)
                        endCircleSize = 13;
                    else
                        endCircleSize = 4;
                    radius = PickerOuterTextRadius;
                    break;
            }

            await DrawHand(context, angle, radius, 2, endCircleSize);
        }

        private async Task DrawHand(Batch2D context, double angle, double length, double width, double endCircleSize)
        {
            await context.SaveAsync();
            await context.StrokeStyleAsync(Color!.Value);
            await context.BeginPathAsync();
            await context.LineWidthAsync(width);
            await context.LineCapAsync(LineCap.Round);
            await context.MoveToAsync(0, 0);
            await context.RotateAsync(angle);
            await context.LineToAsync(0, -length);
            await context.StrokeAsync();
            await context.RotateAsync(-angle);
            await context.ClosePathAsync();
            await context.RestoreAsync();


            await context.SaveAsync();
            await context.BeginPathAsync();
            await context.FillStyleAsync(Color!.Value);
            await context.RotateAsync(angle);
            await context.TranslateAsync(0, -length);
            await context.ArcAsync(0, 0, endCircleSize, 0, 2 * Math.PI);
            await context.ClosePathAsync();
            await context.FillAsync(FillRule.NonZero);

            await context.RestoreAsync();
        }

        private async Task DrawNumbers(Batch2D context)
        {
            var fontFamily = ThemeManager.CurrentTheme.Typography.Default.FontFamily[0];
            await context.TextBaseLineAsync(TextBaseLine.Middle);
            await context.TextAlignAsync(TextAlign.Center);
            for (var num = 1; num < 13; num++)
            {
                await context.FontAsync($"1rem {fontFamily}");
                await context.FillStyleAsync(ThemeManager.CurrentColorScheme.OnSurface.Value);
                if (PickerMode == PickerMode.Minute)
                {
                    if (Minute % 60 == num * 5 % 60)
                        await context.FillStyleAsync(Color.ContrastingColor(ThemeManager.CurrentColorScheme.OnSurface).Value);
                }
                else
                {
                    if (Hour <= 12 && Hour != 0 && Hour % 12 == num % 12)
                        await context.FillStyleAsync(Color.ContrastingColor(ThemeManager.CurrentColorScheme.OnSurface).Value);
                }

                var ang = num * Math.PI / 6;
                await context.RotateAsync(ang);
                await context.TranslateAsync(0, -PickerOuterTextRadius);
                await context.RotateAsync(-ang);
                if (PickerMode == PickerMode.Minute)
                    await context.FillTextAsync((num * 5 % 60).ToString("D2"), 0, 2);
                else
                    await context.FillTextAsync(num.ToString(), 0, 2);

                await context.RotateAsync(ang);
                await context.TranslateAsync(0, PickerOuterTextRadius);
                await context.RotateAsync(-ang);

                if (PickerMode == PickerMode.Hour24)
                {
                    if ((Hour > 12 || Hour == 0) && Hour % 12 == num % 12)
                        await context.FillStyleAsync(Color.ContrastingColor(ThemeManager.CurrentColorScheme.OnSurface).Value);
                    else
                        await context.FillStyleAsync(ThemeManager.CurrentColorScheme.OnSurface.Value);
                    await context.FontAsync($"0.8rem {fontFamily}");
                    await context.RotateAsync(ang);
                    await context.TranslateAsync(0, -PickerInnerTextRadius);
                    await context.RotateAsync(-ang);
                    await context.FillTextAsync(((num + 12) % 24).ToString("D2"), 0, 0);
                    await context.RotateAsync(ang);
                    await context.TranslateAsync(0, PickerInnerTextRadius);
                    await context.RotateAsync(-ang);
                }
            }
        }

        private int GetPickerBodySize()
        {
            return PickerBodySize;
        }

        private void GetHourMinuteAmPmFromTime()
        {
            int prevHour = Hour;
            int prevMinute = Minute;
            if (Time == null)
            {
                Hour = 0;
                Minute = 0;
                IsAM = true;
            }
            else
            {
                var time = (TimeOnly)Time;
                IsAM = time.Hour < 12;
                if (Hours24)
                {

                    Hour = time.Hour;
                    Minute = time.Minute;
                }
                else
                {
                    Hour = time.Hour % 12;
                    if (Hour == 0)
                        Hour += 12;
                    Minute = time.Minute;
                }
            }
        }

        private Color GetHourColor()
        {
            if (PickerMode == PickerMode.Hour12 || PickerMode == PickerMode.Hour24)
                return Color.ContrastingColor(Color!);
            else
                return Color.ContrastingColor(Color!).Darken(0.3);
        }

        private Color GetMinuteColor()
        {
            if (PickerMode == PickerMode.Minute)
                return Color.ContrastingColor(Color!);
            else
                return Color.ContrastingColor(Color!).Darken(0.3);
        }

        private Color GetAMColor()
        {
            if (IsAM)
                return Color.ContrastingColor(Color!);
            else
                return Color.ContrastingColor(Color!).Darken(0.3);
        }

        private Color GetPMColor()
        {
            if (IsAM)
                return Color.ContrastingColor(Color!).Darken(0.3);
            else
                return Color.ContrastingColor(Color!);
        }

        private void OnMinuteClicked()
        {
            PickerMode = PickerMode.Minute;
            MyCanvas?.RefreshCanvas();
            StateHasChanged();
        }

        private void OnHourClicked()
        {
            if (Hours24)
                PickerMode = PickerMode.Hour24;
            else
                PickerMode = PickerMode.Hour12;
            MyCanvas?.RefreshCanvas();
            StateHasChanged();
        }

        private async Task OnAMClicked()
        {
            if (IsAM)
                return;
            IsAM = true;
            Hour = Hour % 12;
            StateHasChanged();
            await PublishTime();
        }
        private async Task OnPMClicked()
        {
            if (!IsAM)
                return;
            IsAM = false;
            StateHasChanged();
            await PublishTime();
        }

        private void OnTouchStart(CanvasTouchEventArgs e)
        {
            Moving = true;
            TouchMode = true;
        }
        private async Task OnTouchEnd(CanvasTouchEventArgs e)
        {
            Moving = false;
            HandleNewPointerPosition(e.XOffset - PickerRadius, e.YOffset - PickerRadius);
            if (PickerMode == PickerMode.Hour12 || PickerMode == PickerMode.Hour24)
                PickerMode = PickerMode.Minute;
            else
                await MinuteSelected.InvokeAsync();
            MyCanvas?.RefreshCanvas();
            StateHasChanged();
        }

        private void OnTouchMove(CanvasTouchEventArgs e)
        {
            if (Moving)
                HandleNewPointerPosition(e.XOffset - PickerRadius, e.YOffset - PickerRadius);
        }

        private async Task OnMouseDown(MouseEventArgs e)
        {
            if (TouchMode)
                return;

            Moving = true;
            await JSRuntime.InvokeVoidAsync("CaptureMouse", MyCanvas?.Id, 1);
        }

        private async Task OnMouseUp(MouseEventArgs e)
        {
            if (TouchMode)
                return;

            Moving = false;
            await JSRuntime.InvokeVoidAsync("ReleaseMouseCapture", MyCanvas?.Id, 1);
            HandleNewPointerPosition(e.OffsetX - PickerRadius, e.OffsetY - PickerRadius);
            if (PickerMode == PickerMode.Hour12 || PickerMode == PickerMode.Hour24)
                PickerMode = PickerMode.Minute;
            else
                await MinuteSelected.InvokeAsync();
            MyCanvas?.RefreshCanvas();
            StateHasChanged();
        }

        private void OnMouseMove(MouseEventArgs e)
        {
            if (TouchMode)
                return;

            if (Moving)
                HandleNewPointerPosition(e.OffsetX - PickerRadius, e.OffsetY - PickerRadius);
        }

        private async void HandleNewPointerPosition(double x, double y)
        {
            double ang = Math.Atan2(y, x) + 90 * (Math.PI / 180);
            double radius = Math.Sqrt(y * y + x * x);
            if (ang < 0)
                ang = (Math.PI / 180) * 360 + ang;
            var angd = ang * 180 / Math.PI;
            double closestDiff = 360 * Math.PI / 180;
            int closestNum = 0;
            int numSteps = GetNumSteps();
            for (var num = 1; num < numSteps + 1; num++)
            {
                var ang1 = num * 360 / numSteps * Math.PI / 180;
                var ang1d = ang1 * 180 / Math.PI;

                if (closestNum == 0 || Math.Abs(ang1 - ang) < closestDiff || Math.Abs(ang1 - ang - 360 * Math.PI / 180) < closestDiff)
                {
                    closestDiff = Math.Abs(ang1 - ang);
                    if (PickerMode != PickerMode.Minute && Hours24)
                        if (Math.Abs(radius - PickerOuterTextRadius) < Math.Abs(radius - PickerInnerTextRadius))
                            closestNum = num;
                        else
                            closestNum = (num + 12) % 24;
                    else
                        closestNum = num;
                }
            }
            if (PickerMode == PickerMode.Minute)
            {
                if (Minute != closestNum)
                {
                    Minute = (closestNum * 60 / numSteps) % 60;
                    MyCanvas?.RefreshCanvas();
                    StateHasChanged();
                    await PublishTime();
                }
            }
            else
            {
                if (Hour != closestNum)
                {
                    Hour = closestNum;
                    MyCanvas?.RefreshCanvas();
                    StateHasChanged();
                    await PublishTime();
                }
            }
        }

        private int GetNumSteps()
        {
            if (PickerMode == PickerMode.Minute)
            {
                switch (MinuteStep)
                {
                    case MinuteStep.One:
                        return 60;
                    case MinuteStep.Five:
                        return 12;
                    case MinuteStep.Ten:
                        return 6;
                    case MinuteStep.Fifteen:
                        return 4;
                }
                return 60;
            }
            else
                return 12;
        }

        private async Task PublishTime()
        {
            try
            {
                if (Hours24)
                    Time = new TimeOnly(Hour, Minute);
                else if (IsAM)
                {
                    if (Hour >= 12)
                        Time = new TimeOnly(Hour - 12, Minute);
                    else
                        Time = new TimeOnly(Hour, Minute);
                }
                else
                {
                    if (Hour < 12)
                        Time = new TimeOnly(Hour + 12, Minute);
                    else
                        Time = new TimeOnly(Hour, Minute);
                }
                await TimeChanged.InvokeAsync(Time);
            }
            catch (Exception ex)
            {
                Logger.AddLog($"TimePicker:PublishTime: {ex.Message}");
            }
        }
    }
}