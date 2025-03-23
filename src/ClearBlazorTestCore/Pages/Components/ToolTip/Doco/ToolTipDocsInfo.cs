/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToolTipDocsInfo:IComponentDocsInfo
    {
        public string Name { get; set; } = "ToolTip";
        public string Description {get; set; } = "A control that provides additional context for a UI element. \r";
        public (string, string) ApiLink  {get; set; } = ("API", "ToolTipApi");
        public (string, string) ExamplesLink {get; set; } = ("Examples", "ToolTip");
        public (string, string) InheritsLink {get; set; } = ("ClearComponentBase", "ClearComponentBaseApi");
        public List<(string, string)> ImplementsLinks {get; set; } = new()
        {
        };
        public List<ApiComponentInfo> ParameterApi {get; set; } = new List<ApiComponentInfo>
        {
            new ApiComponentInfo("Text", "string?", "null", "Text shown in tooltip\r"),
            new ApiComponentInfo("Size", "<a href=SizeApi>Size</a>", "Size.Normal", "Size of tooltip\r"),
            new ApiComponentInfo("ToolTipPosition", "<a href=ToolTipPositionApi>ToolTipPosition?</a>", "null", "Position of tooltip\r"),
            new ApiComponentInfo("Delay", "int?", "null", "The delay in milliseconds before the tooltip is shown\r"),
        };
        public List<ApiComponentInfo> MethodApi {get; set; } =  new List<ApiComponentInfo>
        {
            new ApiComponentInfo("ShowToolTip()", "void", "", "Shows the tooltip\r"),
            new ApiComponentInfo("HideToolTip()", "void", "", "Hides the tooltip\r"),
        };
    }
}
