/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToolbarDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Toolbar";
        public string Description {get; set; } = "A control with a row of IconButtons that provide quick access to \rfrequently used functions or tools.\r";
        public (string, string) ApiLink  {get; set; } = ("API", "ToolbarApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Toolbar");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IBorder", " IBorderApi"),
            (" IBoxShadow", " IBoxShadowApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", "The child content of this control.\r"),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Landscape", "Orientation of the control.\r"),
            new ApiComponentInfo("TrayOrder", "int", "0", "Used when in ToolbarTray. Gives the order of this toolbar within the tray\r"),
            new ApiComponentInfo("NewLine", "bool", "false", "Used when in ToolbarTray. Indicates that this toolbar should be displayed on the next line.\r"),
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
