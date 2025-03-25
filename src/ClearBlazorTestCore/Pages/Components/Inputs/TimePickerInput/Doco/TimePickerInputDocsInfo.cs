/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TimePickerInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TimePickerInput";
        public string Description {get; set; } = "TimePickerInput is a component for selecting time with customizable formats, 24-hour or 12-hour modes, and\rvarious display options.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "TimePickerInputApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TimePickerInput");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<TimeOnly?>", "ContainerInputBase<TimeOnly?>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("TimeFormat", "string", "hh:mm tt", "Specifies the format of the time. The default format is 'hh:mm tt'.\r"),
            new ApiComponentInfo("Hours24", "bool", "false", "Indicates whether the time format is 24-hour. Defaults to false, meaning a 12-hour format is used.\r"),
            new ApiComponentInfo("MinuteStep", "<a href=MinuteStepApi>MinuteStep</a>", "MinuteStep.One", "Defines the step interval for minutes, allowing customization of minute increments. Defaults to a one-minute\rstep.\r"),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", "The orientation of the component. Defaults to portrait.    \r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("Position", "<a href=PopupPositionApi>PopupPosition</a>", "PopupPosition.BottomLeft", "Defines the position of a popup, defaulting to the bottom left corner.\r"),
            new ApiComponentInfo("Transform", "<a href=PopupTransformApi>PopupTransform</a>", "PopupTransform.TopLeft", "Defines the position of a popup relative to its target. The default position is set to the top-left corner.\r"),
            new ApiComponentInfo("AllowVerticalFlip", "bool", "true", "Indicates whether vertical flipping is permitted. Defaults to true.\r"),
            new ApiComponentInfo("AllowHorizontalFlip", "bool", "true", "Indicates whether horizontal flipping is permitted. Defaults to true.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
