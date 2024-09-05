/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DatePickerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "DatePicker";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "DatePickerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "DatePicker");
        public (string, string) InheritsLink {get; set; } = ("", "Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            ("InputBase", "InputBaseApi"),
            ("IBorder", "IBorderApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Date", "DateOnly?", "null", ""),
            new ApiComponentInfo("DateChanged", "EventCallback<DateOnly?>", "", ""),
            new ApiComponentInfo("DateSelected", "EventCallback", "", ""),
            new ApiComponentInfo("FirstDayOfTheWeek", "<a href=FirstDayOfTheWeekApi>FirstDayOfTheWeek?</a>", "null", ""),
            new ApiComponentInfo("StartYear", "int?", "null", ""),
            new ApiComponentInfo("EndYear", "int?", "null", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", ""),
            new ApiComponentInfo("Culture", "CultureInfo", "CultureInfo.InvariantCulture", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColour", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
