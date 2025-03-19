/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ColorPickerInputDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ColorPickerInput";
        public string Description {get; set; } = "A color picker input component\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ColorPickerInputApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ColorPickerInput");
        public (string, string) InheritsLink {get; set; } = ("ContainerInputBase<Color>", "ContainerInputBase<Color>Api");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBackground", " IBackgroundApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ShowHex", "bool", "false", "Indicates whether to display the color in hexadecimal format. Defaults to false.\r"),
            new ApiComponentInfo("BackgroundColor", "Color?", "null", "See <a href=\"IBackgroundApi\">IBackground</a>\r"),
            new ApiComponentInfo("Position", "<a href=PopupPositionApi>PopupPosition</a>", "PopupPosition.BottomLeft", "Defines the position of a popup, defaulting to the bottom left corner. \r"),
            new ApiComponentInfo("Transform", "<a href=PopupTransformApi>PopupTransform</a>", "PopupTransform.TopLeft", "Defines the position of a popup relative to its target. The default position is set to the top-left corner.\r"),
            new ApiComponentInfo("AllowVerticalFlip", "bool", "true", "Indicates whether vertical flipping is permitted. Defaults to true.\r"),
            new ApiComponentInfo("AllowHorizontalFlip", "bool", "true", "Indicates whether horizontal flipping is permitted. Defaults to true.\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
