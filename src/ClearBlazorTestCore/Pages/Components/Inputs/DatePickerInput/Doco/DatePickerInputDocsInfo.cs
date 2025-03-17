/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DatePickerInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "DatePickerInput";
        public string Description {get; set; } = "A date picker input component\r";
        public (string, string) ApiLink  {get; set; } = ("API", "DatePickerInputApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "DatePickerInput");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<DateOnly?>", "ContainerInputBase<DateOnly?>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("DateFormat", "string", "dd MMM yyyy", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Portrait", ""),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
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
