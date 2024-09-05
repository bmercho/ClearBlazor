using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Globalization;

namespace ClearBlazor
{
    public partial class DatePicker : InputBase,IBorder
    {
        public enum DatePickerMode
        {
            Year,Month,Day
        }

        [Parameter]
        public DateOnly? Date { get; set; }
        [Parameter]
        public EventCallback<DateOnly?> DateChanged { get; set; }

        [Parameter]
        public EventCallback DateSelected { get; set; }

        [Parameter]
        public FirstDayOfTheWeek? FirstDayOfTheWeek { get; set; }
        [Parameter]
        public int? StartYear { get; set; }
        [Parameter]
        public int? EndYear { get; set; }


        [Parameter]
        public Orientation Orientation { get; set; } = Orientation.Portrait;

        [Parameter]
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;

        [Parameter]
        public string? BorderThickness { get; set; }

        [Parameter]
        public Color? BorderColor { get; set; }

        [Parameter]
        public BorderStyle? BorderStyle { get; set; }

        [Parameter]
        public string? CornerRadius { get; set; }


        [Parameter]
        public int? BoxShadow { get; set; } = null;

        const int ControlWidthPortrait = 270;
        const int ControlWidthLandscape = 420;

        private DatePickerMode Mode = DatePickerMode.Day;
        private List<int> YearList { get; set; } = new();

        ScrollViewer ScrollViewer = null!;
        private int? MouseOverMonth = null;
        private int? MouseOverDay = null;
        private DayOfWeek FirstDayOfWeek = DayOfWeek.Monday;
        private DateOnly SelectedDate;

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            if (Color == null)
                Color = Color.Primary;
            if (Date == null)   
                Date = DateOnly.FromDateTime(DateTime.Now);
            AddYearRange(StartYear, EndYear);

            FirstDayOfWeek = GetFirstDayOfWeek();
            DateOnly date = (DateOnly)Date;
            SelectedDate = date;
        }

        private int GetYearIndex()
        {
            int? year = Date?.Year;

            int startYear;
            if (StartYear == null)
                startYear = DateTime.Now.AddYears(-100).Year;
            else
                startYear = (int)StartYear;

            if (year != null)
                return  (int)year - startYear + 1;
            return 0;

        }

        protected override string UpdateStyle(string css)
        {
            if (Orientation == Orientation.Portrait)
                css += $"display : grid; width:{ControlWidthPortrait}px; ";
            else
                css += $"display : grid; width:{ControlWidthLandscape}px; ";
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
                return Color.Dark;
        }

        private Color GetMonthColor(int month)
        {
            if (month == SelectedDate.Month)
                return Color!;
            else
                return Color.Dark;
        }

        private Color GetDayColor(int dayIndex)
        {
            if (Date == null)
                return Color.Dark;

            var date = (DateOnly)Date;

            (int firstValidIndex, int lastValidIndex) = GetValidIndexRange(SelectedDate);
            int day = dayIndex - firstValidIndex + 1;

            if (SelectedDate.Year == date.Year && SelectedDate.Month == date.Month &&
                date.Day == dayIndex - firstValidIndex + 1)
                return Color!;

            if (SelectedDate.Year == DateTime.Now.Date.Year && SelectedDate.Month == DateTime.Now.Date.Month &&
                DateTime.Now.Date.Day == dayIndex - firstValidIndex + 1)
                return Color!;

            return Color.Dark;
        }

        private string GetYearStyle()
        {

            return $"display:flex;justify-content: center;";
        }

        private void AddYearRange(int? startYear, int? endYear)
        {
            if (startYear == null)
                startYear = DateTime.Now.AddYears(-100).Year;
            if ( (endYear == null))
                endYear = DateTime.Now.AddYears(100).Year;
            for (int year = (int)startYear; year <= endYear; year++)
                YearList.Add(year);
        }

