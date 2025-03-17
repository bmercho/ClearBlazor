/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DatePickerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "DatePicker";
        public string Description {get; set; } = "Control to select a date.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "DatePickerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "DatePicker");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("IBorder", "IBorderApi"),
            ("IBackground", "IBackgroundApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Date", "DateOnly?", "null", "The initially selected date \r"),
            new ApiComponentInfo("DateChanged", "EventCallback<DateOnly?>", "", "Event raised when the date selection has changed.Used for two way binding.\r"),
            new ApiComponentInfo("DateSelected", "EventCallback", "", "Event raised when the date selection has changed.\r"),
            new ApiComponentInfo("FirstDayOfTheWeek", "<a href=FirstDayOfTheWeekApi>FirstDayOfTheWeek?</a>", "null", "Customizes what the first day of the week is. Normally either Sun or Mon.\rDefault is Sun.\r"),
            new ApiComponentInfo("FirstYear", "int?", "null", "First year available for selection\r"),
            new ApiComponentInfo("LastYear", "int?", "null", "Last year available for selection\r"),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", "The orientation of the control\r"),
            new ApiComponentInfo("Culture", "CultureInfo", "CultureInfo.InvariantCulture", "The culture for the control. Affects the names of the days of the week.\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "ThemeManager.CurrentColorScheme.SurfaceContainerHighest", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("BorderThickness", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderColor", "Color?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("CornerRadius", "string?", "null", "See <a href=\"IBorderApi\">IBorder</a>\r"),
            new ApiComponentInfo("BoxShadow", "int?", "null", "See <a href=\"IBoxShadowApi\">IBoxShadow</a>\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
