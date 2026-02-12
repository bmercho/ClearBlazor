using Microsoft.AspNetCore.Components;
using System.Data;
using System.Globalization;

namespace ClearBlazor
{
    /// <summary>
    /// Control to select a date.
    /// </summary>
    public partial class DateTimePicker : InputBase, IBorder, IBackground, IBoxShadow
    {
        public enum DatePickerMode
        {
            Year, Month, Day
        }

        /// <summary>
        /// The initial date 
        /// </summary>
        [Parameter]
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// The default date/time. This is used when the DateTime parameter is null. If this is also null, 
        /// the default date/time will be DateTime.Now. 
        /// </summary>
        [Parameter]
        public DateTime? DefaultDateTime { get; set; } = null;

        /// <summary>
        /// Event raised when the date selection has changed.Used for two way binding.
        /// </summary>
        [Parameter]
        public EventCallback<DateTime?> DateTimeChanged { get; set; }

        /// <summary>
        /// Event raised when the date/time selection has changed.
        /// </summary>
        [Parameter]
        public EventCallback DateTimeSelected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether seconds are displayed in the time representation.
        /// </summary>
        [Parameter]
        public bool ShowSeconds { get; set; } = false;

        /// <summary>
        /// Customizes what the first day of the week is. Normally either Sun or Mon.
        /// Default is Sun.
        /// </summary>
        [Parameter]
        public FirstDayOfTheWeek? FirstDayOfTheWeek { get; set; }

        /// <summary>
        /// First year available for selection
        /// </summary>
        [Parameter]
        public int? FirstYear { get; set; }

        /// <summary>
        /// Last year available for selection
        /// </summary>
        [Parameter]
        public int? LastYear { get; set; }

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
        /// The culture for the control. Affects the names of the days of the week.
        /// </summary>
        [Parameter]
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

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

        const int ControlWidthPortrait = 270;
        const int ControlHeightPortrait = 402;
        const int ControlHeightLandscape = 320;
        const int ControlWidthLandscape = 360;

        internal DateTime SelectedDateTime;
        private DateOnly? SelectedDateOnly { get; set; }
        private TimeOnly? SelectedTimeOnly { get; set; }

        private bool ShowDatePicker = true;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Color == null)
                Color = Color.Primary;
            if (DateTime == null)
                if (DefaultDateTime == null)
                    DateTime = System.DateTime.Now;
                else
                    DateTime = DefaultDateTime;

            DateTime date = (DateTime)DateTime;
            SelectedDateTime = date;
            SelectedDateOnly = DateOnly.FromDateTime(date);
            SelectedTimeOnly = TimeOnly.FromDateTime(date); 
        }

        private async Task OnToggleChanged()
        {
            ShowDatePicker = !ShowDatePicker;
            await Refresh();
        }

        protected override string UpdateStyle(string css)
        {
            css += $"display : grid; ";
            if (Orientation == Orientation.Portrait)
                css += $"height:{ControlHeightPortrait}px; " +
                       $"width:{ControlWidthPortrait}px; ";
            else
                css += $"height:{ControlHeightLandscape}px; " +
                       $"width:{ControlWidthLandscape}px;  ";
            return css;
        }

        private async Task OnChanged()
        {
            DateOnly dateOnly = SelectedDateOnly == null ? DateOnly.FromDateTime(SelectedDateTime) : (DateOnly)SelectedDateOnly;
            TimeOnly timeOnly = SelectedTimeOnly == null ? TimeOnly.FromDateTime(SelectedDateTime) : (TimeOnly)SelectedTimeOnly;


            SelectedDateTime = dateOnly.ToDateTime(timeOnly);
            DateTime = SelectedDateTime;
            await DateTimeChanged.InvokeAsync(DateTime);
        }
    }
}