        private void OnYearClicked(int year)
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddYears(year - SelectedDate.Year);
            Mode = DatePickerMode.Month;
            StateHasChanged();
        }

        private string GetMonthStyle(int month)
        {
            var css = "display:flex;justify-content: center; align-self: center; margin:0 10px 0 10px; ";
            if (Date == null)
                return css;

            if (MouseOverMonth == month)
                return css + $"background-color: {ThemeManager.CurrentPalette.ListBackgroundColor.Value}; ";

            return css;
        }

        private void OnMonthClicked(int month)
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddMonths(month - SelectedDate.Month);
            Mode = DatePickerMode.Day;
            StateHasChanged();
        }

        private string GetMonthName(int month)
        {
            return Culture.DateTimeFormat.GetAbbreviatedMonthName(month);
        }
        private string GetMonthName()
        {
            return Culture.DateTimeFormat.GetAbbreviatedMonthName(SelectedDate.Month);
        }

        private string GetMonthYear()
        {
            return $"{Culture.DateTimeFormat.GetMonthName(SelectedDate.Month)} {SelectedDate.ToString("yyyy")}";
        }

        private string GetMonthSize(int month)
        {
            if (Date == null)
                return "16px";

            if (month == Date?.Month)
                return "22px";

            return "16px";
        }

        private bool IsDayIndexValid(int dayIndex)
        {
            if (Date == null)
                return false;

            DateOnly date = (DateOnly)Date;

            (int firstValidIndex, int lastValidIndex) = GetValidIndexRange(date);
            if (dayIndex >= firstValidIndex && dayIndex <= lastValidIndex)
                return true;

            return false;
        }

        private (int first,int last) GetValidIndexRange(DateOnly date)
        {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);
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

            Date = SelectedDate.AddDays(day - SelectedDate.Day);
            SelectedDate = (DateOnly)Date;
            await DateChanged.InvokeAsync(Date);
            StateHasChanged();
            await DateSelected.InvokeAsync();

        }

        private TextEditFillMode GetDayButtonStyle(int dayIndex)
        {
            if (Date == null)
                return TextEditFillMode.None;

            var date = (DateOnly)Date;

            (int firstValidIndex, int _) = GetValidIndexRange(SelectedDate);



            if (SelectedDate.Year == date.Year && SelectedDate.Month == date.Month &&
                date.Day == dayIndex - firstValidIndex+1)
                return TextEditFillMode.Filled;

            if (SelectedDate.Year == DateTime.Now.Date.Year && SelectedDate.Month == DateTime.Now.Date.Month &&
                DateTime.Now.Date.Day == dayIndex - firstValidIndex + 1)
                return TextEditFillMode.Outline;

            return TextEditFillMode.None;
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

        private void OnAddYear()
        {            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddYears(1);
            StateHasChanged();
        }

        private void OnSubtractYear()
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddYears(-1);
            StateHasChanged();
        }

        private void OnSubtractMonth()
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddMonths(-1);
            StateHasChanged();
        }

        private void OnAddMonth()
        {
            if (IsReadOnly || IsDisabled)
                return;

            SelectedDate = SelectedDate.AddMonths(1);
            StateHasChanged();
        }

        private void OnGotoMonths()
        {
            Mode = DatePickerMode.Month;
            StateHasChanged();
        }

        
        private int GetBodyWidth()
        {
            return ControlWidthPortrait;
        }

        private int GetBodyHeight()
        {
            return ControlWidthPortrait;
        }

        private void OnDateClicked()
        {
            if (IsReadOnly || IsDisabled)
                return;

            Mode = DatePickerMode.Year;
            StateHasChanged();
        }

        private void OnMouseEnterMonth(int month)
        {
            MouseOverMonth = month;
            StateHasChanged();
        }
        private void OnMouseLeaveMonth()
        {
            MouseOverMonth = null;
            StateHasChanged();
        }
        private void OnMouseEnterDay(int dayIndex)
        {
            MouseOverDay = dayIndex;
            StateHasChanged();
        }
        private void OnMouseLeaveDay()
        {
            MouseOverDay = null;
            StateHasChanged();
        }
    }
}