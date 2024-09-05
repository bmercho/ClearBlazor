/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToolbarDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "Toolbar";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ToolbarApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "Toolbar");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
            (" IContent", " IContentApi"),
            ("IBorder", "IBorderApi"),
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("BorderThickness", "string?", "null", ""),
            new ApiComponentInfo("BorderColour", "Color?", "null", ""),
            new ApiComponentInfo("BorderStyle", "<a href=BorderStyleApi>BorderStyle?</a>", "null", ""),
            new ApiComponentInfo("CornerRadius", "string?", "null", ""),
            new ApiComponentInfo("BoxShadow", "int?", "null", ""),
            new ApiComponentInfo("Orientation", "<a href=OrientationApi>Orientation</a>", "Orientation.Landscape", ""),
            new ApiComponentInfo("OverflowMode", "<a href=OverflowModeApi>OverflowMode</a>", "OverflowMode.Wrap", ""),
            new ApiComponentInfo("in", "When", "null", ""),
            new ApiComponentInfo("NewLine", "bool", "false", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
        };
    }
}
