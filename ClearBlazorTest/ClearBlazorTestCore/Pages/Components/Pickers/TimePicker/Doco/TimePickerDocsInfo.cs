/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TimePickerDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TimePicker";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "TimePickerApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TimePicker");
        public (string, string) InheritsLink {get; set; } = ("InputBase", "InputBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Time", "TimeOnly?", "null", ""),
            new ApiComponentInfo("TimeChanged", "EventCallback<TimeOnly?>", "", ""),
            new ApiComponentInfo("MinuteSelected", "EventCallback", "", ""),
            new ApiComponentInfo("Hours24", "bool", "false", ""),
            new ApiComponentInfo("MinuteStep", "<a href=MinuteStepApi>MinuteStep</a>", "MinuteStep.One", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColour", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("SetMode(PickerMode mode)", "void", "", ""),
            new ApiComponentInfo("Task PaintCanvas(Batch2D context)", "async", "", ""),
        };
    }
}
