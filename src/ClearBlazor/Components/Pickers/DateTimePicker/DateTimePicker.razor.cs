using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace ClearBlazor
{
    /// <summary>
    /// Control to select a date.
    /// </summary>
    public partial class DateTimePicker : InputBase,IBorder,IBackground, IBoxShadow
    {
        public enum DatePickerMode
        {
            Year,Month,Day
        }

        /// <summary>
        /// The initially selected date 
        /// </summary>
        [Parameter]
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Event raised when the date selection has changed.Used for two way binding.
        /// </summary>
        [Parameter]
        public EventCallback<DateTime?> DateChanged { get; set; }

        /// <summary>
        /// Event raised when the date/time selection has changed.
        /// </summary>
        [Parameter]
        public EventCallback DateTimeSelected { get; set; }

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

        private List<YearItem> YearList { get; set; } = new();

        private int? MouseOverMonth = null;
        private DayOfWeek FirstDayOfWeek = DayOfWeek.Monday;
        internal DateTime SelectedDate;

        private bool ShowDatePicker = true; 

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Color == null)
                Color = Color.Primary;
            if (DateTime == null)
                DateTime = System.DateTime.Now;
            AddYearRange(FirstYear, LastYear);

            FirstDayOfWeek = GetFirstDayOfWeek();
            DateTime date = (DateTime)DateTime;
            SelectedDate = date;
        }

        private async Task OnToggleChanged()
        {
            ShowDatePicker = !ShowDatePicker;
            await Refresh();
        }

        private int GetYearIndex()
        {
            int? year = DateTime?.Year;

            int firstYear;
            if (FirstYear == null)
                firstYear = System.DateTime.Now.AddYears(-100).Year;
            else
                firstYear = (int)FirstYear;

            if (year != null)
                return  (int)year - firstYear + 1;
            return 0;

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

        private DayOfWeek GetFirstDayOfWeek()
        {
            switch (FirstDayOfTheWeek)
            {
                case ClearBlazor.FirstDayOfTheWeek.Monday:
                    return DayOfWeek.Monday;
                case ClearBlazor.FirstDayOfTheWeek.Sunday:
                    return DayOfWeek.Sunday;
                case null:
                    return Culture.DateTimeFormat.FirstDayOfWeek;
            }
            return DayOfWeek.Sunday;
        }

        private string GetYearSize(int year)
        {
            if (year == SelectedDate.Year)
                return "22px";

            return "16px";
        }

        private Color GetYearColor(int year)
        {
            if (year == SelectedDate.Year)
                return Color!;
            else
                return ThemeManager.CurrentColorScheme.OnSurface;
        }

        private Color GetMonthColor(int month)
        {
            if (month == SelectedDate.Month)
                return Color!;
            else
                return ThemeManager.CurrentColorScheme.OnSurface;
        }

        private Color GetDayColor(int dayIndex)
        {
            if (DateTime == null)
                return ThemeManager.CurrentColorScheme.OnSurface;

            DateTime date = (DateTime)DateTime;

            (int firstValidIndex, int lastValidIndex) = GetValidIndexRange(SelectedDate);
            int day = dayIndex - firstValidIndex + 1;

            if (SelectedDate.Year == date.Year && SelectedDate.Month == date.Month &&
                date.Day == dayIndex - firstValidIndex + 1)
                return Color!;

            if (SelectedDate.Year == System.DateTime.Now.Date.Year && SelectedDate.Month == System.DateTime.Now.Date.Month &&
                System.DateTime.Now.Date.Day == dayIndex - firstValidIndex + 1)
                return Color!;

            return ThemeManager.CurrentColorScheme.OnSurface;
        }

        private string GetYearStyle()
        {

            return $"display:flex;justify-content: center;";
        }

        private void AddYearRange(int? startYear, int? endYear)
        {
            if (startYear == null)
                startYear = System.DateTime.Now.AddYears(-100).Year;
            if ( (endYear == null))
                endYear = System.DateTime.Now.AddYears(100).Year;
            int index = 0;
            for (int year = (int)startYear; year <= endYear; year++)
            {
                YearList.Add(new YearItem() { Year = year, ListItemId = Guid.NewGuid(), ItemIndex = index++ });
            }
        }

        private async Task OnYearClicked(YearItem year)
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddYears(year.Year - SelectedDate.Year);
            DateTime = SelectedDate;
            await DateChanged.InvokeAsync(DateTime);
            await Refresh();
        }

        private string GetMonthStyle(int month)
        {
            var css = "display:flex;justify-content: center; align-self: center; margin:0 10px 0 10px; ";
            if (DateTime == null)
                return css;

            if (MouseOverMonth == month)
                return css + $"background-color: {ThemeManager.CurrentColorScheme.OnSurface.SetAlpha(0.38).Value}; ";

            return css;
        }

        private async Task OnMonthClicked(int month)
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddMonths(month - SelectedDate.Month);
            DateTime = SelectedDate;
            await DateChanged.InvokeAsync(DateTime);
            await Refresh();
        }

        private ButtonStyle GetMonthButtonStyle(int monthIndex)
        {
            if (DateTime == null)
                return ButtonStyle.LabelOnly;

            var date = (DateTime)DateTime;

            if (SelectedDate.Month == monthIndex)
                return ButtonStyle.Filled;

            return ButtonStyle.LabelOnly;
        }

        private string GetMonthName(int month)
        {
            return Culture.DateTimeFormat.GetAbbreviatedMonthName(month);
        }

        private string GetMonthYear()
        {
            return $"{Culture.DateTimeFormat.GetMonthName(SelectedDate.Month)} {SelectedDate.ToString("yyyy")}";
        }

        private bool IsDayIndexValid(int dayIndex)
        {
            if (DateTime == null)
                return false;

            DateTime date = SelectedDate;

            (int firstValidIndex, int lastValidIndex) = GetValidIndexRange(date);
            if (dayIndex >= firstValidIndex && dayIndex <= lastValidIndex)
                return true;

            return false;
        }

        private (int first,int last) GetValidIndexRange(DateTime date)
        {
            var daysInMonth = System.DateTime.DaysInMonth(date.Year, date.Month);
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var dayOfWeek = firstDayOfMonth.Date.DayOfWeek;
            int firstValidIndex = 1;
            if (FirstDayOfWeek == DayOfWeek.Monday)
                firstValidIndex = (((int)dayOfWeek + 6) % 7) + 1;
            else
                firstValidIndex = (int)dayOfWeek + 1;

            var lastValidIndex = firstValidIndex + daysInMonth - 1;

            return (firstValidIndex, lastValidIndex);   
        }

        private async Task OnDayClicked(int dayIndex)
        {
            if (IsReadOnly || IsDisabled)
                return;

            (int firstValidIndex, int _) = GetValidIndexRange(SelectedDate);

            int day = dayIndex - firstValidIndex + 1;

            DateTime = SelectedDate.AddDays(day - SelectedDate.Day);
            SelectedDate = (DateTime)DateTime;
            await DateChanged.InvokeAsync(DateTime);
            await Refresh();
            await DateTimeSelected.InvokeAsync();
        }

        private ButtonStyle GetDayButtonStyle(int dayIndex)
        {
            if (DateTime == null)
                return ButtonStyle.LabelOnly;

            var date = (DateTime)DateTime;

            (int firstValidIndex, int _) = GetValidIndexRange(SelectedDate);



            if (SelectedDate.Year == date.Year && SelectedDate.Month == date.Month &&
                date.Day == dayIndex - firstValidIndex+1)
                return ButtonStyle.Filled;

            if (SelectedDate.Year == System.DateTime.Now.Date.Year && SelectedDate.Month == System.DateTime.Now.Date.Month &&
                System.DateTime.Now.Date.Day == dayIndex - firstValidIndex + 1)
                return ButtonStyle.Outlined;

            return ButtonStyle.LabelOnly;
        }

        private string GetDay(int dayIndex)
        {
            (int firstValidIndex, int lastValidIndex) = GetValidIndexRange(SelectedDate);
            if (dayIndex >= firstValidIndex && dayIndex <= lastValidIndex)
                return (dayIndex - firstValidIndex + 1).ToString();
            return string.Empty;
        }

        private string GetDayName(int day)
        {
            var names = Culture.DateTimeFormat.AbbreviatedDayNames;
            if (FirstDayOfWeek == DayOfWeek.Sunday)
                return names[day];
            else
                return names[(day + 1) % 7];
        }

        private async Task OnAddYear()
        {            
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddYears(1);
            DateTime = SelectedDate;
            await DateChanged.InvokeAsync(DateTime);
            await Refresh();
        }

        private async Task OnSubtractYear()
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddYears(-1);
            DateTime = SelectedDate;
            await DateChanged.InvokeAsync(DateTime);
            await Refresh();
        }

        private async Task OnSubtractMonth()
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddMonths(-1);
            DateTime = SelectedDate;
            await DateChanged.InvokeAsync(DateTime);
            await Refresh();
        }

        private async Task OnAddMonth()
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddMonths(1);
            DateTime = SelectedDate;
            await DateChanged.InvokeAsync(DateTime);
            await Refresh();
        }

        private async Task GotoMonths()
        {
            if (IsReadOnly || IsDisabled)
                return;

            await Refresh();
        }

        //private int GetBodyWidth()
        //{
        //    if (Orientation == Orientation.Landscape)
        //        return BodyWidthLandscape;
        //    else
        //        return ControlWidthPortrait;
        //}

        //private int GetBodyHeight()
        //{
        //    if (Orientation == Orientation.Landscape)
        //        return ControlHeightLandscape;
        //    else
        //        return BodyHeightPortrait;
        //}

        private async Task OnDateClicked()
        {
            if (IsReadOnly || IsDisabled)
                return;

            await Refresh();
        }

        private async Task OnMouseEnterMonth(int month)
        {
            MouseOverMonth = month;
            await Refresh();
        }
        private async Task OnMouseLeaveMonth()
        {
            MouseOverMonth = null;
            await Refresh();
        }

        private class YearItem:ListItem
        {
            public int Year { get; set; }
        }
    }
}