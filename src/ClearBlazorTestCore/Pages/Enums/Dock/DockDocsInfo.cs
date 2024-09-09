/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DockDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Dock";
        public string Description {get; set; } = "Used by DockPanel to indicate which side a child is docked to. \rIf a child does not have Dock specified it uses the remaining avialable space.\r";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Left", "Dock", "Dock to the left side.\r"),
            new ApiFieldInfo("Top", "Dock", "Dock to the top side.\r"),
            new ApiFieldInfo("Right", "Dock", "Dock to the right side.\r"),
            new ApiFieldInfo("Bottom", "Dock", "Dock to the bottom side.\r"),
        };
    }
}
