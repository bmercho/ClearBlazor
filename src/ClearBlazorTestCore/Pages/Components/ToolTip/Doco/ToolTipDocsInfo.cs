/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToolTipDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ToolTip";
        public string Description {get; set; } = "";
        public (string, string) ApiLink  {get; set; } = ("API", "ToolTipApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ToolTip");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ChildContent", "RenderFragment?", "null", ""),
            new ApiComponentInfo("Text", "string?", "null", ""),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", ""),
            new ApiComponentInfo("Position", "<a href=ToolTipPositionApi>ToolTipPosition?</a>", "null", ""),
            new ApiComponentInfo("Delay", "int?", "null; // Milliseconds", ""),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("GetClasses()", "string", "", ""),
            new ApiComponentInfo("Task ShowToolTip()", "async", "", ""),
            new ApiComponentInfo("HideToolTip()", "void", "", ""),
        };
    }
}
