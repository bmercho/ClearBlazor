/// This file is auto-generated. Do not change manually

using ClearBlazor.Common;
namespace ClearBlazorTest
{
    public record DockDocsInfo:IOtherDocsInfo
    {
        public string Name { get; set; } = "Dock";
        public string Description {get; set; } = "";
        public List<ApiFieldInfo> FieldApi {get; set; } = new List<ApiFieldInfo>
        {
            new ApiFieldInfo("Left", "Dock", ""),
            new ApiFieldInfo("Top", "Dock", ""),
            new ApiFieldInfo("Right", "Dock", ""),
            new ApiFieldInfo("Bottom", "Dock", ""),
        };
    }
}
