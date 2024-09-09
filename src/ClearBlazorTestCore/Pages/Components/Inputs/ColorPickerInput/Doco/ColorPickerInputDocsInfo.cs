/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ColorPickerInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ColorPickerInput";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ColorPickerInputApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ColorPickerInput");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<Color>", "ContainerInputBase<Color>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ShowHex", "bool", "false", ""),
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
