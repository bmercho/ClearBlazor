/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record TimePickerInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "TimePickerInput";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "TimePickerInputApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "TimePickerInput");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<TimeOnly?>", "ContainerInputBase<TimeOnly?>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("TimeFormat", "string", "hh:mm tt", ""),
            new ApiComponentInfo("Hours24", "bool", "false", ""),
            new ApiComponentInfo("MinuteStep", "<a href=MinuteStepApi>MinuteStep</a>", "MinuteStep.One", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", ""),
            new ApiComponentInfo("Position", "<a href=PopupPositionApi>PopupPosition</a>", "PopupPosition.BottomLeft", ""),
            new ApiComponentInfo("Transform", "<a href=PopupTransformApi>PopupTransform</a>", "PopupTransform.TopLeft", ""),
            new ApiComponentInfo("AllowVerticalFlip", "bool", "true", ""),
            new ApiComponentInfo("AllowHorizontalFlip", "bool", "true", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
