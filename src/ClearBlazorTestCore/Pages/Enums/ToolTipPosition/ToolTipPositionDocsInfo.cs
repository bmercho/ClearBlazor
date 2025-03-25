/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record ToolTipPositionDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "ToolTipPosition";
        public string Description {get; set; } = "Defines the location of the tool tip relative to the component that the tool tip is for.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Bottom", "ToolTipPosition", "Bottom of component\r"),
            new ApiFieldInfo("Top", "ToolTipPosition", "Top of component\r"),
            new ApiFieldInfo("Left", "ToolTipPosition", "Left of component\r"),
            new ApiFieldInfo("Right", "ToolTipPosition", "Right of component\r"),
        };
    }
}
