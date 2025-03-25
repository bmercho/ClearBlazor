/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record IconDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Icon";
        public string Description {get; set; } = "Used to show Material Icons and Custom Icons\r";
        public (string, string) ApiLink  {get; set; } = ("API", "IconApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Icon");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("IconName", "string", "string.Empty", "The name of the icon\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "The size of the icon\r"),
            new ApiComponentInfo("Rotation", "double", "0.0", "The rotation angle of the icon, in degrees\r"),
            new ApiComponentInfo("Color", "Color?", "null", "The color used for the icon.\r"),
            new ApiComponentInfo("ViewBox", "string", "0 0 24 24", "The containing box for the icon\r"),
            new ApiComponentInfo("ToolTip", "string?", "null", "Tooltip string for the button\r"),
            new ApiComponentInfo("ToolTipPosition", "<a href=ToolTipPositionApi>ToolTipPosition?</a>", "null", "Tooltip position (when shown)\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
