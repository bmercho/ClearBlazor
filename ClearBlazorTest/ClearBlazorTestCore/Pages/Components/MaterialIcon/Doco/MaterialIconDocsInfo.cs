/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record MaterialIconDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "MaterialIcon";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "MaterialIconApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "MaterialIcon");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Icon", "string", "string.Empty", ""),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Rotation", "double", "0.0", ""),
            new ApiComponentInfo("Colour", "Color", "ThemeManager.CurrentPalette.Dark", ""),
            new ApiComponentInfo("ViewBox", "string", "0 0 24 24", ""),
            new ApiComponentInfo("ToolTip", "string?", "null", ""),
            new ApiComponentInfo("ToolTipPosition", "<a href=ToolTipPositionApi>ToolTipPosition?</a>", "null", ""),
            new ApiComponentInfo("ToolTipDelay", "int?", "null; // Milliseconds", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
